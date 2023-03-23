using System.Collections.Generic;
using UnityEngine;
using static Module;

public class PlayerInteraction : MonoBehaviour
{
    public List<string> HoldableObjects = new();

    [SerializeField] private GameObject leftHand;


    private bool isHoldingObject = false;

    private PlayerInput playerInput;
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;

    private GameObject currentObject;
    private GameObject matchObject;
    private GameObject targetObject;
    private GameObject buildingObject;
    
    private Spaceship spaceship;



    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        spaceship = FindAnyObjectByType<Spaceship>();


        GameObject dummyPrefab = GameObject.Find("Dummy");

        foreach (Transform prefab in dummyPrefab.transform)
        {
            HoldableObjects.Add(prefab.gameObject.name);
        }

        Destroy(dummyPrefab);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Edge"))
    //    {
    //        matchObject = other.gameObject;
    //        Module module = matchObject.GetComponentInParent<Module>();

    //        int idxZ = module.idxZ;
    //        int idxX = module.idxX;
    //        switch (other.gameObject.name)
    //        {
    //            case "EdgeTop":
    //                idxZ += 1;
    //                break;
    //            case "EdgeBottom":
    //                idxZ -= 1;
    //                break;
    //            case "EdgeRight":
    //                idxX += 1;
    //                break;
    //            case "EdgeLeft":
    //                idxX -= 1;
    //                break;
    //        }

    //        targetObject = spaceship.modules[idxZ, idxX];
    //        targetObject.GetComponent<Module>().floorModule.SetActive(true);
    //    }

    //    else if (other.gameObject.CompareTag("Building"))
    //    {
    //        buildingObject = other.gameObject;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Edge"))
    //    {
    //        if (playerInput.Interact)
    //        {
    //            if (targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
    //            {
    //                targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);    // 바닥생성
    //                spaceship.MakeWall(targetObject);
    //            }
    //        }
    //    }
    //    else if (other.gameObject.CompareTag("Building"))
    //    {
    //        switch (other.gameObject.name)
    //        {
    //            case "Supplier":
    //                Supplier supplier = buildingObject.GetComponent<Supplier>();
    //                supplier.SwitchResource();
    //                break;
    //            case "Engine":
    //                break;
    //            case "Oxygenator":
    //                break;
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Edge"))
    //    {
    //        Module module = targetObject.GetComponentInParent<Module>();

    //        if (module.moduleType == ModuleType.Blueprint)
    //        {
    //            targetObject.GetComponent<Module>().floorModule.SetActive(false);
    //        }

    //        matchObject = null;
    //        targetObject = null;
    //    }
    //    else if (other.gameObject.CompareTag("Building"))
    //    {
    //        buildingObject = null;
    //    }
    //}

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
        obj.transform.parent = leftHand.transform;
        obj.transform.SetPositionAndRotation(leftHand.transform.position, leftHand.transform.rotation);

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
