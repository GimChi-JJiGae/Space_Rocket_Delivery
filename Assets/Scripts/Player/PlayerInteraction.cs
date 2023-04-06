using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Module;
using static ResourceChanger;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator playerAnimator;

    private Vector3 playerPosition;

    // Holdable 오브젝트를 달 위치 Object
    private GameObject playerHead;

    private Spaceship spaceship;

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

    // 모듈이 부서지고 Blueprint 위에 올라간 경우
    public GameObject respawnObject;

    // 맞은 모듈 확인
    private Module struckModule;

    public float repairSpeed;
    public float maxHp;

    // skillTree
    public SkillTreeNode skillTree;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        spaceship = FindAnyObjectByType<Spaceship>();

        repairSpeed = 0.5f;

        playerHead = transform.Find("PlayerHead").gameObject;

        skillTree = GetComponent<SkillTreeNode>();

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
            upgradeObject = other.gameObject;
        }
        else if (other.gameObject.CompareTag("Respawn"))
        {
            respawnObject = other.gameObject;
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
            upgradeObject = null;
        }
        else if (other.gameObject.CompareTag("Respawn"))
        {
            respawnObject = null;
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
        Factory factory = FindAnyObjectByType<Factory>();

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

    public void MakeModule(string name)
    {
        if (name == "Laser")
        {
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);
        }
        else if (name == "Shotgun")
        {
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.ShotgunTurret);
        }
        else if (name == "Shield")
        {
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.ShieldTurret);
        }

        spaceship.MakeWall(targetObject);
    }

    public void UpgradeModule()
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
            float health = upgradeObject.transform.GetComponentInChildren<ShieldTurret>().maxShieldHealth;

            if (health == 20f)
            {
                health = 30f;
            }
            else if (health == 30f)
            {
                health = 40f;
            }
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
                    resourceObject.GetComponent<ResourceChanger>().SwitchResource();

                    if (resourceObject.GetComponentInParent<Supplier>() != null)
                    {
                        resourceObject.GetComponentInParent<Supplier>().currentResource = resourceObject.GetComponent<ResourceChanger>().resourceType;
                    }
                }
                else if (produceObject != null)
                {
                    produceObject.GetComponentInParent<Factory>().SwitchModule();
                    produceObject.GetComponentInParent<Factory>().ProduceModule();
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
                            MakeModule(currentObject.name);
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
                            FindAnyObjectByType<Oxygenator>().Increase();
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
                    UpgradeModule();
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

        //if (respawnObject != null)
        //{
        //    transform.GetComponent<PlayerMovement>().Respawn();
        //}
    }
}
