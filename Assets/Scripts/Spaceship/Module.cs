using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using static Module;

public class Module : MonoBehaviour
{
    public int idxX; // x����
    public int idxZ; // y����
    public enum ModuleType
    {
        Blueprint,      // û����
        Engine,         // ����
        Cargo,          // ȭ��
        Factory,        // ���۱�
        Supplier,       // ������
        Oxygenator,     // ��������
        DefaultTurret,  // �⺻�ͷ�
        LaserTurret,    // �������ͷ�
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


    // �ٴ��� �����ϴ� �Լ�
    public void CreateFloor(ModuleType t)
    {
        moduleType = t;
        GameObject floorPrefab; 
        switch (t)
        {
            case ModuleType.Blueprint:      // û����
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/BlueprintFloor");
                break;
            case ModuleType.Engine:         // ����
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Cargo:          // ȭ��
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;    
            case ModuleType.Factory:        // ���۱�
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Supplier:       // ������
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.Oxygenator:     // ��������
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.DefaultTurret:  // �⺻�ͷ�
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            case ModuleType.LaserTurret:    // �������ͷ�
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
            default:
                floorPrefab = Resources.Load<GameObject>("Spaceship/Module/DefaultFloor");
                break;
        }
        float positionX = gameObject.transform.position.x;     // ���� ������Ʈ�� ��ġ�� ������
        float positionZ = gameObject.transform.position.z;
        float positionY = gameObject.transform.position.y;
        Vector3 position2 = new Vector3(positionX, positionY, positionZ);       // �ٴ� Ÿ���� ��ġ
        Quaternion rotation2 = Quaternion.identity;                             // �ٴ� Ÿ���� ȸ��

        GameObject floorModule = Instantiate(floorPrefab, position2, rotation2);    // �ٴ� ������Ű��
        floorModule.transform.parent = transform;                                   // �ٴ� ��ġ�� Spaceship�Ʒ��� �����ֱ�
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
