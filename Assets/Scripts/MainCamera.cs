using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    public GameObject Target;
    public float offsetX = 0f;
    public float offsetY = 2f;
    public float offsetZ = -7f;
    public float CameraSpeed = 3f;
    Vector3 TargetPos;
    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = GetComponent<CameraShake>();
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
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

            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
        }
    }

    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            Target = target;
        }
    }
}
