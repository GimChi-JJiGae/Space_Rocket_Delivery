using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public AudioClip projectileDestroyedSound;
    public RangedEnemyController rangedEnemyController;
    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Module module = collision.gameObject.GetComponentInParent<Module>();
            if (module != null) // 이 줄 추가
            {
                Attack(collision);
                DestroyProjectile();
            }
        }
    }


    void DestroyProjectile()
    {
        AudioSource.PlayClipAtPoint(projectileDestroyedSound, transform.position);
        Destroy(gameObject);
    }
    public void Attack(Collision collision)
    {
        Module module = collision.gameObject.GetComponentInParent<Module>();
        // Debug.Log("맞았다!" + module.idxX + module.idxZ);
        module.Attacked();
    }
}
