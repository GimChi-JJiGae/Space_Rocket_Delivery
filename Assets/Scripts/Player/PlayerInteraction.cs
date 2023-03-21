using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<string> HoldableObjects = new();

    public Transform leftHand;

    public GameObject currentObject = null;
    public bool isHoldingObject = false;

    private PlayerInput playerInput;
    private Animator playerAnimator;

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

    private void Update()
    {
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
