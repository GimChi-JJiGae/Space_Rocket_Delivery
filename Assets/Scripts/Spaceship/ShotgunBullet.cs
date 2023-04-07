using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 20f;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward*speed;
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            EnemyController controller = other.GetComponent<EnemyController>();     // 근거리 적일 경우
            if (controller != null)
            {
               
                controller.health -= damage;
            
            }
            else
            {
                RangedEnemyController Rangedcontroller = other.GetComponent<RangedEnemyController>();   // 원거리 적일 경우

                Rangedcontroller.health -= damage;
                
            }

        }
    }
}
