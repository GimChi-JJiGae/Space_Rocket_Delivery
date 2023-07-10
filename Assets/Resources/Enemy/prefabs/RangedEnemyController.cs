using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackRange = 50f;
    public float minDistance = 100f;
    public float speed = 3f;
    public float attackInterval = 2f;
    public float health = 1;
    public GameObject target;
    private float nextAttackTime;
    public float targetUpdateInterval = 5f;
    private float nextTargetUpdateTime;
    public float projectileSpeed = 20f;


    void Start()
    {
        target = FindClosestWall();
        nextAttackTime = Time.time + attackInterval;
        nextTargetUpdateTime = Time.time + targetUpdateInterval;
    }

    void Update()
    {
        if (Time.time >= nextTargetUpdateTime)
        {
            target = FindClosestWall();
            nextTargetUpdateTime = Time.time + targetUpdateInterval;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > minDistance)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Vector3 velocity = direction * speed;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = velocity;
            rb.freezeRotation = true;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else if (distance < minDistance)
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

        if (Time.time >= nextAttackTime && distance <= attackRange + 10)
        {
            Attack();
            nextAttackTime = Time.time + attackInterval;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }


    }

    void Attack()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2.0f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Vector3 directionToTarget = (target.transform.position - spawnPosition).normalized;
        projectile.transform.rotation = Quaternion.LookRotation(directionToTarget);

        // 수정된 ProjectileController에 대한 참조를 설정합니다.
        projectile.GetComponent<ProjectileController>().rangedEnemyController = this;

        // 이 부분을 수정하여 발사체의 Rigidbody를 가져온 뒤 속도를 설정합니다.
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = directionToTarget * projectileSpeed;
    }



    GameObject FindClosestWall()
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
