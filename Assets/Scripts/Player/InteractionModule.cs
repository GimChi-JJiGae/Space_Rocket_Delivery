using ResourceNamespace;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Module;

public class InteractionModule : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator playerAnimator;
    private InteractionObject interactionObject;

    private Spaceship spaceship;
    private GameObject player;

    // Edge 체크를 위한 오브젝트
    private GameObject matchObject;
    private GameObject targetObject;

    // Resource 변경을 위한 오브젝트
    private GameObject resourceObject;

    public GameObject inputObject;

    private GameObject produceObject;

    public SkillTreeNode skillTree;

    // 맞은 모듈 확인
    private Module struckModule;

    // player 위치
    private Vector3 playerPosition;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        interactionObject = GetComponent<InteractionObject>();

        spaceship = FindAnyObjectByType<Spaceship>();

        player = GameObject.Find("PlayerCharacter");
        playerPosition = player.GetComponent<Transform>().position;

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
    }

    private float CalculateRepairSpeed()
    {
        float baseRepairSpeed = 0.1f;
        float repairSpeedLevel = skillTree.GetRepairSpeedLevel();
        float repairSpeed = baseRepairSpeed + (0.1f * (repairSpeedLevel - 1));
        return repairSpeed;
    }

    private void Update()
    {
        if (playerInput.Interact)
        {
            if (matchObject != null && targetObject != null)
            {
                if (targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
                {
                    targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);    // 바닥생성
                    spaceship.MakeWall(targetObject);
                }
            }
            else if (resourceObject != null)
            {
                resourceObject.GetComponent<ResourceChanger>().SwitchResource();

                if (resourceObject.GetComponentInParent<Supplier>() != null)
                {
                    resourceObject.GetComponentInParent<Supplier>().currentResource = resourceObject.GetComponent<ResourceChanger>().resourceType;
                }
            }
            else if (produceObject != null)
            {
                produceObject.GetComponentInParent<Factory>().SwitchModule();
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
                        }
                    }
                    else if (inputObject.GetComponentInParent<Factory>())
                    {
                        inputObject.GetComponentInParent<Factory>().ProduceModule();
                    }
                }
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
            playerAnimator.SetBool("Repairing", false);
        }
    }
}