using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // 들 수 있는 모든 obj를 담을 liftableObjects를 리스트로 선언
    public List<GameObject> LiftableObjects = new();
    // character의 왼손, 오른손, 상호작용 할 수 있는 물체와의 최대 거리 선언
    public Transform leftHand;
    public Transform rightHand;
    public float maxDistance = 1f;

    public GameObject LiftableObjectPrefab;

    //public Text LiftPrompText;

    // currentObject(현재 물체)를 null로 선언
    private GameObject currentObject = null;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        //LiftPrompText.gameObject.SetActive(false);
    }

    // 범위 내 가장 가까운, liftable 물체를 찾는 함수.
    private GameObject GetClosestObject()
    {
        GameObject closestObject = null;
        float closestDistance = maxDistance;

        foreach (GameObject obj in LiftableObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= closestDistance)
            {
                closestObject = obj;
                closestDistance = distance;
            }
        }

        //가까운 물체가 없으면 null 반환
        //설정된 distance 내에 liftable한 물체가 있으면 그 물체 반환
        return closestObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Liftable"))
        {
            if (playerInput.Interact)
            {
                currentObject = collision.gameObject;
                playerAnimator.SetBool("Lift", true);

                currentObject.GetComponent<Rigidbody>().useGravity = false;
                currentObject.transform.parent = transform;
                currentObject.transform.SetPositionAndRotation(leftHand.position, leftHand.rotation);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // 상호작용 키를 눌렀을 때,
        if (playerInput.Interact)
        {
            // 들고 있는 currentObject가 없으면
            if (currentObject == null)
            {
                // 가까운 obj를 찾는다.
                currentObject = GetClosestObject();

                // currentObject가 갱신됐으면
                if (currentObject != null)
                {
                    // 물체를 든다. Lift 상태를 true로 바꿔주고
                    playerAnimator.SetBool("Lift", true);


                    currentObject.transform.parent = transform;
                    currentObject.transform.SetLocalPositionAndRotation(leftHand.localPosition, leftHand.localRotation);
                }
            }
            else
            {
                playerAnimator.SetBool("Lift", false);

                currentObject.transform.parent = null;
                currentObject = null;
            }
        }
    }
}
