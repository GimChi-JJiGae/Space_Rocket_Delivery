using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // 들 수 있는 모든 obj를 담을 liftableObjects를 리스트로 선언
    public List<string> HoldableObjects = new();
    // character의 왼손, 오른손, 상호작용 할 수 있는 물체와의 최대 거리 선언
    public Transform leftHand;
    public float maxDistance = 1f;

    private GameObject fuelPrefab;
    private GameObject orePrefab;
    //[SerializeField] private GameObject upgradePrefab;
    //[SerializeField] private GameObject laserPrefab;

    // currentObject(현재 물체)를 null로 선언
    public GameObject currentObject = null;
    public bool isHoldingObject = false;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        GameObject dummyPrefab = GameObject.Find("Dummy");

        foreach (Transform prefab in dummyPrefab.transform)
        {
            HoldableObjects.Add(prefab.gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (HoldableObjects.Contains(collision.gameObject.name))
        {
            currentObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (currentObject != null)
        {
            currentObject = null;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isHoldingObject)
        {
            playerAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            playerAnimator.SetLayerWeight(1, 0);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // 상호작용 키를 눌렀을 때,
        if (playerInput.Interact && playerInput.CanInteract())
        {
            if (currentObject != null)
            {
                if (isHoldingObject == false)
                {
                    isHoldingObject = true;

                    currentObject.transform.parent = leftHand;
                    currentObject.transform.SetLocalPositionAndRotation(leftHand.position, leftHand.rotation);
                }
                else
                {
                    isHoldingObject = false;
                    
                    currentObject.transform.parent = null;

                }

                StartCoroutine(playerInput.DisableInteractionForSeconds(1f));
            }
        }
    }
}
