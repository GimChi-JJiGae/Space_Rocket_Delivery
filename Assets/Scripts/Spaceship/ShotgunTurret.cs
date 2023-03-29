using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTurret : MonoBehaviour
{
    public float rotationSpeed = 0.8f;          // 회전 속도
    public float maxRotationAngle = 60.0f;      // 최대 회전 각도
    public Transform Nearest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TurnHeadTowardsNearestEnemy(transform.position);
    }

    Transform FindNearestEnemy(Vector3 TurretHeadPosition)
    {
        // Transform nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(TurretHeadPosition, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                Nearest = enemy.transform;
            }
        }

        return Nearest;
    }

    void TurnHeadTowardsNearestEnemy(Vector3 playerPosition)
    {
        Transform nearestEnemy = FindNearestEnemy(playerPosition);
        if (nearestEnemy != null)
        {

       
            Vector3 direction = nearestEnemy.position - transform.position; // 다른 물체를 바라보는 방향 벡터 계산

            Quaternion targetRotation = Quaternion.LookRotation(direction); // 다른 물체를 바라보는 새로운 회전값 계산
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // 현재 회전값에서 새로운 회전값까지 보간

            // x축, z축 회전 각도 제한
            Vector3 euler = newRotation.eulerAngles;
            euler.x = Mathf.Clamp(euler.x, -maxRotationAngle, maxRotationAngle);
            euler.z = Mathf.Clamp(euler.z, -maxRotationAngle, maxRotationAngle);
            newRotation = Quaternion.AngleAxis(euler.y, Vector3.up) * Quaternion.AngleAxis(euler.x, Vector3.right) * Quaternion.AngleAxis(euler.z, Vector3.forward);

            transform.rotation = newRotation; // 회전값 적용




        }
    }
}
