using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 shakeOffset = Vector3.zero;
    private float elapsedTime = 0.0f;

    void Update()
    {
        if (elapsedTime > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            elapsedTime -= Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
            shakeOffset = Vector3.zero;
        }
    }

    public void Shake()
    {
        elapsedTime = shakeDuration;
    }

    public Vector3 GetShakeOffset()
    {
        return shakeOffset;
    }
}