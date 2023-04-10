using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipClone : MonoBehaviour
{
    public static int rows = 21; // 바닥의 행 개수
    public static int cols = 21; // 바닥의 열 개수
    public static int size = 5; // 바닥의 크기

    public GameObject[,] modules = new GameObject[cols, rows];

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SettingModule());
    }

    private IEnumerator SettingModule()
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
        CreateDefaultModule(10, 11, Module.ModuleType.BasicTurret);

        // 제작기
        CreateDefaultModule(9, 10, Module.ModuleType.Factory);

        // 보급기
        CreateDefaultModule(10, 10, Module.ModuleType.Supplier);

        // 산소생성기
        CreateDefaultModule(11, 10, Module.ModuleType.Oxygenator);

        // 엔진
        CreateDefaultModule(10, 9, Module.ModuleType.Engine);

        // Shield
        CreateDefaultModule(10, 12, Module.ModuleType.ShieldTurret);
        CreateDefaultModule(10, 8, Module.ModuleType.ShieldTurret);
        CreateDefaultModule(8, 10, Module.ModuleType.ShieldTurret);
        CreateDefaultModule(12, 10, Module.ModuleType.ShieldTurret);

        // Laser
        CreateDefaultModule(7, 9, Module.ModuleType.LaserTurret);
        CreateDefaultModule(7, 11, Module.ModuleType.LaserTurret);
        CreateDefaultModule(9, 7, Module.ModuleType.LaserTurret);
        CreateDefaultModule(11, 7, Module.ModuleType.LaserTurret);
        CreateDefaultModule(9, 13, Module.ModuleType.LaserTurret);
        CreateDefaultModule(11, 13, Module.ModuleType.LaserTurret);
        CreateDefaultModule(13, 9, Module.ModuleType.LaserTurret);
        CreateDefaultModule(13, 11, Module.ModuleType.LaserTurret);

        // Shotgun
        CreateDefaultModule(7, 10, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(8, 9, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(8, 11, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(10, 13, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(9, 12, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(11, 12, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(13, 10, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(12, 9, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(12, 11, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(10, 7, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(9, 8, Module.ModuleType.ShotgunTurret);
        CreateDefaultModule(11, 8, Module.ModuleType.ShotgunTurret);

        yield return new WaitForSeconds(1.0f);
        CreateDefaultWall();
        yield return null;
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
        Vector3 position = new(positionX, positionY, positionZ);    // 바닥 타일의 위치
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

    void CreateDefaultWall()
    {
        MakeWall(modules[9, 8]);
        MakeWall(modules[9, 7]);
        MakeWall(modules[10, 7]);
        MakeWall(modules[11, 7]);
        MakeWall(modules[11, 8]);

        MakeWall(modules[8, 9]);
        MakeWall(modules[7, 9]);
        MakeWall(modules[7, 10]);
        MakeWall(modules[7, 11]);
        MakeWall(modules[8, 11]);

        MakeWall(modules[9, 12]);
        MakeWall(modules[9, 13]);
        MakeWall(modules[10, 13]);
        MakeWall(modules[11, 13]);
        MakeWall(modules[11, 12]);

        MakeWall(modules[12, 11]);
        MakeWall(modules[13, 11]);
        MakeWall(modules[13, 10]);
        MakeWall(modules[13, 9]);
        MakeWall(modules[12, 9]);
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

    public void DestroyWall(GameObject targetObject)
    {
        Module targetModule = targetObject.GetComponent<Module>();
        Module topModule = modules[targetModule.idxZ + 1, targetModule.idxX].GetComponent<Module>();
        Module bottomModule = modules[targetModule.idxZ - 1, targetModule.idxX].GetComponent<Module>();
        Module leftModule = modules[targetModule.idxZ, targetModule.idxX - 1].GetComponent<Module>();
        Module rightModule = modules[targetModule.idxZ, targetModule.idxX + 1].GetComponent<Module>();

        targetModule.wallTop.SetActive(false);
        targetModule.wallBottom.SetActive(false);
        targetModule.wallRight.SetActive(false);
        targetModule.wallLeft.SetActive(false);
        if (topModule.moduleType != Module.ModuleType.Blueprint)
        {
            topModule.wallBottom.SetActive(true);
        }
        if (bottomModule.moduleType != Module.ModuleType.Blueprint)
        {
            bottomModule.wallTop.SetActive(true);
        }
        if (leftModule.moduleType != Module.ModuleType.Blueprint)
        {
            leftModule.wallRight.SetActive(true);
        }
        if (rightModule.moduleType != Module.ModuleType.Blueprint)
        {
            rightModule.wallLeft.SetActive(true);
        }
    }
}
