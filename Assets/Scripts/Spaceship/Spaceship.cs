using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    static int rows = 21; // �ٴ��� �� ����
    static int cols = 21; // �ٴ��� �� ����
    static int size = 5; // �ٴ��� ũ��

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
        // ��⿡ ����̶�� �������� ���δ�. �� �ȿ� module������Ʈ�� �����Ѵ�.
        GameObject modulePrefab;
        modulePrefab = Resources.Load<GameObject>("Spaceship/Module/Module");

        float positionX = -(size * rows) / 2 + (x * size) + (size) / 2; // -��üũ�� +������ġ +�߾����Ŀ� size/2
        float positionZ = -(size * cols) / 2 + (z * size) + (size) / 2;
        float positionY = 0;
        Vector3 position = new Vector3(positionX, positionY, positionZ); // �ٴ� Ÿ���� ��ġ
        Quaternion rotation = Quaternion.identity; // �ٴ� Ÿ���� ȸ��
        GameObject module = Instantiate(modulePrefab, position, rotation); // ��� ������Ű��
        module.transform.parent = transform; // ��� ��ġ�� Spaceship�Ʒ��� �����ֱ�
        module.name = modulePrefab.name; // ��� �̸� ����
        Module moduleStatus = module.GetComponent<Module>();
        moduleStatus.CreateModule(x, z);
        modules[z, x] = module; // ��� �ֱ�

        // ��� �Ʒ��� �ٴ� �������� �����Ų��.
        GameObject floorPrefab;
        if (moduleType != Module.ModuleType.Blueprint)
        {
            floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
        }
        else
        {
            floorPrefab = Resources.Load<GameObject>("Spaceship/Module/BlueprintFloor");
        }

        Vector3 floorPosition = new Vector3(positionX, positionY, positionZ); // �ٴ� Ÿ���� ��ġ
        Quaternion floorRotation = Quaternion.identity; // �ٴ� Ÿ���� ȸ��
        GameObject floorModule = Instantiate(floorPrefab, floorPosition, floorRotation); // ��� ������Ű��
        floorModule.transform.parent = module.transform; // ��� ��ġ�� Spaceship�Ʒ��� �����ֱ�
    }
}
