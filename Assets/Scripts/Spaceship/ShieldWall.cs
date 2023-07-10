using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShieldWall : MonoBehaviour
{
    public float damageInterval = 1f;
    public float afterDamage = 0f;
    public GameObject shieldTurret;
    

    // Start is called before the first frame update
    void Start()
    {
        afterDamage = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)   // 충돌 중에는 1초마다 데미지가 깎인다.
    {
        
        if (afterDamage > damageInterval)
        {
            afterDamage = 0f;   // 쉴드 터렛에서 체력 감소
            
            ShieldTurret st = shieldTurret.GetComponent<ShieldTurret>();
            st.shieldHealth--;

        }
        else
        {
            afterDamage += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "FX_Fireball_Shooting_Straight_01")
        {
            ShieldTurret st = shieldTurret.GetComponent<ShieldTurret>();
            st.shieldHealth -= 3;
        }
    }
}
