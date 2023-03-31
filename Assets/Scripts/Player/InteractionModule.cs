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
    private GameObject player;

    private Spaceship spaceship;

    // Edge 체크를 위한 오브젝트
    private GameObject matchObject;
    private GameObject targetObject;

    // Resource 변경을 위한 오브젝트
    private GameObject resourceObject;

    public GameObject inputObject;
    public SkillTreeNode skillTree;


    private void OnTriggerExit(Collider other)
    {
        if (!interactionObject.isHoldingObject && targetObject != null)
        {
            Module module = targetObject.GetComponentInParent<Module>();

            if (module.moduleType == ModuleType.Blueprint)
            {
                targetObject.GetComponent<Module>().floorModule.SetActive(false);
            }

            matchObject = null;
            targetObject = null;
        }

        if (resourceObject != null)
        {
            resourceObject = null;
        }

        if (inputObject != null)
        {
            inputObject = null;
        }
    }

    // 맞은 모듈 확인
    private Module struckModule;

    // player 위치
    private Vector3 playerPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (!interactionObject.isHoldingObject)
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
        }

        if (other.gameObject.CompareTag("Change"))
        {
            resourceObject = other.gameObject;
            Debug.Log(1);
        }

        if (other.gameObject.CompareTag("Input"))
        {
            inputObject = other.gameObject;
        }
    }

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
            if (!interactionObject.isHoldingObject)
            {
                if (matchObject != null && targetObject != null)
                {
                    if (targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
                    {
                        targetObject.GetComponent<Module>().CreateFloor(ModuleType.ShieldTurret);    // 바닥생성
                        spaceship.MakeWall(targetObject);
                    }
                }
                else if (resourceObject != null)
                {
                    resourceObject.GetComponent<ResourceChanger>().SwitchResource();
                    Debug.Log(3);

                    if (resourceObject.GetComponentInParent<Supplier>() != null)
                    {
                        resourceObject.GetComponentInParent<Supplier>().currentResource = resourceObject.GetComponent<ResourceChanger>().resourceType;
                    }
                }
            }
            else
            {
                if (inputObject != null)
                {
                    GameObject insertObject = interactionObject.currentObject;
                    if (insertObject.name == "Fuel" && inputObject.GetComponentInParent<Oxygenator>())
                    {
                        inputObject.GetComponentInParent<Oxygenator>().Increase();
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
                struckModule.hp += repairAmount; // 모듈 수리
            }
        }
        else
        {
            //playerAnimator.SetBool("Repairing", false);
        }
    }
}