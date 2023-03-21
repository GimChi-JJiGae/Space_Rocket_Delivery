using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // �÷��̾� ĳ������ Transform
    public float distance = 50.0f; // ī�޶�� �÷��̾� ������ �Ÿ�
    public float height = 20.0f; // ī�޶��� ����

    void LateUpdate()
    {
        Vector3 targetPosition = player.position - player.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f);
        transform.LookAt(player);
    }
}