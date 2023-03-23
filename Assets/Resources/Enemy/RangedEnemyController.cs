using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackRange = 100f;
    public float minDistance = 100f; // Add this line
    public float speed = 3f; // Add this line
    public float attackInterval = 2f;
    public int health;
    public GameObject target;
    private float nextAttackTime;
    public float targetUpdateInterval = 5f; // Add this line
    private float nextTargetUpdateTime; // Add this line

    void Start()
    {
        target = FindClosestWall(); // Update this line
        nextAttackTime = Time.time + attackInterval;
        nextTargetUpdateTime = Time.time + targetUpdateInterval; // Add this line
    }

    void Update()
    {

        if(health == 0)
        {
            Destroy(gameObject);
        }

        if (Time.time >= nextTargetUpdateTime) // Add this block
        {
            target = FindClosestWall();
            nextTargetUpdateTime = Time.time + targetUpdateInterval;
        }
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > minDistance) // Add this block
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Vector3 velocity = direction * speed;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = velocity;
            rb.freezeRotation = true;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else if (distance < minDistance) // Add this block
        {
            Vector3 direction = (transform.position - target.transform.position).normalized;
            Vector3 velocity = direction * speed;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = velocity;
            rb.freezeRotation = true;
            transform.rotation = Quaternion.LookRotation(-direction);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (Time.time >= nextAttackTime && distance <= attackRange)
        {
            Attack();
            nextAttackTime = Time.time + attackInterval;
        }

    }


    void Attack()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2.0f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Calculate the direction from the spawn position to the target
        Vector3 directionToTarget = (target.transform.position - spawnPosition).normalized;

        // Set the projectile's velocity and direction
        projectile.GetComponent<Rigidbody>().velocity = directionToTarget * 10;

        // Rotate the projectile to face the target
        projectile.transform.rotation = Quaternion.LookRotation(directionToTarget);

        // 추가된 코드: 원거리 적에 대한 참조를 설정합니다.
        projectile.GetComponent<ProjectileController>().rangedEnemyController = this;

    }


    public void Attack(Collision collision)
    {
        Module module = collision.gameObject.GetComponentInParent<Module>();
        if (module != null)
        {
            module.Attacked();
        }
    }



    GameObject FindClosestWall() // Add this method
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
