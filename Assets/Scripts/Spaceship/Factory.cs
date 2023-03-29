using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{

    public Animator popAnimator;

    public enum ResourceType
    {
        Fuel,
        Ore,
    }

    GameObject fuelObject;
    GameObject oreObject;

    ResourceType resourceType = ResourceType.Fuel;

    private GameObject fuelPrefab;
    private GameObject orePrefab;

    public enum PrintType
    {
        Kit,
        Laser,
    }
    
    private GameObject kitObject;
    private GameObject LaserObject;

    PrintType objectType = PrintType.Kit;

    private GameObject kitPrefab; // 프리펩 저장
    private GameObject laserPrefab; // 프리펩 저장

    // Start is called before the first frame update
    private void Start()
    {
        Transform kitTransform;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
