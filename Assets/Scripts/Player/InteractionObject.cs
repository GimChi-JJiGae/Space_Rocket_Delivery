using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using static Module;

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

        if (obj.name == "Kit")
        {
            Quaternion rotation = playerHead.transform.rotation;
            rotation.eulerAngles = new Vector3(90f, rotation.eulerAngles.y, rotation.eulerAngles.z);

            obj.transform.SetPositionAndRotation(playerHead.transform.position, rotation);
        }
        else
        {
            obj.transform.SetPositionAndRotation(playerHead.transform.position, playerHead.transform.rotation);
        }

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
        Factory factory = interactionModule.inputObject.GetComponentInParent<Factory>();
        if (obj.name == "Fuel")
        {
            factory.destroyFuel++;
        }
        else if (obj.name == "Ore")
        {
            factory.destroyOre++;
        }

        Destroy(obj);

        isHoldingObject = false;

        factory.ProduceModule();
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
                if (interactionModule.targetObject != null)
                {
                    if (currentObject.name == "Laser" || currentObject.name == "Shotgun" || currentObject.name == "Shield")
                    {
                        InsertObject(currentObject);
                        currentObject = null;
                    }

                }
                else if (interactionModule.inputObject != null)
                {
                    if (currentObject.name == "Fuel" && interactionModule.inputObject.GetComponentInParent<Oxygenator>())
                    {
                        InsertObject(currentObject);
                        currentObject = null;
                    }
                    else if (interactionModule.inputObject.GetComponentInParent<Factory>())
                    {
                        if (currentObject.name == "Fuel" || currentObject.name == "Ore")
                        {
                            SaveObject(currentObject);
                            currentObject = null;
                        }
                        else
                        {
                            DropObject(currentObject);
                            currentObject = null;
                        }
                    }
                    else
                    {
                        DropObject(currentObject);
                        currentObject = null;
                    }
                }
                else
                {
                    DropObject(currentObject);
                    currentObject = null;
                }
            }
        }
    }
}
