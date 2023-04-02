// BasicTurretDamage.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretDamage : MonoBehaviour
{
    public int damage = 1; // 기본 데미지 값
    private SkillTreeNode skillTreeNode;

    // Start is called before the first frame update
    void Start()
    {
        skillTreeNode = FindObjectOfType<SkillTreeNode>();
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
