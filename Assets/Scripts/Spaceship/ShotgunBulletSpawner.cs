using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRate = 1f;
    private float timeAfterSpawn;
    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn > spawnRate)
        {
            timeAfterSpawn = 0;

            GameObject shotgunBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}
