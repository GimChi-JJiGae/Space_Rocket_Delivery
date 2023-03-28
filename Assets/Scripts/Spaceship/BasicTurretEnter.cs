using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicTurretEnter : MonoBehaviour
{
    public GameObject enteredPlayer;
    public bool isUserIn = false;

    public GameObject MainCam;

   

    Vector3 SittingPosition = new Vector3(-1.23f, 2.2f, 4.31f);
    // Start is called before the first frame update
    void Start()
    {
        GameObject maincam = GameObject.Find("Main Camera");
        MainCam = maincam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("여기충돌했어!");
            if (Input.GetKeyDown(KeyCode.Space) && !isUserIn)
            {
                isUserIn = true;                                // 유저가 이미 들어가있음을 확인
                other.transform.position = SittingPosition;
                enteredPlayer = other.gameObject;               // 나중에 다른 스크립트에서 유저 잠금 해제를 위해 퍼블릭으로 저장해두자
                PlayerMovement controller = other.GetComponent<PlayerMovement>();
                controller.enabled = false;                                 // 유저의 컨트롤 잠금

                Transform parent = transform.parent;
                Transform turretHead;
                
                foreach (Transform child in parent)
                {
                    Debug.Log("체크됨?");
                    if (child.name == "TurretHead")    // 터렛 머리 찾기
                    {
                        turretHead = child;
                        
                        turretHead.GetComponent<BasicTurretSpinController>().enabled = true;

                        Debug.Log("터렛머리 찾음!");

                        foreach (Transform obj in child)
                        {
                            if (obj.name == "TurretCam")
                            {
                                Debug.Log("test");
                                GameObject TurretCam = obj.gameObject;
                                TurretCam.SetActive(true);

                                
                                MainCam.SetActive(false);


                            }
                            if (obj.name == "TurretShooting")
                            {
                                BasicTurretController basicTurretController = obj.GetComponent<BasicTurretController>();
                                basicTurretController.enabled = true;

                                foreach (Transform cam in obj)
                                {
                                    if (cam.name == "TurretCam")
                                    {
                                        
                                        GameObject TurretCam = cam.gameObject;
                                        TurretCam.SetActive(true);

                                        GameObject maincam = GameObject.Find("Main Camera");
                                        maincam.SetActive(false);


                                    }
                                }
                            }
                        }
                    }
                }

                

                

            }
        }
    }
}
