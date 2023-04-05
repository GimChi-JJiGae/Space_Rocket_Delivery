using ResourceNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Module;

public class MultiSpaceship : MonoBehaviour
{
    public enum ActiveNum
    {
        RESOURCE_CHANGE,
        FACTORY_CHANGE,
        FACTORY_PRODUCE,
        INCREASE_OXYGEN,
        RESPAWN,
    }

    Multiplayer multiplayer; // 멀티플레이인지 확인하는 변수

    Spaceship spaceship;
    Controller controller;

    InteractionModule interactionModule;

    public GameObject[] resourceList = new GameObject[10000];
    Vector3[] targetPosition = new Vector3[10000];
    Quaternion[] targetRotation = new Quaternion[10000];
    //bool[] putResource = new bool[10000];
    int resourceCount = 0;

    public delegate void EventResourceSpownHandler(int idxR);           // 리소스 생성 이벤트
    public event EventResourceSpownHandler eventResourceSpown;

    public delegate void EventResourceChangeHandler();                  // 리소스 변경 이벤트
    public event EventResourceChangeHandler eventResourceChange;

    public delegate void EventResourceMoveHandler(DTOresourcemove[] DTOresourcemove);                    // 리소스 무브 이벤트
    public event EventResourceMoveHandler eventResourceMove;

    void Start()
    {
        spaceship = FindAnyObjectByType<Spaceship>();
        GameObject socketObj = GameObject.Find("SocketClient");
        controller = socketObj.GetComponent<Controller>();                   // 컨트롤러 연결하기
        multiplayer = GetComponent<Multiplayer>();

        StartCoroutine(SendCreateResource());
        StartCoroutine(SendPositionResource());

        eventResourceMove += MoveResource;
    }

    void FixedUpdate()
    {
        if (multiplayer.isHost == false)
        {
            for (int i = 0; i < resourceList.Length; i++)
            {
                if (resourceList[i] != null)
                {
                    Vector3 v = (targetPosition[i] - resourceList[i].transform.position) * 5.0f * Time.deltaTime;
                    resourceList[i].transform.position += v;
                    resourceList[i].transform.rotation = Quaternion.Lerp(targetRotation[i], resourceList[i].transform.rotation, 0.1f * Time.deltaTime);
                }
            }
        }
    }

    // 자원 생성 send corutine
    public void CreateModule_SEND(int id, int xIdx, int zIdx, int moduleType)
    {
        controller.Send(PacketType.MODULE_CREATE, id, xIdx, zIdx, moduleType);
    }

    public void CreateModule_RECEIVE(int id, int xIdx, int zIdx, int moduleType)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                GameObject targetObject = spaceship.modules[zIdx, xIdx];
                targetObject.GetComponent<Module>().CreateFloor((ModuleType)moduleType);
                spaceship.MakeWall(targetObject);
            }
        });
    }

    public void ChangeResource_SEND(int id, int moduleType)
    {
        // Packet 번호가 없다.
        controller.Send(PacketType.MODULE_INTERACTION, id, moduleType, (int)ActiveNum.RESOURCE_CHANGE);
    }

    public void ChangeResource_RECEIVE(int id)
    {
        // 인덱스가 없다.
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                GameObject supplier = spaceship.modules[10, 10];

                supplier.GetComponent<ResourceChanger>().SwitchResource();
                supplier.GetComponent<Supplier>().currentResource = supplier.GetComponent<ResourceChanger>().resourceType;
            }
        });
    }

    public void ChangeModule_SEND(int id, int moduleType)
    {
        // Packet 번호가 없다.
        controller.Send(PacketType.MODULE_INTERACTION, id, moduleType, ActiveNum.FACTORY_CHANGE);
    }

    public void ChangeModule_RECEIVE(int id)
    {
        // 변경된 거에 대한 neededOre, neededFuel까지 다 받아서 처리 가능?
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                GameObject factory = spaceship.modules[10, 9];
                factory.GetComponent<Factory>().SwitchModule();
                factory.GetComponent<Factory>().ProduceModule();
            }
        });
    }

    public void ProduceModule_SEND(int id, int moduleType)
    {
        controller.Send(PacketType.MODULE_INTERACTION, id, moduleType, ActiveNum.FACTORY_PRODUCE);
    }

    public void ProduceModule_RECEIVE(int id)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                GameObject factory = spaceship.modules[10, 9];
                factory.GetComponent<Factory>().ProduceModule();
            }
        });
    }

    public void IncreaseOxygen_SEND(int id, int moduleType)
    {
        controller.Send(PacketType.MODULE_INTERACTION, id, moduleType, ActiveNum.INCREASE_OXYGEN);
        
    }

    public void IncreaseOxygen_RECEIVE(int id)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                GameObject oxygenator = spaceship.modules[10, 11];
                oxygenator.GetComponent<Oxygenator>().Increase();
            }
        });
    }

    public void Repair_SEND(int id, int xIdx, int zIdx)
    {
        controller.Send(PacketType.MODULE_REPAIR, id, xIdx, zIdx);
    }

    public void Repair_RECEIVE(int id, int xIdx, int zIdx)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                Module struckModule = spaceship.modules[zIdx, xIdx].GetComponent<Module>();
                float repairAmount = interactionModule.CalculateRepairSpeed();

                struckModule.hp += repairAmount;
            }
        });
    }

    public void Respawn_SEND(int id, int activeNum)
    {
        controller.Send(PacketType.MODULE_INTERACTION, id, activeNum);
    }

    public void Respawn_RECEIVE(int id)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                GameObject.Find("Player" + id).transform.position = new Vector3(0, 0, -2);
            }
        });
    }

    public void ModuleUpgrade_SEND(int id, int x, int z)
    {
        controller.Send(PacketType.MODULE_UPGRADE, id, x, z);
    }

    public void ModuleUpgrade_RECEIVE(int id, int x, int z)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (PlayerPrefs.GetInt("userId") != id)
            {
                Module module = spaceship.modules[z, x].GetComponent<Module>();
                if (module.transform.GetComponentInChildren<ParticleController>())
                {
                    module.transform.GetComponentInChildren<ParticleController>().damage += 1;
                }
                else if (module.transform.GetComponentInChildren<ShotgunBullet>())
                {
                    module.transform.GetComponentInChildren<ShotgunBullet>().damage += 1;
                }
                else if (module.transform.GetComponentInChildren<ShieldTurret>())
                {
                    float health = module.transform.GetComponentInChildren<ShieldTurret>().maxShieldHealth;

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
        });
    }

    IEnumerator SendCreateResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f); // 0.1초마다 반복
                                                    // 반복해서 호출할 함수 호출
            if (multiplayer.isMultiplayer && multiplayer.isHost == true)
            {
                controller.Send(PacketType.RESOURCE_CREATE, resourceCount);
                resourceCount++;
            }
        }
    }

    public void ReceiveCreateResource(int idxR)
    {
        eventResourceSpown?.Invoke(idxR);
    }

    IEnumerator SendPositionResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 반복
                                                   // 반복해서 호출할 함수 호출
            if (multiplayer.isMultiplayer && multiplayer.isHost == true)
            {
                List<object> list = new List<object>();
                int rCount = 0;
                for (int i = 0; i < resourceCount; i++)
                {
                    if (resourceList[i] != null)
                    {
                        rCount++;
                    }
                }
                list.Add(rCount);
                for (int i = 0; i < resourceCount; i++)
                {
                    if (resourceList[i] != null)
                    {
                        list.Add(i);
                        Vector3 a = resourceList[i].transform.position;
                        list.Add(a.x);
                        list.Add(a.y);
                        list.Add(a.z);
                        Quaternion q = resourceList[i].transform.rotation;
                        list.Add(q.x);
                        list.Add(q.y);
                        list.Add(q.z);
                        list.Add(q.w);
                    }
                }
                controller.ListSend(PacketType.RESOURCE_MOVE, list);
            }
        }
    }

    public void ReceiveMoveResource(DTOresourcemove[] DTOresourcemove)
    {
        if (eventResourceMove != null && multiplayer.isHost != true)
        {
            eventResourceMove.Invoke(DTOresourcemove);
        }
    }

    public void MoveResource(DTOresourcemove[] DTOresourcemove)
    {
        for (int i = 0; i < DTOresourcemove.Length; i++)
        {
            Vector3 v = new Vector3(DTOresourcemove[i].px, DTOresourcemove[i].py, DTOresourcemove[i].pz);
            Quaternion q = new Quaternion(DTOresourcemove[i].rx, DTOresourcemove[i].ry, DTOresourcemove[i].rz, DTOresourcemove[i].rw);
            targetPosition[DTOresourcemove[i].idxR] = v;
            targetRotation[DTOresourcemove[i].idxR] = q;
        }
    }
}