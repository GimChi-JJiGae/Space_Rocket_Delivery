using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // �� �� �ִ� ��� obj�� ���� liftableObjects�� ����Ʈ�� ����
    public List<GameObject> LiftableObjects = new();
    // character�� �޼�, ������, ��ȣ�ۿ� �� �� �ִ� ��ü���� �ִ� �Ÿ� ����
    public Transform leftHand;
    public Transform rightHand;
    public float maxDistance = 1f;

    public GameObject LiftableObjectPrefab;

    public Text LiftPrompText;

    private bool isNearLiftableObject = false;

    // currentObject(���� ��ü)�� null�� ����
    private GameObject currentObject = null;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        LiftPrompText.gameObject.SetActive(false);
    }

    //private IEnumerator Delay(float delay)
    //{
    //    isDelaying = true;

    //    yield return new WaitForSeconds(delay);

    //    playerAnimator.SetBool("Lift", false);

    //    isDelaying = false;
    //}

    // ���� �� ���� �����, liftable ��ü�� ã�� �Լ�.
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

        //����� ��ü�� ������ null ��ȯ
        //������ distance ���� liftable�� ��ü�� ������ �� ��ü ��ȯ
        return closestObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LiftableObjects.Contains(other.gameObject))
        {
            isNearLiftableObject = true;
            LiftPrompText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LiftableObjects.Contains(other.gameObject))
        {
            isNearLiftableObject = false;
            LiftPrompText.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // ��ȣ�ۿ� Ű�� ������ ��,
        if (isNearLiftableObject && playerInput.Interact)
        {
            // ��� �ִ� currentObject�� ������
            if (currentObject == null)
            {
                // ����� obj�� ã�´�.
                currentObject = GetClosestObject();

                // currentObject�� ���ŵ�����
                if (currentObject != null)
                {
                    // ��ü�� ���. Lift ���¸� true�� �ٲ��ְ�
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
