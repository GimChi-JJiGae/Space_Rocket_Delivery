using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    static int rows = 21; // 바닥의 행 개수
    static int cols = 21; // 바닥의 열 개수
    static int size = 5; // 바닥의 크기

    private GameObject[,] modules = new GameObject[cols, rows];

    // Start is called before the first frame update
    void Start()
    {
        GenerateDefaultModule();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateDefaultModule()
    {
        for (int z = 0; z < cols; z++)
        {
            for (int x = 0; x < rows; x++)
            {
                if (x >= 9 && x <= 11 && z >= 9 && z <= 11)
                {

                }
                else
                {
                    CreateDefaultModule(x, z, Module.ModuleType.Blueprint);
                }
            }
        }
        // 화물
        CreateDefaultModule(9, 9, Module.ModuleType.Cargo);
        CreateDefaultModule(11, 11, Module.ModuleType.Cargo);
        CreateDefaultModule(9, 11, Module.ModuleType.Cargo);
        CreateDefaultModule(11, 9, Module.ModuleType.Cargo);

        // 기본포탑
        CreateDefaultModule(10, 9, Module.ModuleType.DefaultTurret);

        // 제작기
        CreateDefaultModule(9, 10, Module.ModuleType.Factory);

        // 보급기
        CreateDefaultModule(10, 10, Module.ModuleType.Supplier);

        // 산소생성기
        CreateDefaultModule(11, 10, Module.ModuleType.Oxygenator);

        // 엔진
        CreateDefaultModule(10, 11, Module.ModuleType.Engine);
    }

    void CreateDefaultModule(int x, int z, Module.ModuleType moduleType)
    {
        // 모듈에 모듈이라는 프리펩을 붙인다. 그 안에 module컴포넌트도 존재한다.
        GameObject modulePrefab;
        modulePrefab = Resources.Load<GameObject>("Spaceship/Module/Module");

        float positionX = -(size * rows) / 2 + (x * size) + (size) / 2; // -전체크기 +놓을위치 +중앙정렬용 size/2
        float positionZ = -(size * cols) / 2 + (z * size) + (size) / 2;
        float positionY = 0;
        Vector3 position = new Vector3(positionX, positionY, positionZ); // 바닥 타일의 위치
        Quaternion rotation = Quaternion.identity; // 바닥 타일의 회전
        GameObject module = Instantiate(modulePrefab, position, rotation); // 모듈 생성시키기
        module.transform.parent = transform; // 모듈 위치를 Spaceship아래로 내려주기
        module.name = modulePrefab.name; // 모듈 이름 변경
        Module moduleStatus = module.GetComponent<Module>();
        moduleStatus.CreateModule(x, z);
        modules[z, x] = module; // 모듈 넣기

        // 모듈 아래에 바닥 프리펩을 적용시킨다.
        GameObject floorPrefab;
        if (moduleType != Module.ModuleType.Blueprint)
        {
            floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
        }
        else
        {
            floorPrefab = Resources.Load<GameObject>("Spaceship/Module/BlueprintFloor");
        }

        Vector3 floorPosition = new Vector3(positionX, positionY, positionZ); // 바닥 타일의 위치
        Quaternion floorRotation = Quaternion.identity; // 바닥 타일의 회전
        GameObject floorModule = Instantiate(floorPrefab, floorPosition, floorRotation); // 모듈 생성시키기
        floorModule.transform.parent = module.transform; // 모듈 위치를 Spaceship아래로 내려주기
    }
}
