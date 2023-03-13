using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
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
    public ModuleType moduleType;   // ���Ÿ��
    //private Spaceship spaceship;    // ���ּ� ������

    public GameObject wallTop;
    public GameObject wallLeft;
    public GameObject wallBottom;
    public GameObject wallRight;

    public GameObject floorModule;

    // Start is called before the first frame update
    void Start()
    {
        //spaceship = FindAnyObjectByType<Spaceship>();
        // �� ã��
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
        Vector3 position = new Vector3(positionX, positionY, positionZ);       // �ٴ� Ÿ���� ��ġ
        Quaternion rotation = Quaternion.identity;                             // �ٴ� Ÿ���� ȸ��

        floorModule = Instantiate(floorPrefab, position, rotation);    // �ٴ� ������Ű��
        floorModule.name = "Floor";                                               // ��� �̸� ����
        floorModule.transform.parent = transform;                                 // �ٴ� ��ġ�� Spaceship�Ʒ��� �����ֱ�
        
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
}
