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

    // Edge 체크를 위한 오브젝트
    public GameObject matchObject;
    private GameObject targetObject;

    // Building 체크를 위한 오브젝트
    public GameObject buildingObject;
    private bool isOnBuildingStay = false;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        spaceship = FindAnyObjectByType<Spaceship>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEdgeEnter(other);
        OnBuildingEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnEdgeExit(other);
        OnBuildingExit(other);
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
    public void OnEdgeStay()
    {
        // 모서리안 && 입력 && 블루프린트 모듈일 때
        if (matchObject.tag == "Edge" && targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
        {
            Debug.Log("생성되어라 얍");
            matchObject = gameObject;
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);    // 바닥생성
            spaceship.MakeWall(targetObject);                                           // 벽생성 (연계되어있는 모듈이 많아 우주선에서 관리)
        }
    }

    // 모서리를 나올 때 청사진을 안보여줌
    private void OnEdgeExit(Collider other)
    {
        if (other.gameObject.tag == "Edge")
        {
            Module module = targetObject.GetComponentInParent<Module>();            
            if (module.moduleType == ModuleType.Blueprint)                          // 블루프린터인 상황이면
            {
                targetObject.GetComponent<Module>().floorModule.SetActive(false);   // 바닥을 비활성화시킨다
            }
            matchObject = null;
            targetObject = null;
            
        }
    }

    // 오브젝트 선택
    private void OnBuildingEnter(Collider other)
    {
        if (other.gameObject.tag == "Building")
        {
            buildingObject = other.gameObject;
        }
    }

    // 1초에 한번만 선택할 수 있게
    public void OnBuildingStay()
    {
        if (buildingObject.tag == "Building")
        {
            switch (buildingObject.name)
            {
                case "Supplier":
                    Supplier supplier = buildingObject.GetComponent<Supplier>();
                    supplier.SwitchResource();
                    break;
                case "Engine":
                    break;
                case "Oxygenator":
                    break;
            }
        }
    }



    private void OnBuildingExit(Collider other)
    {
        if (other.gameObject.tag == "Building")
        {
            buildingObject = null;
        }
    }
}