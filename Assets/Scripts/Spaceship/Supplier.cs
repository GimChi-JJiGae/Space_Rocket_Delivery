using System.Collections;
using ResourceNamespace;
using UnityEngine;

public class Supplier : MonoBehaviour
{
    Multiplayer multiplayer; // 멀티플레이 중인지 확인만 함
    MultiSpaceship multiSpaceship; // 자원을 여기다가 저장함
    int resourceCount = 0;

    public Animator popAnimator;

    public GameObject currentPrefab;

    private GameObject fuelPrefab;

    public ResourceType currentResource;
    private GameObject orePrefab;

    // 생성주기
    readonly private float respawnTime = 10f;

    //Start is called before the first frame update
    void Start()
    {
        popAnimator = GetComponent<Animator>();

        fuelPrefab = Resources.Load<GameObject>("Resources/Fuel");
        orePrefab = Resources.Load<GameObject>("Resources/Ore");

        currentPrefab = null;

        popAnimator = GetComponent<Animator>();
        try
        {
            multiplayer = FindAnyObjectByType<Multiplayer>();
            multiSpaceship = FindAnyObjectByType<MultiSpaceship>();
            multiSpaceship.eventResourceSpown += MultiSpawnResource;
            
            if (!multiplayer.isMultiplayer) 
            {
                Debug.Log("멀티가 아니네요");
                StartCoroutine(SpawnResource());
            }
        }
        catch
        {

        }
    }

    // 자원 생성
    private IEnumerator SpawnResource()
    {
        float positionX = transform.position.x;     // 현재 오브젝트의 위치를 가져옴
        float positionZ = transform.position.z;
        float positionY = transform.position.y;

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
    }

    // 자원 생성
    public void MultiSpawnResource(int idxR)
    {
        float positionX = transform.position.x;     // 현재 오브젝트의 위치를 가져옴
        float positionZ = transform.position.z;
        float positionY = transform.position.y;
        Vector3 position = new(positionX, positionY, positionZ - 2); // 앞에 생성

        GameObject currentPrefab = currentResource switch
        {
            ResourceType.Fuel => fuelPrefab,
            ResourceType.Ore => orePrefab,
            _ => null,
        };
        Debug.Log("Supplier: " + currentResource + " 생성");
        GameObject newResource = Instantiate(currentPrefab, position, Quaternion.identity);

        // 이름변경
        newResource.name = currentResource.ToString();
        popAnimator.Play("SupplierPopAnimation");

        multiSpaceship.resourceList[idxR] = newResource;
        resourceCount++;
    }
}
