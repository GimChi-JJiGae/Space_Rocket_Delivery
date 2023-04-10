using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Module;
using static Supplier;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator playerAnimator;
    private Vector3 playerPosition;

    private ModuleType currentType;

    private Controller controller;
    private GameObject socketObj;

    // Holdable 오브젝트를 달 위치 Object
    private GameObject playerHead;

    private Spaceship spaceship;

    private Multiplayer multiplayer;
    private MultiSpaceship multiSpaceship;

    public List<string> HoldableObjects = new();

    public bool isHoldingObject = false;

    // 부딪힌 Object
    private GameObject collideObject;

    // 들고 있는 Object
    public GameObject currentObject;

    // 
    public GameObject matchObject;
    public GameObject targetObject;

    // ResourceChanger
    public GameObject resourceObject;

    // Input 가능한 Object
    public GameObject inputObject;

    // Factory 모듈 변경
    public GameObject produceObject;

    // 업그레이드할 모듈
    public GameObject upgradeObject;

    // 맞은 모듈 확인
    private Module struckModule;

    // skillTree
    public SkillTreeNode skillTree;
    public float repairSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        multiSpaceship = GameObject.Find("Server").GetComponent<MultiSpaceship>();

        socketObj = GameObject.Find("SocketClient");
        controller = socketObj.GetComponent<Controller>();

        spaceship = FindAnyObjectByType<Spaceship>();

        playerHead = transform.Find("PlayerHead").gameObject;

        skillTree = GetComponent<SkillTreeNode>();
		repairSpeed = 0.5f;

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

    private void OnTriggerEnter(Collider other)
    {
        if (!isHoldingObject && HoldableObjects.Contains(other.gameObject.name))
        {
            collideObject = other.gameObject;
        }
        else if (other.gameObject.CompareTag("Edge"))
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
            upgradeObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isHoldingObject && HoldableObjects.Contains(other.gameObject.name))
        {
            collideObject = null;
        }
        else if (other.gameObject.CompareTag("Edge") && targetObject != null)
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
            upgradeObject = null;
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
        int playerId = controller.userId;

        if (obj.name == "Fuel")
        {
            multiSpaceship.FactoryInput_SEND(playerId, (int)ResourceType.Fuel);
        }
        else if (obj.name == "Ore")
        {
            multiSpaceship.FactoryInput_SEND(playerId, (int)ResourceType.Ore);
        }

        Destroy(obj);

        isHoldingObject = false;
        }

    public void MakeModule(int id, string name)
    {
        switch (name)
        {
            case "Laser":
                currentType = ModuleType.LaserTurret;
                break;
            case "Shotgun":
                currentType = ModuleType.ShotgunTurret;
                break;
            case "Shield":
                currentType = ModuleType.ShieldTurret;
                break;
        }

        targetObject.GetComponent<Module>().CreateFloor(currentType);
        spaceship.MakeWall(targetObject);

        if (controller.userId == id)
        {
            multiSpaceship.CreateModule_SEND(id, targetObject.GetComponent<Module>().idxX, targetObject.GetComponent<Module>().idxZ, (int)currentType);
        }
    }

    public void UpgradeModule(int id)
    {
        if (upgradeObject.transform.GetComponentInChildren<ParticleController>())
        {
            upgradeObject.transform.GetComponentInChildren<ParticleController>().damage += 1; 
        }
        else if (upgradeObject.transform.GetComponentInChildren<ShotgunBullet>())
        {
            upgradeObject.transform.GetComponentInChildren<ShotgunBullet>().damage += 1;
        }
        else if (upgradeObject.transform.GetComponentInChildren<ShieldTurret>())
        {
            upgradeObject.transform.GetComponentInChildren<ShieldTurret>().maxShieldHealth += 10;
        }

        if (controller.userId == id)
        {
            multiSpaceship.ModuleUpgrade_SEND(id, upgradeObject.GetComponentInParent<Module>().idxX, upgradeObject.GetComponentInParent<Module>().idxZ);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerInput.Interact)
        {
            if (!isHoldingObject)
            {
                if (collideObject != null)
                {
                    PickUpObject(collideObject);
                }
                else if (resourceObject != null)
                {
                    resourceObject.GetComponent<Supplier>().SwitchResource(controller.userId);
                }
                else if (produceObject != null)
                {
                    produceObject.GetComponentInParent<Factory>().SwitchModule(controller.userId);
                }
            }
            else
            {
                if (targetObject != null)
                {
                    if (targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
                    {
                        if (currentObject.name == "Laser" || currentObject.name == "Shotgun" || currentObject.name == "Shield")
                        {
                            MakeModule(controller.userId, currentObject.name);
                            InsertObject(currentObject);
                            collideObject = null;
                            currentObject = null;
                        }
                        else
                        {
                            DropObject(currentObject);
                            collideObject = null;
                            currentObject = null;
                        }
                    }
                }
                else if (inputObject != null)
                {
                    if (inputObject.GetComponentInParent<Oxygenator>())
                    {
                        if (currentObject.name == "Fuel")
                        {
                            FindAnyObjectByType<Oxygenator>().Increase(controller.userId);
                            InsertObject(currentObject);
                            collideObject = null;
                            currentObject = null;
                        }
                        else
                        {
                            DropObject(currentObject);
                            collideObject = null;
                            currentObject = null;
                        }
                    }
                    else if (inputObject.GetComponentInParent<Factory>())
                    {
                        if (currentObject.name == "Fuel" || currentObject.name == "Ore")
                        {
                            SaveObject(currentObject);
                            collideObject = null;
                            currentObject = null;
                        }
                        else
                        {
                            DropObject(currentObject);
                            collideObject = null;
                            currentObject = null;
                        }
                    }
                    else
                    {
                        DropObject(currentObject);
                        collideObject = null;
                        currentObject = null;
                    }
                }
                else if (upgradeObject != null)
                {
                    UpgradeModule(controller.userId);
                    InsertObject(currentObject);
                    collideObject = null;
                    currentObject = null;
                }
                else
                {
                    DropObject(currentObject);
                    collideObject = null;
                    currentObject = null;
                }
            }
        }

        if (playerInput.RepairModule)
        {
            playerPosition = transform.position;

            int playerX = (int)(Math.Round(playerPosition.x / 5) + 10);
            int playerZ = (int)(Math.Round(playerPosition.z / 5) + 10);

            struckModule = spaceship.modules[playerZ, playerX].GetComponent<Module>();

            playerAnimator.SetBool("Repairing", true);

            if (struckModule.hp < struckModule.maxHp)
            {
                Debug.Log("수리중");
                struckModule.hp += repairSpeed * Time.deltaTime; // 기본 수리량 0.1f 에 증가량 더해서 총 수리량 계산
                struckModule.Checked();
            }
        }
        else
        {
            playerAnimator.SetBool("Repairing", false);
        }
    }
}
