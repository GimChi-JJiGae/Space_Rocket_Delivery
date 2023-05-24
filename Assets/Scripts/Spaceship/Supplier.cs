using System.Collections;
using UnityEngine;

public class Supplier : MonoBehaviour
{
    private MultiSpaceship multiSpaceship;

    private Controller controller;
    private GameObject socketObj; 

    public GameObject fuelObject;
    public GameObject oreObject;

    public enum ResourceType
    {
        Fuel,
        Ore,
    }

    public ResourceType resourceType;

    public Animator popAnimator;

    private GameObject fuelPrefab;
    private GameObject orePrefab;

    public GameObject currentPrefab;

    // 생성주기
    public float respawnTime = 10f;

    //Start is called before the first frame update
    void Start()
    {
        multiSpaceship = GameObject.Find("Server").GetComponent<MultiSpaceship>();

        socketObj = GameObject.Find("SocketClient");
        controller = socketObj.GetComponent<Controller>();

        popAnimator = GetComponent<Animator>();

        fuelObject = transform.Find("Resource").gameObject.transform.Find("FuelBlueprint").gameObject;
        oreObject = transform.Find("Resource").gameObject.transform.Find("OreBlueprint").gameObject;

        oreObject.SetActive(false);

        fuelPrefab = Resources.Load<GameObject>("Resources/Fuel");
        orePrefab = Resources.Load<GameObject>("Resources/Ore");

        currentPrefab = null;

        resourceType = ResourceType.Fuel;

        popAnimator = GetComponent<Animator>();
        StartCoroutine(SpawnResource());
    }
    public void SetRespawnTime(float newRespawnTime)
    {
        respawnTime = newRespawnTime;
    }

    public void SwitchResource(int id)
    {
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

        if (controller.userId == id)
        {
            multiSpaceship.ChangeResource_SEND(id);
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
            switch (resourceType)
            {
                case ResourceType.Fuel:
                    currentPrefab = fuelPrefab;
                    break;
                case ResourceType.Ore:
                    currentPrefab = orePrefab;
                    break;
            }

            Debug.Log("Supplier: " + resourceType + " 생성");

            GameObject newResource = Instantiate(currentPrefab, position, Quaternion.identity);

            // 이름 변경
            newResource.name = resourceType.ToString();
            popAnimator.Play("SupplierPopAnimation");

            yield return new WaitForSeconds(respawnTime);
        }
    }
}
