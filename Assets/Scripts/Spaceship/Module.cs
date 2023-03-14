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
    }
    public ModuleType moduleType;   // 모듈타입
    //private Spaceship spaceship;    // 우주선 역참조

    public GameObject wallTop;
    public GameObject wallLeft;
    public GameObject wallBottom;
    public GameObject wallRight;

    public GameObject floorModule; // 바닥 모듈
    public GameObject buildingModule; // 건물 모듈

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
    }

    // Update is called once per frame
    void Update()
    {
    }


    // 바닥을 생성하는 함수
    public void CreateFloor(ModuleType t)
    {
        moduleType = t;
        if (transform.Find("Floor"))
        {
            GameObject beforeFloor = transform.Find("Floor").gameObject;
            Destroy(beforeFloor);
        }

        GameObject floorPrefab;
        GameObject buildingPrefab = null;
        switch (t)
        {
            case ModuleType.Blueprint:      // 청사진
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/BlueprintFloor");
                break;
            case ModuleType.Engine:         // 엔진
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Cargo:          // 화물
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Cargo");
                break;    
            case ModuleType.Factory:        // 제작기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Supplier:       // 생성기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Supplier");
                break;
            case ModuleType.Oxygenator:     // 산소재생기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/Oxygenator");
                break;
            case ModuleType.DefaultTurret:  // 기본터렛
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.LaserTurret:    // 레이저터렛
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            default:
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
        }
        float positionX = gameObject.transform.position.x;     // 지금 오브젝트의 위치를 가져옴
        float positionZ = gameObject.transform.position.z;      
        float positionY = gameObject.transform.position.y;
        Vector3 position = new Vector3(positionX, positionY, positionZ);       // 바닥 타일의 위치
        Quaternion rotation = Quaternion.identity;                             // 바닥 타일의 회전

        floorModule = Instantiate(floorPrefab, position, rotation);    // 바닥 생성시키기
        floorModule.name = "Floor";                                               // 모듈 이름 변경
        floorModule.transform.parent = transform;                                 // 바닥 위치를 Spaceship아래로 내려주기

        if (t == ModuleType.Blueprint)
        {
            floorModule.SetActive(false);
        }
    }

    public void CreateBuilding(ModuleType t)
    {
        moduleType = t;
        if (transform.Find("Building"))
        {
            GameObject beforebuilding = transform.Find("Building").gameObject;
            Destroy(beforebuilding);
        }
    }

    void UpgradeModule()
    {

    }
    void DeleteModule()
    {

    }
}
