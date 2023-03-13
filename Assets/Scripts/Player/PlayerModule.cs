using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Module;

public class PlayerModule : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private Spaceship spaceship;
    private GameObject matchObject;
    private GameObject targetObject;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        spaceship = FindAnyObjectByType<Spaceship>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Edge()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEdgeEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        OnEdgeStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnEdgeExit(other);
    }

    // 모서리에 들어가서 청사진을 보여줌
    private void OnEdgeEnter(Collider other)
    {
        // 모서리에 진입했을 때
        if (other.gameObject.tag == "Edge")
        {
            matchObject = other.gameObject;
            Module module = matchObject.GetComponentInParent<Module>();
            int idxZ = module.idxZ;
            int idxX = module.idxX;
            switch (other.gameObject.name)
            {
                case "EdgeTop":
                    idxZ += 1;
                    break;
                case "EdgeBottom":
                    idxZ -= 1;
                    break;
                case "EdgeRight":
                    idxX += 1;
                    break;
                case "EdgeLeft":
                    idxX -= 1;
                    break;
            }
            // 활성화를 시킨다
            targetObject = spaceship.modules[idxZ, idxX];
            targetObject.GetComponent<Module>().floorModule.SetActive(true);
        }
    }

    // 모서리 안에 있을 때 입력을 체크함
    private void OnEdgeStay(Collider other)
    {
        // 모서리안 && 입력 && 블루프린트 모듈일 때
        if (other.gameObject.tag == "Edge" && playerInput.Interact && targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
        {
            Debug.Log("생성되어라 얍");
            matchObject = other.gameObject;
            // 바닥생성
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);
            // 벽생성 (연계되어있는 모듈이 많아 우주선에서 관리)
            spaceship.MakeWall(targetObject);
        }
    }

    // 모서리를 나올 때 청사진을 안보여줌
    private void OnEdgeExit(Collider other)
    {
        if (other.gameObject.tag == "Edge")
        {
            Module module = targetObject.GetComponentInParent<Module>();
            if (module.moduleType == ModuleType.Blueprint)
            {
                targetObject.GetComponent<Module>().floorModule.SetActive(false);
            }
            matchObject = null;
            targetObject = null;
            
        }
    }
}
