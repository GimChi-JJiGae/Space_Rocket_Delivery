using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int counter;
    public GameObject[] enemies;
    public GameObject target;
    public GameObject explosionVFX;
    public float speed = 5f;

    void Start()
    {
        InvokeRepeating("spawnEnemy", 0, 1f);
    }

    public void spawnEnemy()
    {

        // Calculate random position for enemy to spawn around the target
        float minDistance = 80;
        float maxDistance = 100;

        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0; // Keep y value constant
        randomDirection.Normalize();

        Vector3 spawnPos = target.transform.position + randomDirection * Random.Range(minDistance, maxDistance);
        spawnPos.y = target.transform.position.y; // Set y value to target's y value

        // Instantiate enemy object at random position
        // ...

        // Instantiate enemy object at random position
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPos, Quaternion.identity);
        enemy.layer = LayerMask.NameToLayer("Enemy"); // Assign the 'Enemy' layer to the enemy object

        // ...


        // Add Collider and Rigidbody to the enemy
        BoxCollider collider = enemy.AddComponent<BoxCollider>();
        collider.size = new Vector3(5, 2, 7);
        Rigidbody rb = enemy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        // Calculate direction and velocity towards target
        Vector3 direction = (target.transform.position - enemy.transform.position).normalized;
        Vector3 velocity = direction * speed;

        // Set velocity and freeze rotation of the enemy object
        rb.velocity = velocity;
        rb.freezeRotation = true;

        // Rotate the enemy to face the target and then rotate 180 degrees
        enemy.transform.rotation = Quaternion.LookRotation(direction);

        // Set up OnCollisionEnter function to destroy enemy on collision
        EnemyController controller = enemy.AddComponent<EnemyController>();
        controller.spawner = this;
    }
}

public class EnemyController : MonoBehaviour
{
    public Spawner spawner;
    public float speed = 5f;
    private bool hasExploded = false;
    public CameraShake cameraShake; // Add this line

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
        if (!hasExploded)
        {
            // Instantiate the VFX at the enemy's position and rotation
            GameObject vfxInstance = Instantiate(spawner.explosionVFX, transform.position, transform.rotation);
            hasExploded = true;

            // Automatically destroy the VFX instance after the duration of the particle system
            ParticleSystem vfxParticleSystem = vfxInstance.GetComponent<ParticleSystem>();
            Destroy(vfxInstance, vfxParticleSystem.main.duration);
        }

        if (cameraShake != null)
        {
            cameraShake.Shake();
        }
        else
        {
            Debug.LogError("CameraShake component not found on the main camera.");
        }
        spawner.counter++;
        Destroy(gameObject);

        Attack(collision);
    }

    // 공격
    void Attack(Collision collision)
    {
        Module module = collision.gameObject.GetComponentInParent<Module>();
        // Debug.Log("맞았다!" + module.idxX + module.idxZ);
        module.Attacked();
    }
}
