using System;
using System.Collections;
using UnityEngine;
using ResourceNamespace;

public class Supplier : MonoBehaviour
{
    public Animator popAnimator;

    private GameObject fuelPrefab;
    private GameObject orePrefab;

    public GameObject currentPrefab;

    public ResourceType currentResource;

    // 생성주기
    readonly private float respawnTime = 10f;

    //Start is called before the first frame update
    void Start()
    {
        popAnimator = GetComponent<Animator>();

        fuelPrefab = Resources.Load<GameObject>("Resources/Fuel");
        orePrefab = Resources.Load<GameObject>("Resources/Ore");

        currentPrefab = null;

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
            switch (currentResource)
            {
                case ResourceType.Fuel:
                    currentPrefab = fuelPrefab;
                    break;
                case ResourceType.Ore:
                    currentPrefab = orePrefab;
                    break;
            }

            Debug.Log("Supplier: " + currentResource + " 생성");

            GameObject newResource = Instantiate(currentPrefab, position, Quaternion.identity);

            // 이름 변경
            newResource.name = currentResource.ToString();
            popAnimator.Play("SupplierPopAnimation");

            yield return new WaitForSeconds(respawnTime);
        }


        //while (true)
        //{
        //    switch (resourceType)
        //    {
        //        case ResourceType.Fuel:
        //            prefabState = fuelPrefab;
        //            break;
        //        case ResourceType.Ore:
        //            prefabState = orePrefab;
        //            break;
        //    }

        //    Debug.Log("Supplier: " + resourceType + " 생성");

        //    GameObject newResource = Instantiate(resourceChanger.currentPrefab, position, Quaternion.identity);

        //    // 이름변경
        //    newResource.name = resourceType.ToString();

        //    popAnimator.Play("SupplierPopAnimation");

        //    // 스폰 코루틴
        //    yield return new WaitForSeconds(spawnWait);
        //}
    }
}
