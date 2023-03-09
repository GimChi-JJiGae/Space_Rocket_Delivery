using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    public ModuleType moduleType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    // 바닥을 생성하는 함수
    public void CreateFloor(ModuleType t)
    {
        moduleType = t;
        GameObject floorPrefab; 
        switch (t)
        {
            case ModuleType.Blueprint:      // 청사진
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/BlueprintFloor");
                break;
            case ModuleType.Engine:         // 엔진
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Cargo:          // 화물
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;    
            case ModuleType.Factory:        // 제작기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Supplier:       // 생성기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Oxygenator:     // 산소재생기
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
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
        Vector3 position2 = new Vector3(positionX, positionY, positionZ);       // 바닥 타일의 위치
        Quaternion rotation2 = Quaternion.identity;                             // 바닥 타일의 회전

        GameObject floorModule = Instantiate(floorPrefab, position2, rotation2);    // 바닥 생성시키기
        floorModule.transform.parent = transform;                                   // 바닥 위치를 Spaceship아래로 내려주기
        if (t == Module.ModuleType.Blueprint)
        {
            // gameObject.SetActive(false);
        }
    }

    void UpgradeModule()
    {

    }
    void DeleteModule()
    {

    }
}
