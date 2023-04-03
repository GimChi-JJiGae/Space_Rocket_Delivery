using ResourceNamespace;
//using ResourceNamespace;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Module;

public class InteractionModule : MonoBehaviour
{
    // 멀티를 위한 오브젝트
    public Multiplayer multiplayer;
    public MultiSpaceship multiSpaceship;

    private PlayerInput playerInput;
    private Animator playerAnimator;
    private InteractionObject interactionObject;

    //private ResourceChanger resourceChanger;
    private Spaceship spaceship;
    private GameObject player;

    // Resource 변경을 위한 오브젝트
    public GameObject resourceObject;

    // Edge 체크를 위한 오브젝트
    public GameObject matchObject;
    public GameObject targetObject;

    public GameObject inputObject;

    // 맞은 모듈 확인
    private Module struckModule;

    public GameObject turretObject;

    public SkillTreeNode skillTree;

    public GameObject produceObject;

    // player 위치
    private Vector3 playerPosition;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        interactionObject = GetComponent<InteractionObject>();

        spaceship = FindAnyObjectByType<Spaceship>();

        multiSpaceship = GameObject.Find("Server").GetComponent<MultiSpaceship>();
        multiplayer = GameObject.Find("Server").GetComponent<Multiplayer>();
        /*
        player = GameObject.Find("PlayerCharacter");
        playerPosition = player.GetComponent<Transform>().position;
        */

        skillTree = GetComponent<SkillTreeNode>();
    }

    private void OnTriggerEnter(Collider other)
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
        else if (other.gameObject.CompareTag("Input"))
        {
            inputObject = other.gameObject;
        }
        else if (other.gameObject.CompareTag("Change"))
        {
            resourceObject = other.gameObject;
        }
        else if (other.gameObject.CompareTag("Produce"))
        {
            produceObject = other.gameObject;
        }
        else if (other.gameObject.CompareTag("Turret"))
        {
            turretObject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Edge") && targetObject != null)
        {
            Module module = targetObject.GetComponentInParent<Module>();

            if (module.moduleType == ModuleType.Blueprint)
            {
                targetObject.GetComponent<Module>().floorModule.SetActive(false);
            }

            matchObject = null;
            targetObject = null;
        }
        else if (other.gameObject.CompareTag("Change"))
        {
            resourceObject = null;
        }
        else if (other.gameObject.CompareTag("Input"))
        {
            inputObject = null;
        }
        else if (other.gameObject.CompareTag("Produce"))
        {
            produceObject = null;
        }
        else if (other.gameObject.CompareTag("Turret"))
        {
            turretObject = null;
        }
    }
    private float CalculateRepairSpeed()
    {
        float baseRepairSpeed = 0.1f;
        float repairSpeedLevel = skillTree.GetRepairSpeedLevel();
        float repairSpeed = baseRepairSpeed + (0.1f * (repairSpeedLevel - 1));
        return repairSpeed;
    }

    public void UpgradeModule()
    {

    }

    public void MakeModule()
    {
        if (interactionObject.currentObject.name == "Laser")
        {
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);
        }
        else if (interactionObject.currentObject.name == "Shotgun")
        {
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.ShotgunTurret);
        }
        else if (interactionObject.currentObject.name == "Shield")
        {
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.ShieldTurret);
        }
        spaceship.MakeWall(targetObject);
    }

    private void Update()
    {
        if (playerInput.Interact)
        {
            if (matchObject != null && targetObject != null)
            {
                if (targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint && interactionObject.isHoldingObject) //
                {
                    if (multiplayer.isMultiplayer == true)
                    {
                        int tIdxX = targetObject.GetComponent<Module>().idxX;
                        int tIdxZ = targetObject.GetComponent<Module>().idxZ;
                        multiSpaceship.SendCreateModule(tIdxX, tIdxZ, (int)ModuleType.DefaultTurret);
                    }
                    else
                    {
                        if (interactionObject.currentObject.name == "Laser" || interactionObject.currentObject.name == "Shotgun" || interactionObject.currentObject.name == "Shield")
                        {
                            MakeModule();
                        }
                    }
                    /*
                    
                    */
                }
            }
            // Supplier 자원 변경
            else if (resourceObject != null)
            {
                if (multiplayer.isMultiplayer == true)
                {
                    ResourceType resourceType = resourceObject.GetComponent<ResourceChanger>().resourceType;
                    switch (resourceType)
                    {
                        case ResourceType.Fuel:
                            resourceType = ResourceType.Ore;
                            break;
                        case ResourceType.Ore:
                            resourceType = ResourceType.Fuel;
                            break;
                    }
                    multiSpaceship.SendChangeSupplier((int)resourceType);
                }
                else
                {
                    resourceObject.GetComponent<ResourceChanger>().SwitchResource();

                    if (resourceObject.GetComponentInParent<Supplier>() != null)
                    {
                        resourceObject.GetComponentInParent<Supplier>().currentResource = resourceObject.GetComponent<ResourceChanger>().resourceType;
                    }
                }
            }
            else if (produceObject != null)
            {
                produceObject.GetComponentInParent<Factory>().SwitchModule();
                produceObject.GetComponentInParent<Factory>().ProduceModule();
            }

            if (interactionObject.isHoldingObject)
            {
                if (inputObject != null)
                {
                    if (inputObject.GetComponentInParent<Oxygenator>())
                    {
                        GameObject insertObject = interactionObject.currentObject;
                        if (insertObject.name == "Fuel")
                        {
                            inputObject.GetComponentInParent<Oxygenator>().Increase();
                            Debug.Log(multiplayer.isMultiplayer);
                            if (multiplayer.isMultiplayer == true)
                            {
                                Module module = targetObject.GetComponent<Module>();
                                multiSpaceship.SendCreateModule(module.idxX, module.idxZ, (int)ModuleType.LaserTurret);    // 바닥생성
                            }
                            else
                            {
                                targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);
                                spaceship.MakeWall(targetObject);
                            }
                        }
                        else if (inputObject.GetComponentInParent<Factory>())
                        {
                            inputObject.GetComponentInParent<Factory>().ProduceModule();
                        }
                    }
                    else if (turretObject != null)
                    {
                        UpgradeModule();
                    }
                }

                if (playerInput.RepairModule)
                {
                    playerPosition = player.GetComponent<Transform>().position;

                    int playerX = (int)(Math.Round(playerPosition.x / 5) + 10);
                    int playerZ = (int)(Math.Round(playerPosition.z / 5) + 10);

                    struckModule = spaceship.modules[playerZ, playerX].GetComponent<Module>();

                    playerAnimator.SetBool("Repairing", true);

                    float repairAmount = CalculateRepairSpeed(); // 기본 수리량 0.1f 에 증가량 더해서 총 수리량 계산

                    if (struckModule.hp < 3)
                    {
                        struckModule.hp += repairAmount;
                    }
                }
                else
                {
                }
            }
        }
    }
}