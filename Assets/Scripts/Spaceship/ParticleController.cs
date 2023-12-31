using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem lazerParticleSystem;

    public float delay = 0.2f; // 딜레이 시간
    public float interval = 7f; // 반복 주기
    public float miniInterval = 0.5f;

    public int damage = 1;

    public float damageInterval = 0.2f;
    public float timeAfterDamaged = 0f;

    public GameObject turretLazer;

    // Start is called before the first frame update
    void Start()
    {
        lazerParticleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(RepeatParticleSystem());
    }
    
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collided with " + other.name);
        if (other.CompareTag("enemy"))
        {
            if (other.TryGetComponent<EnemyController>(out var controller))
            {
                if (timeAfterDamaged > damageInterval)
                {
                    controller.health -= damage;
                    timeAfterDamaged = 0;
                }
                else
                {
                    timeAfterDamaged += Time.deltaTime;
                }
            }
            else
            {
                RangedEnemyController Rangedcontroller = other.GetComponent<RangedEnemyController>();   // 원거리 적일 경우


                if (timeAfterDamaged > damageInterval)
                {
                    Rangedcontroller.health -= damage;
                    timeAfterDamaged = 0;
                }
                else
                {
                    timeAfterDamaged += Time.deltaTime;
                }
            }
            
        }
    }
    IEnumerator RepeatParticleSystem()
    {
        

        while (true)
        {
            //lazerParticleSystem.Stop();
            TurretLazer targetCheck = turretLazer.GetComponent<TurretLazer>();
            yield return new WaitForSeconds(delay); // 딜레이 시간만큼 대기
            if (targetCheck.isOnTarget)
            {
                lazerParticleSystem.Play();
                
                yield return new WaitForSeconds(interval);
            }
            else
            {
                lazerParticleSystem.Stop();
            }
            
            //lazerParticleSystem.Play(); // Particle System 실행
            //    //Debug.Log(targetCheck.isOnTarget);
            //yield return new WaitForSeconds(interval); // 지정된 주기마다 대기
            //lazerParticleSystem.Stop();
            //yield return new WaitForSeconds(interval); // 지정된 주기마다 대기


        }


        //while (true)
        //{
        //    if (targetCheck.isOnTarget)
        //    {
        //        lazerParticleSystem.Play(); // Particle System 실행
        //        //Debug.Log(targetCheck.isOnTarget);
        //        yield return new WaitForSeconds(interval); // 지정된 주기마다 대기

        //    }
        //    else
        //    {
        //        lazerParticleSystem.Stop();
        //        yield return new WaitForSeconds(miniInterval);
        //    }
        //}

    }


}
