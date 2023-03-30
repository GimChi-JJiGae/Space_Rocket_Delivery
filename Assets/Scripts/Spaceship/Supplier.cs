using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Supplier : MonoBehaviour
{
    Multiplayer multiplayer; // 멀티플레이 중인지 확인만 함


    public Animator popAnimator;
    public enum ResourceType
    {
        Fuel,      // 연료
        Ore,       // 광석
    }
    GameObject fuelObject;
    GameObject oreObject;
    
    ResourceType resourceType = ResourceType.Fuel;

    private GameObject fuelPrefab; // 프리펩 저장
    private GameObject orePrefab;  // 프리펩 저장

    // enum To array
    //ResourceType[] ResourceTypeArray = (ResourceType[])Enum.GetValues(typeof(ResourceType));

    // 생성주기
    private float spawnWait = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Transform fuelTransform = transform.Find("Resource").Find("FuelBlueprint");
        fuelObject = fuelTransform.gameObject;
        Transform oreTransform = transform.Find("Resource").Find("OreBlueprint");
        oreObject = oreTransform.gameObject;
        oreObject.SetActive(false);

        fuelPrefab = Resources.Load<GameObject>("Resources/Fuel");
        orePrefab = Resources.Load<GameObject>("Resources/Ore");

        popAnimator = GetComponent<Animator>();
        
        try
        {
            multiplayer = FindAnyObjectByType<Multiplayer>();
            if (multiplayer.isMultiplayer) {
                // 3초마다 연속 생성 명령
                StartCoroutine(SpawnResource());
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    // 자원 변경
    public void SwitchResource()
    {
        Debug.Log("자원 변경");
        switch (resourceType)
        {
            case ResourceType.Fuel:
                fuelObject.SetActive(false);
                resourceType = ResourceType.Ore;
                oreObject.SetActive(true);
                break; 
            case ResourceType.Ore:
                oreObject.SetActive(false);
                resourceType = ResourceType.Fuel;
                fuelObject.SetActive(true);
                break;
        }

    }

    // 자원 생성
    private IEnumerator SpawnResource()
    {
        float positionX = gameObject.transform.position.x;     // 현재 오브젝트의 위치를 가져옴
        float positionZ = gameObject.transform.position.z;
        float positionY = gameObject.transform.position.y;
        Vector3 position = new Vector3(positionX, positionY, positionZ - 2); // 앞에 생성

        while (true)
        {
            GameObject currentPrefab;
            switch (resourceType)
            {
                case ResourceType.Fuel:
                    currentPrefab = fuelPrefab;
                    break;
                case ResourceType.Ore:
                    currentPrefab = orePrefab;
                    break;
                default:
                    currentPrefab = null;
                    break;
            }
            Debug.Log("Supplier: " + resourceType + " 생성");
            GameObject newResource = Instantiate(currentPrefab, position, Quaternion.identity);

            // 이름변경
            newResource.name  = resourceType.ToString();
            popAnimator.Play("SupplierPopAnimation");

            // 스폰 코루틴
            yield return new WaitForSeconds(spawnWait);
        }
        
    }
}
