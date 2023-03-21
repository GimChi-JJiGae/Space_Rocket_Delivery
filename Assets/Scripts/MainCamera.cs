using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float offsetX = 0f;            // 카메라의 x좌표
    public float offsetY = 2f;           // 카메라의 y좌표
    public float offsetZ = -7f;          // 카메라의 z좌표

    public float CameraSpeed = 3f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치

    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        if (cameraShake != null)
        {
            TargetPos += cameraShake.GetShakeOffset();
        }
        else
        {
            Debug.LogError("CameraShake component not found.");
        }

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }
}