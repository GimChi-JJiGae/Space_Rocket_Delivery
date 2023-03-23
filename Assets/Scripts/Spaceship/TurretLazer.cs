using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretLazer : MonoBehaviour
{
    public GameObject Turret;
    public float rotationSpeed = 0.8f; // 회전 속도
    private Transform nearest = null;

    public bool isOnTarget = false;
    public float maxRotationAngle = 45.0f;      // 최대 회전 각도

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Turret TurretComponent = Turret.GetComponent<Turret>();
        nearest = TurretComponent.Nearest;
        TurnHeadTowardsNearestEnemy(transform.position);

    }

    

    void TurnHeadTowardsNearestEnemy(Vector3 playerPosition)
    {
        
        if (nearest != null)
        {
            Vector3 direction = nearest.position - transform.position; // 다른 물체를 바라보는 방향 벡터 계산

            Quaternion targetRotation = Quaternion.LookRotation(direction); // 다른 물체를 바라보는 새로운 회전값 계산
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // 현재 회전값에서 새로운 회전값까지 보간

            // x축, z축 회전 각도 제한
            Vector3 euler = newRotation.eulerAngles;
            euler.x = Mathf.Clamp(euler.x, -maxRotationAngle, maxRotationAngle);
            euler.z = Mathf.Clamp(euler.z, -maxRotationAngle, maxRotationAngle);
            newRotation = Quaternion.AngleAxis(euler.y, Vector3.up) * Quaternion.AngleAxis(euler.x, Vector3.right) * Quaternion.AngleAxis(euler.z, Vector3.forward);

            transform.rotation = newRotation; // 회전값 적용
            //Debug.Log("현재각" + transform.rotation.y);
            //Debug.Log("목표각" + targetRotation.y);

            if (Math.Abs(transform.rotation.y - targetRotation.y) < 0.1)
            {
                
                isOnTarget = true;
            }
            else
            {
                isOnTarget = false;
            }

            //Vector3 direction = nearestEnemy.position - transform.position;

            //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);

            //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 1f);

            //transform.LookAt(nearestEnemy);

        }
    }
    //void LateUpdate()
    //{
    //    if (nearestEnemy != null)
    //    {
    //        Vector3 targetDir = nearestEnemy.position - transform.position; // 물체를 향하는 방향 벡터
    //        targetDir.y = 0f; // y축 회전각도를 제한하기 위해 y값을 0으로 설정
    //        Quaternion targetRotation = Quaternion.LookRotation(targetDir); // 물체를 향하는 방향으로의 회전값

    //        // x축과 z축의 회전각도를 제한
    //        float xAngle = targetRotation.eulerAngles.x;
    //        float zAngle = targetRotation.eulerAngles.z;
    //        xAngle = Mathf.Clamp(xAngle, -50f, 50f);
    //        zAngle = Mathf.Clamp(zAngle, -50f, 50f);
    //        targetRotation = Quaternion.Euler(xAngle, targetRotation.eulerAngles.y, zAngle);

    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // 천천히 회전하도록 함

    //    }
    //}
}
