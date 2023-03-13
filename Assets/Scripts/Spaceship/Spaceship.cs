using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public static int rows = 21; // 바닥의 행 개수
    public static int cols = 21; // 바닥의 열 개수
    public static int size = 5; // 바닥의 크기

    public GameObject[,] modules = new GameObject[cols, rows];

    // Start is called before the first frame update
    void Start()
    {
        SettingModule();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SettingModule()
    {
        // 청사진 모듈 생성
        for (int z = 0; z < cols; z++)
            for (int x = 0; x < rows; x++)
                if (x < 9 || x > 11 || z < 9 || z > 11)
                    CreateDefaultModule(x, z, Module.ModuleType.Blueprint);
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
        // 위치 지정 및 기본 모듈 세팅
        GameObject modulePrefab;
        modulePrefab = Resources.Load<GameObject>("Spaceship/Module/Module");
        // 위치지정
        float positionX = -(size * rows) / 2 + (x * size) + (size) / 2;     // -전체크기 +놓을위치 +중앙정렬용 size/2
        float positionZ = -(size * cols) / 2 + (z * size) + (size) / 2;
        float positionY = 0;
        Vector3 position = new Vector3(positionX, positionY, positionZ);    // 바닥 타일의 위치
        Quaternion rotation = Quaternion.identity;                           // 바닥 타일의 회전

        // 모듈 생성하고 space의 배열에 할당시키기
        GameObject module = Instantiate(modulePrefab, position, rotation);  
        module.transform.parent = transform;            // 모듈 위치를 Spaceship아래로 내려주기
        module.name = modulePrefab.name;                // 모듈 이름 변경
        modules[z, x] = module;                         // 모듈 넣기

        // 모듈의 속성을 변경시키기
        Module moduleStatus = module.GetComponent<Module>();
        moduleStatus.idxX = x;
        moduleStatus.idxZ = z;
        moduleStatus.CreateFloor(moduleType);
    }

    // Gets
    public int GetRows() { return rows; }
    public int GetCols() { return cols; }
    public int GetSize() { return size; }


    public void MakeWall(GameObject targetObject)
    {
        Module targetModule = targetObject.GetComponent<Module>();
        Module topModule = modules[targetModule.idxZ + 1, targetModule.idxX].GetComponent<Module>();
        Module bottomModule = modules[targetModule.idxZ - 1, targetModule.idxX].GetComponent<Module>();
        Module leftModule = modules[targetModule.idxZ, targetModule.idxX - 1].GetComponent<Module>();
        Module rightModule = modules[targetModule.idxZ, targetModule.idxX + 1].GetComponent<Module>();

        if (topModule.moduleType == Module.ModuleType.Blueprint)
        {
            targetModule.wallTop.SetActive(true);
        }
        else
        {
            targetModule.wallTop.SetActive(false);
            topModule.wallBottom.SetActive(false);
        }
        if (bottomModule.moduleType == Module.ModuleType.Blueprint)
        {
            targetModule.wallBottom.SetActive(true);
        }
        else
        {
            targetModule.wallBottom.SetActive(false);
            bottomModule.wallTop.SetActive(false);
        }
        if (leftModule.moduleType == Module.ModuleType.Blueprint)
        {
            targetModule.wallLeft.SetActive(true);
        }
        else
        {
            targetModule.wallLeft.SetActive(false);
            leftModule.wallRight.SetActive(false);
        }
        if (rightModule.moduleType == Module.ModuleType.Blueprint)
        {
            targetModule.wallRight.SetActive(true);
        }
        else
        {
            targetModule.wallRight.SetActive(false);
            rightModule.wallLeft.SetActive(false);
        }
    }
}


