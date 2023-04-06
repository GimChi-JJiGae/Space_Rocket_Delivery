using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicTurretController : MonoBehaviour
{
    public float degree = 15f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        float vertical = Input.GetAxis("Vertical"); // 수직 입력 값

        //if (horizontal != 0 || vertical != 0)
        //{
        //    Debug.Log(horizontal);
        //    Debug.Log(vertical);
        //    Vector3 rotationDirection = new Vector3(-vertical, horizontal, 0f);

        //    // 회전 방향으로 오브젝트 회전
        //    transform.Rotate(rotationDirection * degree * Time.deltaTime);
        //}
        
        if (vertical != 0)
        {
            Vector3 rotationDirection = new Vector3(-vertical, 0, 0f);
            transform.Rotate(rotationDirection * degree * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            gameObject.GetComponent<BasicTurretController>().enabled = false; // 일단 이 스크립트 잠금
            GameObject turretHead = transform.parent.gameObject;
            turretHead.GetComponent<BasicTurretSpinController>().enabled = false; // 터렛 수평회전 스크립트 잠금

            //GameObject maincam = GameObject.Find("Main Camera");
            //maincam.SetActive(true);


            

            


            GameObject basicTurret = turretHead.transform.parent.gameObject;

            foreach (Transform child in basicTurret.transform)     // TurretEnter를 찾아내어 그 안에 있는 스크립트 조정
            {
                if (child.name == "TurretEnter")
                {
                    BasicTurretEnter turretEnter = child.GetComponent<BasicTurretEnter>();
                   
                    PlayerMovement playerMovement = turretEnter.enteredPlayer.GetComponent<PlayerMovement>();
                    
                    turretEnter.isUserIn = false;
                    playerMovement.enabled = true;

                    turretEnter.MainCam.SetActive(true);

                    foreach (Transform cam in transform)
                    {
                        if (cam.name == "TurretCam")
                        {
                            cam.gameObject.SetActive(false);
                        }
                    }


                }
            }

        }
    }
}
