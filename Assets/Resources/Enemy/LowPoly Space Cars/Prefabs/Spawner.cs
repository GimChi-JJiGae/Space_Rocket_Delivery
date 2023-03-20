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
    public GameObject target;
    public GameObject explosionVFX;
    public float speed = 5f;
    public GameObject rangedEnemyPrefab;
    public float rangedEnemySpawnChance = 0.2f;

    void Start()
    {
        InvokeRepeating("spawnEnemy", 0, 0.5f);
    }

    public void spawnEnemy()
    {
        if (counter >= maxEnemies)
        {
            CancelInvoke("spawnEnemy");
            return;
        }

        counter++;

        float minDistance = 80;
        float maxDistance = 100;

        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0;
        randomDirection.Normalize();

        Vector3 spawnPos = target.transform.position + randomDirection * Random.Range(minDistance, maxDistance);
        spawnPos.y = 1;

        int randomIndex = Random.Range(0, enemies.Length);
        GameObject enemy;

        if (Random.value < rangedEnemySpawnChance)
        {
            enemy = Instantiate(rangedEnemyPrefab, spawnPos, Quaternion.identity);
            RangedEnemyController rangedController = enemy.GetComponent<RangedEnemyController>();
            rangedController.target = target;
        }
        else
        {
            enemy = Instantiate(enemies[randomIndex], spawnPos, Quaternion.identity);
            EnemyController controller = enemy.AddComponent<EnemyController>();
            controller.spawner = this;
            controller.health = enemyHealths[randomIndex];
        }
        // 원거리 적 프리팹에 BoxCollider를 추가
        BoxCollider collider = enemy.AddComponent<BoxCollider>();
        collider.size = new Vector3(5, 2, 7); // 필요한 경우 적절한 크기로 조정

        Rigidbody rb = enemy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        Vector3 direction = (target.transform.position - enemy.transform.position).normalized;
        Vector3 velocity = direction * speed;

        rb.velocity = velocity;
        rb.freezeRotation = true;

        enemy.transform.rotation = Quaternion.LookRotation(direction);
    }
}



public class EnemyController : MonoBehaviour
{
    public Spawner spawner;
    public float speed = 5f;
    private bool hasExploded = false;
    public CameraShake cameraShake; // Add this line
    public int health; // 변경된 부분: [SerializeField] private int health; 에서 public int health;


    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Add this line
    }
    void Update()
    {
        if (!hasExploded)
        {
            // Update direction and velocity towards the moving target
            Vector3 direction = (spawner.target.transform.position - transform.position).normalized;
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
        if (collision.gameObject.name == "SM_Bld_Wall_Exterior_Window_01")
        {
            DestroyEnemy();
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

    private void DestroyEnemy()
    {
        if (!hasExploded)
        {
            // Instantiate the VFX at the enemy's position and rotation
            GameObject vfxInstance = Instantiate(spawner.explosionVFX, transform.position, transform.rotation);
            hasExploded = true;

            // Automatically destroy the VFX instance after the duration of the particle system
            ParticleSystem vfxParticleSystem = vfxInstance.GetComponent<ParticleSystem>();
            Destroy(vfxInstance, vfxParticleSystem.main.duration);

            spawner.counter--;
        }
        spawner.spawnEnemy();
        Destroy(gameObject);
    }
}