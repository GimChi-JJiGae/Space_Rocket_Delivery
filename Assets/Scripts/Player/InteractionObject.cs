using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public List<string> HoldableObjects = new();

    public bool isHoldingObject = false;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    private GameObject playerHead;
    private GameObject currentObject;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        playerHead = GameObject.Find("PlayerHead");
        try
        {
            GameObject dummyPrefab = GameObject.Find("Dummy");

            foreach (Transform prefab in dummyPrefab.transform)
            {
                HoldableObjects.Add(prefab.gameObject.name);
            }

            Destroy(dummyPrefab);
        }
        catch
        {
            Debug.Log("여긴 필요없어");
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHoldingObject && HoldableObjects.Contains(collision.gameObject.name))
        {
            currentObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isHoldingObject)
        {
            currentObject = null;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isHoldingObject && currentObject != null)
        {
            playerAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            playerAnimator.SetLayerWeight(1, 0);
        }
    }

    private void PickUpObject(GameObject obj)
    {
        obj.transform.parent = playerHead.transform;
        obj.transform.SetPositionAndRotation(playerHead.transform.position, playerHead.transform.rotation);

        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<MeshCollider>().enabled = false;
        isHoldingObject = true;
    }

    private void DropObject(GameObject obj)
    {
        obj.transform.parent = null;

        obj.transform.position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);

        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<MeshCollider>().enabled = true;
        isHoldingObject = false;
    }

    private void Update()
    {
        if (playerInput.Interact)
        {
            if (isHoldingObject && currentObject != null)
            {
                DropObject(currentObject);
                currentObject = null;
            }
            else if (!isHoldingObject && currentObject != null)
            {
                PickUpObject(currentObject);
            }
        }
    }
}
