using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public GameObject impactFX;
    public int damage = 1; // 탄환 데미지 설정

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Module module = collision.gameObject.GetComponentInParent<Module>();
            if (module != null)
            {
                for (int i = 0; i < damage; i++)
                {
                    module.Attacked();
                }
            }
        }

        GameObject impact = Instantiate(impactFX, transform.position, transform.rotation);
        Destroy(impact, 2f);
        Destroy(gameObject);
    }
}


