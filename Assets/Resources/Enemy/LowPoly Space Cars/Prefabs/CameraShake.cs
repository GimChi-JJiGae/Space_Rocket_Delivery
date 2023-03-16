using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private float elapsedTime = 0.0f;

    void Start()
    {
        originalPosition = transform.position; // Initialize the original position
    }

    public void Shake()
    {
        elapsedTime = shakeDuration;
    }

    void Update()
    {
        if (elapsedTime > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            elapsedTime -= Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
            transform.position = originalPosition;
        }
    }
}
