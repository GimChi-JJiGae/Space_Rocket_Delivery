using UnityEngine;
using static Module;

public class InteractionModule : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private InteractionObjects interactionObjects;

    private Spaceship spaceship;

    // Edge 체크를 위한 오브젝트
    private GameObject matchObject;
    private GameObject targetObject;

    // Building 체크를 위한 오브젝트
    private GameObject buildingObject;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        interactionObjects = GetComponent<InteractionObjects>();

        spaceship = FindAnyObjectByType<Spaceship>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!interactionObjects.isHoldingObject)
        {
            if (other.gameObject.CompareTag("Edge"))
            {
                matchObject = other.gameObject;
                Module module = matchObject.GetComponentInParent<Module>();

                int idxZ = module.idxZ;
                int idxX = module.idxX;
                switch (other.gameObject.name)
                {
                    case "EdgeTop":
                        idxZ += 1;
                        break;
                    case "EdgeBottom":
                        idxZ -= 1;
                        break;
                    case "EdgeRight":
                        idxX += 1;
                        break;
                    case "EdgeLeft":
                        idxX -= 1;
                        break;
                }

                targetObject = spaceship.modules[idxZ, idxX];
                targetObject.GetComponent<Module>().floorModule.SetActive(true);
            }
            else if (other.gameObject.CompareTag("Building"))
            {
                buildingObject = other.gameObject;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!interactionObjects.isHoldingObject)
        {
            if (targetObject != null)
            {
                if (targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
                {
                    Debug.Log(4);
                    if (playerInput.Interact)
                    {
                        Debug.Log(5);
                        targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);    // 바닥생성
                        spaceship.MakeWall(targetObject);
                    }
                }
            }
            else if (buildingObject != null)
            {
                if (playerInput.Interact)
                {
                    switch (buildingObject.name)
                    {
                        case "Supplier":
                            Supplier supplier = buildingObject.GetComponent<Supplier>();
                            supplier.SwitchResource();
                            break;
                        case "Engine":
                            break;
                        case "Oxygenator":
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactionObjects.isHoldingObject)
        {
            if (targetObject != null)
            {
                Module module = targetObject.GetComponentInParent<Module>();

                if (module.moduleType == ModuleType.Blueprint)
                {
                    targetObject.GetComponent<Module>().floorModule.SetActive(false);
                }

                matchObject = null;
                targetObject = null;
            }
            else if (buildingObject != null)
            {
                buildingObject = null;
            }
        }
    }
}