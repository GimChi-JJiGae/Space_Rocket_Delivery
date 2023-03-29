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

    private GameObject fixedObject;

    private void Start()
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

    private void FixedUpdate()
    {
        if (playerInput.RepairModule)
        {

        }
    }

    private void Update()
    {
        if (playerInput.Interact)
        {
            if (matchObject != null && targetObject != null)
            {
                if (targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
                {
                    targetObject.GetComponent<Module>().CreateFloor(ModuleType.ShotgunTurret);    // 바닥생성
                    spaceship.MakeWall(targetObject);
                }
            }
            else if (buildingObject != null)
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