using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBullet : MonoBehaviour
{
    public float bulletSpeed = 80f;
    private Rigidbody bulletRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
        bulletRigidBody.velocity = transform.forward * bulletSpeed;

        Destroy(gameObject, 4f);   // 4초후 자동 소멸 시켜서 리소스 줄이자
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemey")
        {
            Destroy(gameObject);
        }
    }
}
