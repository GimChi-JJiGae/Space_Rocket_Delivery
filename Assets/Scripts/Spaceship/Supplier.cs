using System.Collections;
using UnityEngine;
using static ResourceChanger;

public class Supplier : MonoBehaviour
{
    public Animator popAnimator;

    public GameObject currentPrefab;

    private GameObject fuelPrefab;

    public ResourceType currentResource;
    private GameObject orePrefab;

    // 생성주기
    public float respawnTime = 10f;

    //Start is called before the first frame update
    void Start()
    {
        popAnimator = GetComponent<Animator>();

        fuelPrefab = Resources.Load<GameObject>("Resources/Fuel");
        orePrefab = Resources.Load<GameObject>("Resources/Ore");

        currentPrefab = null;

        popAnimator = GetComponent<Animator>();
        StartCoroutine(SpawnResource());
    }
    public void SetRespawnTime(float newRespawnTime)
    {
        respawnTime = newRespawnTime;
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
}
