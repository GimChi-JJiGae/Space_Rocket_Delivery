using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // 플레이어 캐릭터의 Transform
    public float distance = 50.0f; // 카메라와 플레이어 사이의 거리
    public float height = 20.0f; // 카메라의 높이

    void LateUpdate()
    {
        Vector3 targetPosition = player.position - player.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f);
        transform.LookAt(player);
    }
}