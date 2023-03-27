using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Module;

public class Module : MonoBehaviour
{
    public int idxX; // x순번
    public int idxZ; // y순번
    public enum ModuleType
    {
        Blueprint,      // 청사진
        Engine,         // 엔진
        Cargo,          // 화물
        Factory,        // 제작기
        Supplier,       // 생성기
        Oxygenator,     // 산소재생기
        DefaultTurret,  // 기본터렛
        LaserTurret,    // 레이저터렛
        BasicTurret     // 제공되는 커다란 기본 터렛
    }
    public ModuleType moduleType;   // 모듈타입

    public GameObject wallTop;      // 벽 모듈
    public GameObject wallLeft;
    public GameObject wallBottom;
    public GameObject wallRight;

    public GameObject floorModule; // 바닥 모듈

    public GameObject broken1;      // 데미지 레벨에 따른 이펙트
    public GameObject broken2;

    public GameObject hitArea;      // 데미지 받는 범위

    public float hp = 0;

    // Start is called before the first frame update
    void Start()
    {
        //spaceship = FindAnyObjectByType<Spaceship>();
        // 벽 찾기
        Transform wallTransform = transform.Find("Wall");
        wallTop = wallTransform.Find("WallTop").gameObject;
        wallLeft = wallTransform.Find("WallLeft").gameObject;
        wallBottom = wallTransform.Find("WallBottom").gameObject;
        wallRight = wallTransform.Find("WallRight").gameObject;
        wallTop.SetActive(false);
        wallLeft.SetActive(false);
        wallBottom.SetActive(false);
        wallRight.SetActive(false);

        Transform Broken = transform.Find("Broken");
        broken1 = Broken.Find("Broken1").gameObject;
        broken2 = Broken.Find("Broken2").gameObject;
        broken1.SetActive(false);
        broken2.SetActive(false);

        hitArea = transform.Find("HitArea").gameObject;
        hitArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }


    // 모듈 생성
    // 모듈 타입을 받아서, 해당 모듈을 생성시킴
    public void CreateFloor(ModuleType t)
    {
        moduleType = t;                 

        // 예외처리
        if (transform.Find("Floor"))    //바닥이 존재하면 부수고 새로 생성
        {
            GameObject beforeFloor = transform.Find("Floor").gameObject;
            Destroy(beforeFloor);
        }

        
        GameObject floorPrefab;
        // T에 따라 파일 로드
        switch (t)
        {
            case ModuleType.Blueprint:      // 청사진
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/BlueprintFloor");
                break;
            case ModuleType.Engine:         // 엔진
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Engine");
                break;
            case ModuleType.Cargo:          // 화물
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Cargo");
                break;    
            case ModuleType.Factory:        // 제작기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Factory");
                break;
            case ModuleType.Supplier:       // 생성기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Supplier");
                break;
            case ModuleType.Oxygenator:     // 산소재생기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Oxygenator");
                break;
            case ModuleType.DefaultTurret:  // 기본터렛
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Turret");
                break;
            case ModuleType.LaserTurret:    // 레이저터렛
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Turret");
                break;                      
            case ModuleType.BasicTurret:    // 기존 제공 터렛
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/BasicTurret");
                break;
            default:
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
        }

        float positionX = gameObject.transform.position.x;     // 지금 오브젝트의 위치를 가져옴
        float positionZ = gameObject.transform.position.z;      
        float positionY = gameObject.transform.position.y;
        Vector3 position = new Vector3(positionX, positionY, positionZ);        // 바닥 타일의 위치
        Quaternion rotation = Quaternion.identity;                              // 바닥 타일의 회전

        floorModule = Instantiate(floorPrefab, position, rotation);             // 바닥 생성시키기
        floorModule.name = "Floor";                                             // 모듈 이름 변경
        floorModule.transform.parent = transform;                               // 바닥 위치를 Spaceship아래로 내려주기

        hp = 3;
        if (t == ModuleType.Blueprint)
        {
            floorModule.SetActive(false);
        }
    }

    void UpgradeModule()
    {

    }
    void DeleteModule()
    {

    }

    // 공격받음
    public void Attacked()
    {
        hp -= 1;
        if (hp <= 0)
        {
            ResetModule();
        }
        else if (hp <= 1)
        {
            broken2.SetActive(true);
        }
        else if (hp <= 2)
        {
            broken1.SetActive(true);
        }
        
    }

    // 모듈 초기화시키기
    private void ResetModule()
    {
        // 부수기
        Destroy(floorModule);

        // 부서짐 이펙트 초기화
        broken1.SetActive(false);
        broken2.SetActive(false);

        // 바닥 재생성 (청사진)
        CreateFloor(ModuleType.Blueprint); // Blueprint로 다시 생성하기

        // 벽 초기화
        transform.GetComponentInParent<Spaceship>().DestroyWall(gameObject);
    }

    public static implicit operator Module(GameObject v)
    {
        throw new NotImplementedException();
    }
}
