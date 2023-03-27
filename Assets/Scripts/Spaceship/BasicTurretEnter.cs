using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicTurretEnter : MonoBehaviour
{

    Vector3 SittingPosition = new Vector3(-1.23f, 2.2f, 4.31f);
    // Start is called before the first frame update
    void Start()
    {
        
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                other.transform.position = SittingPosition;
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

                                GameObject maincam = GameObject.Find("Main Camera");
                                maincam.SetActive(false);


                            }
                            if (obj.name == "TurretShooting")
                            {
                                BasicTurretController basicTurretController = obj.GetComponent<BasicTurretController>();
                                basicTurretController.enabled = true;

                                foreach (Transform cam in obj)
                                {
                                    if (cam.name == "TurretCam")
                                    {
                                        Debug.Log("test");
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
