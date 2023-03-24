using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public AudioClip projectileDestroyedSound;
    public RangedEnemyController rangedEnemyController;
    private Rigidbody rb;

    bool destroyed = false;

    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Module module = collision.gameObject.GetComponentInParent<Module>();
            if (module != null)
            {
                module.Attacked();
            }
            DestroyProjectile();
        }
    }


    void DestroyProjectile()
    {
        AudioSource.PlayClipAtPoint(projectileDestroyedSound, transform.position);
        Destroy(gameObject);
    }

}
