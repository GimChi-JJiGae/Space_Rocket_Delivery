using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackRange = 200f;
    public float minDistance = 200f; // Add this line
    public float speed = 3f; // Add this line
    public float attackInterval = 2f;
    public int health;
    public GameObject target;
    private float nextAttackTime;

    void Start()
    {
        nextAttackTime = Time.time + attackInterval;
    }

    void Update()
    {
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
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (Time.time >= nextAttackTime && distance <= attackRange + 10)
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

        projectile.GetComponent<ProjectileCollision>().target = target;
    }



    GameObject FindClosestWindowTile()
    {
        GameObject[] windowTiles;
        windowTiles = GameObject.FindGameObjectsWithTag("SM_Bld_Wall_Exterior_Window_01");

        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject windowTile in windowTiles)
        {
            float distance = Vector3.Distance(windowTile.transform.position, position);
            if (distance < minDistance)
            {
                closest = windowTile;
                minDistance = distance;
            }
        }

        return closest;
    }
}
