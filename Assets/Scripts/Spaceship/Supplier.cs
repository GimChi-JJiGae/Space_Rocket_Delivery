using System;
using System.Collections;
using UnityEngine;
using ResourceNamespace;


public class Supplier : MonoBehaviour
{
    public ResourceChanger resourceChanger;
    public Animator popAnimator;

    readonly ResourceType resourceType;

    // 생성주기
    private float spawnWait = 10.0f;

    //// Start is called before the first frame update
    void Start()
    {
        resourceChanger = GetComponent<ResourceChanger>();

        //    Transform fuelTransform = supplier.transform.Find("Resource").Find("FuelBlueprint");
        //    fuelObject = fuelTransform.gameObject;
        //    Transform oreTransform = transform.Find("Resource").Find("OreBlueprint");
        //    oreObject = oreTransform.gameObject;
        //    oreObject.SetActive(false);

        popAnimator = GetComponent<Animator>();

        // 3초마다 연속 생성 명령
        StartCoroutine(SpawnResource());
    }

    // 자원 생성
    private IEnumerator SpawnResource()
    {
        float positionX = gameObject.transform.position.x;     // 현재 오브젝트의 위치를 가져옴
        float positionZ = gameObject.transform.position.z;
        float positionY = gameObject.transform.position.y;
        Vector3 position = new(positionX, positionY, positionZ - 2); // 앞에 생성

        while (true)
        {
            //switch (resourceType)
            //{
            //    case ResourceType.Fuel:
            //        currentPrefab = fuelPrefab;
            //        break;
            //    case ResourceType.Ore:
            //        currentPrefab = orePrefab;
            //        break;
            //    default:
            //        currentPrefab = null;
            //        break;
            //}

            Debug.Log("Supplier: " + resourceType + " 생성");

            GameObject newResource = Instantiate(resourceChanger.currentPrefab, position, Quaternion.identity);

            // 이름변경
            newResource.name = resourceType.ToString();
            popAnimator.Play("SupplierPopAnimation");

            // 스폰 코루틴
            yield return new WaitForSeconds(spawnWait);
        }
    }
}
