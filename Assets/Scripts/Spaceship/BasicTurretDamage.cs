using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "enemy")
        {
            EnemyController controller = other.GetComponent<EnemyController>();     // 근거리 적일 경우
            if (controller != null)
            {
                controller.health -= 1;
            }
            else
            {
                RangedEnemyController Rangedcontroller = other.GetComponent<RangedEnemyController>();   // 원거리 적일 경우

                Rangedcontroller.health -= 1;
                
            }

        }
    }
}
