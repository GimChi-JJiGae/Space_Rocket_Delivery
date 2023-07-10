using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int[] enemyHealths;
    public GameObject explosionVFX;
    public float speed = 5f;
    public float spawnInterval = 5f;
    private float nextDifficultyIncreaseTime; // 다음 난이도 증가 시간
    public float difficultyTimeStep = 300f; // 난이도가 증가하는 시간 간격 (초)
    private int difficultyLevel = 0; // 현재 난이도 레벨
    public AudioClip enemyDestroyedSound;
    public GameObject[] enemiesTier2; // 레벨 2에 등장하는 적의 프리팹 배열
    public GameObject[] enemiesTier3; // 레벨 3에 등장하는 적의 프리팹 배열
    public int[] enemyHealthsTier2; // 레벨 2 적 체력 배열
    public int[] enemyHealthsTier3; // 레벨 3 적 체력 배열
    public GameObject[] enemiesTier4; // 레벨 4에 등장하는 적의 프리팹 배열
    public int[] enemyHealthsTier4; // 레벨 4 적 체력 배열



    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());

        nextDifficultyIncreaseTime = Time.time + difficultyTimeStep;
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            spawnEnemy();
            Debug.Log("Enemy spawned at: " + Time.time); // Add this line
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    void Update()
    {
        if (Time.time >= nextDifficultyIncreaseTime)
        {
            difficultyLevel++;
            nextDifficultyIncreaseTime = Time.time + difficultyTimeStep;
        }

        if (difficultyLevel >= 3) // 난이도 레벨이 2 이상인 경우
        {
            spawnInterval = 1f; // 스폰 속도를 1초로 설정
        }
    }




    public void spawnEnemy()
    {
        float minDistance = 80;
        float maxDistance = 100;

        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0;
        randomDirection.Normalize();

        Vector3 spawnPos = transform.position + randomDirection * Random.Range(minDistance, maxDistance);
        spawnPos.y = Random.Range(5f, 10f);

        GameObject[] currentEnemies;
        int[] currentEnemyHealths;

        if (difficultyLevel == 0)
        {
            currentEnemies = enemies;
            currentEnemyHealths = enemyHealths;
        }
        else if (difficultyLevel == 1)
        {
            currentEnemies = enemiesTier2;
            currentEnemyHealths = enemyHealthsTier2;
        }
        else if (difficultyLevel == 2)
        {
            currentEnemies = enemiesTier3;
            currentEnemyHealths = enemyHealthsTier3;
        }
        else if (difficultyLevel == 3)
        {
            currentEnemies = enemiesTier4;
            currentEnemyHealths = enemyHealthsTier4;
        }
        else
        {
            Debug.LogError("난이도 레벨이 잘못 설정되었습니다.");
            return;
        }

        if (currentEnemies.Length == 0)
        {
            Debug.LogError("currentEnemies 배열이 비어있습니다.");
            return;
        }

        int randomIndex = Random.Range(0, currentEnemies.Length);
        GameObject enemy;
        GameObject closestWall = FindClosestWall();

        if (closestWall == null)
        {
            Debug.LogError("가장 가까운 벽을 찾을 수 없습니다.");
            return;
        }

        enemy = Instantiate(currentEnemies[randomIndex], spawnPos, Quaternion.identity);

        if (enemy.GetComponent<RangedEnemyController>() != null)
        {
            RangedEnemyController rangedController = enemy.GetComponent<RangedEnemyController>();
            rangedController.target = closestWall;
        }
        else
        {
            EnemyController controller = enemy.AddComponent<EnemyController>();
            controller.spawner = this; // spawner를 설정해주세요.
            controller.health = currentEnemyHealths[randomIndex];
            controller.target = closestWall;
            controller.enemyDestroyedSound = enemyDestroyedSound;
        }

        BoxCollider collider = enemy.AddComponent<BoxCollider>();
        if (enemy.GetComponent<RangedEnemyController>() != null)
        {
            collider.size = new Vector3(0.7f, 0.7f, 0.7f); // 원거리 적의 경우 적절한 크기로 조정
        }
        else
        {
            collider.size = new Vector3(1f, 1f, 1f); // 근거리 적의 경우 적절한 크기로 조정
        }

        Rigidbody rb = enemy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        Vector3 direction = (closestWall.transform.position - enemy.transform.position).normalized;
        Vector3 velocity = direction * speed;

        rb.velocity = velocity;
        rb.freezeRotation = true;

        enemy.transform.rotation = Quaternion.LookRotation(direction);
    }

    public GameObject FindClosestWall() // Add this method
    {
        GameObject[] wallObjects;
        wallObjects = GameObject.FindGameObjectsWithTag("Wall");

        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject wallObject in wallObjects)
        {
            float distance = Vector3.Distance(wallObject.transform.position, position);
            if (distance < minDistance)
            {
                closest = wallObject;
                minDistance = distance;
            }
        }

        return closest;
    }
}



public class EnemyController : MonoBehaviour
{
    public Spawner spawner;
    public float speed = 5f;
    private bool hasExploded = false;
    public CameraShake cameraShake; // Add this line
    public float health; // 변경된 부분: [SerializeField] private int health; 에서 public int health;
    public GameObject target;
    public AudioClip enemyDestroyedSound;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Add this line
    }
    void Update()
    {
        if (health < 0)
        {
            DestroyEnemy();
        }

        if (!hasExploded)
        {
            // Find the closest wall and update the target
            target = spawner.FindClosestWall();

            // Update direction and velocity towards the moving target
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Vector3 velocity = direction * speed;

            // Set the velocity and freeze rotation of the enemy object
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = velocity;
            rb.freezeRotation = true;

            // Rotate the enemy to face the target and then rotate 180 degrees
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // SM_Bld_Wall_Exterior_Window_01 오브젝트에 부딪힌 경우
        if (collision.gameObject.CompareTag("Wall"))
        {
            DestroyEnemy();
            Attack(collision);
        }
        else
        {
            // 체력 감소
            health--;

            if (!hasExploded && health <= 0) // 체력이 0 이하일 때만 파괴
            {
                DestroyEnemy();
            }
        }
    }

    public void DestroyEnemy()
    {
        if (!hasExploded)
        {
            // Instantiate the VFX at the enemy's position and rotation
            GameObject vfxInstance = Instantiate(spawner.explosionVFX, transform.position, transform.rotation);
            hasExploded = true;
            AudioSource.PlayClipAtPoint(enemyDestroyedSound, transform.position);

            // Automatically destroy the VFX instance after the duration of the particle system
            ParticleSystem vfxParticleSystem = vfxInstance.GetComponent<ParticleSystem>();
            Destroy(vfxInstance, vfxParticleSystem.main.duration);
        }
        Destroy(gameObject);
    }

    // 공격
    public void Attack(Collision collision)
    {
        Module module = collision.gameObject.GetComponentInParent<Module>();
        // Debug.Log("맞았다!" + module.idxX + module.idxZ);
        module.Attacked();
    }
}