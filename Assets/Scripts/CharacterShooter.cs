using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooter : MonoBehaviour
{
    public GameObject flyingCharacterPrefab;
    public float spawnRate = 30f;

    private float timeAfterSpawn;

    public Vector3 direction;
    private int[] dx = { -50, 950 };
    private int[] dy = { -70, 500 };

    void Start()
    {
        timeAfterSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn > spawnRate)
        {
            timeAfterSpawn = 0f;

            int d = Random.Range(0, 2);
            direction.x = dx[d];
            d = Random.Range(0, 2);
            direction.y = dy[d];
            Vector3 spawnPos = new Vector3(direction.x, direction.y, 800);
            GameObject flyingCharacter = Instantiate(flyingCharacterPrefab, spawnPos, transform.rotation);

            Destroy(flyingCharacter, 20f);
        }
    }
}
