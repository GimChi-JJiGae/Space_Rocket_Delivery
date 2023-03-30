using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public List<string> HoldableObjects = new();

    public bool isHoldingObject = false;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    private InteractionModule interactionModule;

    private GameObject playerHead;

    private GameObject collideObject;
    public GameObject currentObject;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        interactionModule = GetComponent<InteractionModule>();

        playerHead = GameObject.Find("PlayerHead");

        GameObject dummyPrefab = GameObject.Find("Dummy");

        foreach (Transform prefab in dummyPrefab.transform)
        {
            HoldableObjects.Add(prefab.gameObject.name);
        }

        Destroy(dummyPrefab);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHoldingObject && HoldableObjects.Contains(collision.gameObject.name))
        {
            collideObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collideObject != null)
        {
            collideObject = null;
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

        currentObject = obj;
        isHoldingObject = true;
    }

    private void DropObject(GameObject obj)
    {
        obj.transform.parent = null;

        obj.transform.position = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);

        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<MeshCollider>().enabled = true;

        currentObject = null;
        isHoldingObject = false;
    }

    private void InsertObject(GameObject obj)
    {
        obj.transform.parent = null;
        Destroy(obj);

        isHoldingObject = false;
    }

    private void SaveObject(GameObject obj)
    {
        Transform saveObject = interactionModule.inputObject.GetComponentInParent<Factory>().transform.Find("Input");

        obj.transform.parent = saveObject;

        obj.SetActive(false);
        currentObject = null;
        isHoldingObject = false;
    }

    private void Update()
    {
        if (playerInput.Interact)
        {
            if (!isHoldingObject && collideObject != null)
            {
                PickUpObject(collideObject);
            }
            else if (isHoldingObject)
            {
                if (interactionModule.inputObject != null)
                {
                    if (currentObject.name == "Fuel" && interactionModule.inputObject.GetComponentInParent<Oxygenator>())
                    {
                        InsertObject(currentObject);
                        currentObject = null;
                    }
                    else if (interactionModule.inputObject.GetComponentInParent<Factory>())
                    {
                        SaveObject(currentObject);
                    }
                    else
                    {
                        DropObject(currentObject);
                    }
                }
                else
                {
                    DropObject(currentObject);
                }
            }
        }
    }
}
