using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    { 
        GameObject fuel = new("Fuel");
        GameObject gas = new("Gas");
        GameObject upgradeKit = new("UpgradeKit");
        GameObject laser = new("Laser");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCharacter"))
        {
            // 가까이 가면 함수를 실행하는데
        }
    }

    // Update is called once per frame
}
