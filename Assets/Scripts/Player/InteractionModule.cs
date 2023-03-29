using System;
using UnityEngine;
using static Module;

public class InteractionModule : MonoBehaviour
{
    private PlayerInput playerInput;

    private Spaceship spaceship;
    private GameObject player;

    // Edge 체크를 위한 오브젝트
    private GameObject matchObject;
    private GameObject targetObject;
    private bool isRepairing;

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
            else if (other.gameObject.CompareTag("Building"))
            {
                buildingObject = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!interactionObject.isHoldingObject)
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
    private Animator playerAnimator;
    private InteractionObject interactionObject;

    // 맞은 모듈 확인
    private Module struckModule;

    // player 위치
    private Vector3 playerPosition;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        interactionObject = GetComponent<InteractionObject>();

        isRepairing = playerAnimator.GetBool("Repairing");

        spaceship = FindAnyObjectByType<Spaceship>();

        player = GameObject.Find("PlayerCharacter");
        playerPosition = player.GetComponent<Transform>().position;
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

        if (playerInput.RepairModule)
        {
            playerPosition = player.GetComponent<Transform>().position;

            int playerX = (int)(Math.Round(playerPosition.x / 5) + 10);
            int playerZ = (int)(Math.Round(playerPosition.z / 5) + 10);

            struckModule = spaceship.modules[playerZ, playerX].GetComponent<Module>();

            playerAnimator.SetBool("Repairing", true);

            if (struckModule.hp < 3)
            {
                struckModule.hp += 0.1f;
            }
        }
        else
        {
            playerAnimator.SetBool("Repairing", false);
        }
    }
}