using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int counter;
    public int maxEnemies = 20;
    public int[] rangedEnemyHealths;
    public GameObject[] enemies;
    public int[] enemyHealths;
    public GameObject explosionVFX;
    public float speed = 5f;
    public GameObject rangedEnemyPrefab;
    public float rangedEnemySpawnChance = 0.2f;
    public AudioClip enemyDestroyedSound;

    void Start()
    {
        InvokeRepeating("spawnEnemy", 0, 1f);
    }

    public void spawnEnemy()
    {
        if (counter >= maxEnemies)
        {
            return;
        }

        counter++;

        float minDistance = 80;
        float maxDistance = 100;

        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0;
        randomDirection.Normalize();

        Vector3 spawnPos = transform.position + randomDirection * Random.Range(minDistance, maxDistance);
        spawnPos.y = Random.Range(5f, 10f);

        int randomIndex = Random.Range(0, enemies.Length);
        GameObject enemy;
        GameObject closestWall = FindClosestWall();

        if (Random.value < rangedEnemySpawnChance)
        {
            enemy = Instantiate(rangedEnemyPrefab, spawnPos, Quaternion.identity);
            RangedEnemyController rangedController = enemy.GetComponent<RangedEnemyController>();
            rangedController.target = closestWall;
        }
        else
        {
            enemy = Instantiate(enemies[randomIndex], spawnPos, Quaternion.identity);
            EnemyController controller = enemy.AddComponent<EnemyController>();
            controller.spawner = this;
            controller.health = enemyHealths[randomIndex];
            controller.target = closestWall;
            controller.enemyDestroyedSound = enemyDestroyedSound;

        }
        // 원거리 적 프리팹에 BoxCollider를 추가
        BoxCollider collider = enemy.AddComponent<BoxCollider>();
        if (Random.value < rangedEnemySpawnChance)
        {
            collider.size = new Vector3(0.7f, 0.7f, 0.7f); // 원거리 적의 경우 적절한 크기로 조정
        }
        else
        {
            collider.size = new Vector3(0.5f, 0.5f, 0.5f); // 근거리 적의 경우 적절한 크기로 조정
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
    public int health; // 변경된 부분: [SerializeField] private int health; 에서 public int health;
    public GameObject target;
    public AudioClip enemyDestroyedSound;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Add this line
    }
    void Update()
    {
        if(health < 0)
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

            spawner.counter--;
            spawner.spawnEnemy();
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