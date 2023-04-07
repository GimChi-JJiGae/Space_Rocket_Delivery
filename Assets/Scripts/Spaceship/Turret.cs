using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Turret : MonoBehaviour
{

    
    //public float spawnRate = 0.2f;              // 0.2초마다 발사되는 안보이는 총알
    //public float timeAfterSpawn = 0f;


    public ParticleSystem lazerParticle;
    public float rotationSpeed = 0.8f;          // 회전 속도
    public float maxRotationAngle = 45.0f;      // 최대 회전 각도

    public Transform Nearest;

    public float startDelay = 1.5f;
    public float afterStartDelay = 0f;
    public GameObject turretLazer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        TurretLazer targetCheck = turretLazer.GetComponent<TurretLazer>();
        TurnHeadTowardsNearestEnemy(transform.position);




        //if (lazerParticle.isPlaying)
        //{
        //    //Debug.Log("레이저 발생");
        //    if (afterStartDelay > startDelay)
        //    {
        //        if (targetCheck.isOnTarget)
        //        {
        //            if (timeAfterSpawn > spawnRate)
        //            {
        //                GameObject bullet = Instantiate(lazerBullet, transform.position, transform.rotation);

        //                bullet.transform.LookAt(Nearest);

        //                timeAfterSpawn = 0f;

        //            }
        //            else
        //            {
        //                timeAfterSpawn += Time.deltaTime;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        afterStartDelay += Time.deltaTime;
        //    }


        //    //}
        //}
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

            //Vector3 targetDir = nearestEnemy.position - transform.position;
            //targetDir.y = 0f;
            //Quaternion targetRotation = Quaternion.LookRotation(targetDir); // 물체를 향하는 방향으로의 회전값
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // 천천히 회전하도록 함

            Vector3 direction = nearestEnemy.position - transform.position; // 다른 물체를 바라보는 방향 벡터 계산

            Quaternion targetRotation = Quaternion.LookRotation(direction); // 다른 물체를 바라보는 새로운 회전값 계산
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // 현재 회전값에서 새로운 회전값까지 보간

            // x축, z축 회전 각도 제한
            Vector3 euler = newRotation.eulerAngles;
            euler.x = Mathf.Clamp(euler.x, -maxRotationAngle, maxRotationAngle);
            euler.z = Mathf.Clamp(euler.z, -maxRotationAngle, maxRotationAngle);
            newRotation = Quaternion.AngleAxis(euler.y, Vector3.up) * Quaternion.AngleAxis(euler.x, Vector3.right) * Quaternion.AngleAxis(euler.z, Vector3.forward);

            transform.rotation = newRotation; // 회전값 적용



            // Vector3 direction = nearestEnemy.position - transform.position;

            // Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);

            // transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 1f);
            //transform.LookAt(nearestEnemy);

        }
    }

    //void LateUpdate()
    //{
    //    Vector3 targetDir = Nearest.position - transform.position; // 물체를 향하는 방향 벡터
    //    targetDir.y = 0f; // y축 회전각도를 제한하기 위해 y값을 0으로 설정
    //    Quaternion targetRotation = Quaternion.LookRotation(targetDir); // 물체를 향하는 방향으로의 회전값

    //    // x축과 z축의 회전각도를 제한
    //    float xAngle = targetRotation.eulerAngles.x;
    //    float zAngle = targetRotation.eulerAngles.z;
    //    xAngle = Mathf.Clamp(xAngle, -50f, 50f);
    //    zAngle = Mathf.Clamp(zAngle, -50f, 50f);
    //    targetRotation = Quaternion.Euler(xAngle, targetRotation.eulerAngles.y, zAngle);

    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // 천천히 회전하도록 함
    //}

}
