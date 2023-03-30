using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTurret : MonoBehaviour
{
    public float shieldHealth = 20f;
    public GameObject sheildWall;
    public GameObject beacon;
    public float shieldReload = 20f;
    public float afterShieldDestroyed;

    public bool shieldOn;
    // Start is called before the first frame update
    void Start()
    {
        afterShieldDestroyed = 0f;
        shieldOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldOn && shieldHealth <= 0f)
        {
            sheildWall.SetActive(false);
            beacon.SetActive(false);
            shieldOn = false;

        }

        if (!shieldOn)
        {
            afterShieldDestroyed += Time.deltaTime;
            
            if (afterShieldDestroyed > shieldReload) {
                afterShieldDestroyed = 0f;
                shieldHealth = 20f;
                
                sheildWall.SetActive(true);
                beacon.SetActive(true);
                shieldOn = true;
            }
        }
    }

    
}
