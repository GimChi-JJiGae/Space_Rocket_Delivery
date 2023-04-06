using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharacter : MonoBehaviour
{
    public float speed = 20f;
    public Vector3 direction;
    
    private Rigidbody rb;
    private int[] dx  = { -30, 950 };
    private int[] dy = { -50, 500 };


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3((float)Random.Range(-2, 2), (float)Random.Range(-2, 2), (float)Random.Range(-2, 2)) * speed;

        Vector3 tmp = new Vector3((float)Random.Range(-2, 2), (float)Random.Range(-2, 2), (float)Random.Range(-2, 2));
        rb.angularVelocity = tmp;

        //Destroy(rb, 20f);
    }
}
