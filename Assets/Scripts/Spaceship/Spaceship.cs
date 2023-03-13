using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public static int rows = 21; // �ٴ��� �� ����
    public static int cols = 21; // �ٴ��� �� ����
    public static int size = 5; // �ٴ��� ũ��

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
        // û���� ��� ����
        for (int z = 0; z < cols; z++)
            for (int x = 0; x < rows; x++)
                if (x < 9 || x > 11 || z < 9 || z > 11)
                    CreateDefaultModule(x, z, Module.ModuleType.Blueprint);
        // ȭ��
        CreateDefaultModule(9, 9, Module.ModuleType.Cargo);
        CreateDefaultModule(11, 11, Module.ModuleType.Cargo);
        CreateDefaultModule(9, 11, Module.ModuleType.Cargo);
        CreateDefaultModule(11, 9, Module.ModuleType.Cargo);

        // �⺻��ž
        CreateDefaultModule(10, 9, Module.ModuleType.DefaultTurret);

        // ���۱�
        CreateDefaultModule(9, 10, Module.ModuleType.Factory);

        // ���ޱ�
        CreateDefaultModule(10, 10, Module.ModuleType.Supplier);

        // ��һ�����
        CreateDefaultModule(11, 10, Module.ModuleType.Oxygenator);

        // ����
        CreateDefaultModule(10, 11, Module.ModuleType.Engine);
    }

    void CreateDefaultModule(int x, int z, Module.ModuleType moduleType)
    {
        // ��ġ ���� �� �⺻ ��� ����
        GameObject modulePrefab;
        modulePrefab = Resources.Load<GameObject>("Spaceship/Module/Module");
        // ��ġ����
        float positionX = -(size * rows) / 2 + (x * size) + (size) / 2;     // -��üũ�� +������ġ +�߾����Ŀ� size/2
        float positionZ = -(size * cols) / 2 + (z * size) + (size) / 2;
        float positionY = 0;
        Vector3 position = new Vector3(positionX, positionY, positionZ);    // �ٴ� Ÿ���� ��ġ
        Quaternion rotation = Quaternion.identity;                           // �ٴ� Ÿ���� ȸ��

        // ��� �����ϰ� space�� �迭�� �Ҵ��Ű��
        GameObject module = Instantiate(modulePrefab, position, rotation);  
        module.transform.parent = transform;            // ��� ��ġ�� Spaceship�Ʒ��� �����ֱ�
        module.name = modulePrefab.name;                // ��� �̸� ����
        modules[z, x] = module;                         // ��� �ֱ�

        // ����� �Ӽ��� �����Ű��
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


