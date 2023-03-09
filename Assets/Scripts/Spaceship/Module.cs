using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
