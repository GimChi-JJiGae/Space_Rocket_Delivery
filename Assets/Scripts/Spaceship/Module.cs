using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void CreateModule(int x, int z)
    {
        idxX = x;
        idxZ = z;
        moduleType = ModuleType.Blueprint;
    }
    void UpgradeModule()
    {

    }
    void DeleteModule()
    {

    }
}
