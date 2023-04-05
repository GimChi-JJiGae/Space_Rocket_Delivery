using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        BasicTurret,     // 제공되는 커다란 기본 터렛
        ShotgunTurret,   // 샷건터렛
        ShieldTurret
    }

    public InteractionModule interactionModule;

    public GameObject[] players;

    public ModuleType moduleType;   // 모듈타입

    public GameObject wallTop;      // 벽 모듈
    public GameObject wallLeft;
    public GameObject wallBottom;
    public GameObject wallRight;

    public GameObject floorModule; // 바닥 모듈

    public GameObject broken1;      // 데미지 레벨에 따른 이펙트
    public GameObject broken2;

    public GameObject hitArea;      // 데미지 받는 범위
    public GameManager gameManager;

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
        gameManager = FindObjectOfType<GameManager>();
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

        GameObject floorPrefab = t switch
        {
            // 청사진
            ModuleType.Blueprint => Resources.Load<GameObject>("Spaceship/Module/BlueprintFloor"),
            // 엔진
            ModuleType.Engine => Resources.Load<GameObject>("Spaceship/Module/Engine"),
            // 화물
            ModuleType.Cargo => Resources.Load<GameObject>("Spaceship/Module/Cargo"),
            // 제작기
            ModuleType.Factory => Resources.Load<GameObject>("Spaceship/Module/Factory"),
            // 생성기
            ModuleType.Supplier => Resources.Load<GameObject>("Spaceship/Module/Supplier"),
            // 산소재생기
            ModuleType.Oxygenator => Resources.Load<GameObject>("Spaceship/Module/Oxygenator"),
            // 기본터렛
            ModuleType.DefaultTurret => Resources.Load<GameObject>("Spaceship/Module/Turret"),
            // 레이저터렛
            ModuleType.LaserTurret => Resources.Load<GameObject>("Spaceship/Module/Turret"),
            // 기존 제공 터렛
            ModuleType.BasicTurret => Resources.Load<GameObject>("Spaceship/Module/BasicTurret"),
            // 샷건터렛
            ModuleType.ShotgunTurret => Resources.Load<GameObject>("Spaceship/Module/ShotgunTurret"),
            ModuleType.ShieldTurret => Resources.Load<GameObject>("Spaceship/Module/ShieldTurret"),
            _ => Resources.Load<GameObject>("Spaceship/Module/DefaultFloor"),
        };
        float positionX = gameObject.transform.position.x;     // 지금 오브젝트의 위치를 가져옴
        float positionZ = gameObject.transform.position.z;      
        float positionY = gameObject.transform.position.y;
        Vector3 position = new(positionX, positionY, positionZ);        // 바닥 타일의 위치
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

    // 공격받음
    public void Attacked()
    {
        hp -= 1;
        if (hp <= 0)
        {
            CheckGameOverCondition(); // 모듈 파괴 후 게임 오버 조건 확인
            ResetModule();
            Debug.Log('6');
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

    public void Checked()
    {
        if (hp >= 2)
        {
            broken2.SetActive(false);
            broken1.SetActive(false);
        }
        else if (hp >= 1)
        {
            broken2.SetActive(false);
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

        foreach (GameObject player in players)
        {
            if (player.GetComponent<InteractionModule>().respawnObject != null)
            {
                player.GetComponent<PlayerMovement>().Respawn();
            }
        }

        // 벽 초기화
        transform.GetComponentInParent<Spaceship>().DestroyWall(gameObject);
    }

    public static implicit operator Module(GameObject v)
    {
        throw new NotImplementedException();
    }
    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }
    private void CheckGameOverCondition()
    {
        Debug.Log('5');
        switch (moduleType)
        {
            case ModuleType.Engine:
                Debug.Log(8);
                gameManager.EndGame();
                break;
            case ModuleType.Supplier:
                Debug.Log(8);
                gameManager.EndGame();
                break;
            case ModuleType.Oxygenator:
                Debug.Log(8);
                gameManager.EndGame();
                break;
            case ModuleType.Factory:
                Debug.Log(8);
                gameManager.EndGame();
                break;
        }
    }
}
