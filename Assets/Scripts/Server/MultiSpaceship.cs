using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Module;
using static Supplier;

public class MultiSpaceship : MonoBehaviour
{
    public enum ActiveNum
    {
        RESOURCE_CHANGE,
        FACTORY_CHANGE,
        INCREASE_OXYGEN,
        RESPAWN,
    }

    private Supplier supplier;
    private Oxygenator oxygenator;

    private Multiplayer multiplayer; // 멀티플레이인지 확인하는 변수

    private GameObject socketObj;
    private Spaceship spaceship;
    private Controller controller;

    private GameObject player;

    private ResourceType resourceType;

    private PlayerInteraction playerInteraction;


    public GameObject[] resourceList = new GameObject[10000]; 
    public Vector3[] targetPosition = new Vector3[10000];
    public Quaternion[] targetRotation = new Quaternion[10000];

    public delegate void EventResourceSpownHandler(int idxR);           // 리소스 생성 이벤트
    public event EventResourceSpownHandler eventResourceSpown;

    public delegate void EventResourceChangeHandler();                  // 리소스 변경 이벤트
    public event EventResourceChangeHandler eventResourceChange;

    public delegate void EventResourceMoveHandler(DTOresourcemove[] DTOresourcemove);                    // 리소스 무브 이벤트
    public event EventResourceMoveHandler eventResourceMove;

    void Start()
    {
        controller = FindAnyObjectByType<Controller>();                   // 컨트롤러 연결하기

        player = GameObject.Find("Player" + controller.userId);
        playerInteraction = player.transform.GetComponentInChildren<PlayerInteraction>();

        spaceship = FindAnyObjectByType<Spaceship>();

        multiplayer = GetComponent<Multiplayer>();

        //StartCoroutine(SendCreateResource());
        //StartCoroutine(SendPositionResource());

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
                    Vector3 v = 5.0f * Time.deltaTime * (targetPosition[i] - resourceList[i].transform.position);
                    resourceList[i].transform.position += v;
                    resourceList[i].transform.rotation = Quaternion.Lerp(targetRotation[i], resourceList[i].transform.rotation, 0.1f * Time.deltaTime);
                }
            }
        }
    }

    // 모듈 생성 send Coroutine
    public void CreateModule_SEND(int id, int xIdx, int zIdx, int moduleType)
    {
        controller.Send(PacketType.MODULE_CREATE, controller.roomCode ,id, xIdx, zIdx, moduleType);
    }

    public void CreateModule_RECEIVE(int id, int xIdx, int zIdx, int moduleType)
    {
        if (controller.userId != id)
        {
            GameObject targetObject = spaceship.modules[zIdx, xIdx];
            targetObject.GetComponent<Module>().CreateFloor((ModuleType)moduleType);
            spaceship.MakeWall(targetObject);
        }
    }
    
    public void ChangeResource_SEND(int id)
    {
        // Packet 번호가 없다.
        controller.Send(PacketType.MODULE_INTERACTION, controller.roomCode, id, (int)ActiveNum.RESOURCE_CHANGE);
    }

    public void ChangeResource_RECEIVE(int id)
    {
        if (controller.userId != id)
        {
            FindAnyObjectByType<Supplier>().SwitchResource(id);
        }
    }

    public void ChangeModule_SEND(int id)
    {
        controller.Send(PacketType.MODULE_INTERACTION, controller.roomCode, id, (int)ActiveNum.FACTORY_CHANGE);
    }

    public void ChangeModule_RECEIVE(int id)
    {
        if (controller.userId != id)
        {
            FindAnyObjectByType<Factory>().SwitchModule(id);
        }
        
    }

    public void IncreaseOxygen_SEND(int id)
    {
        controller.Send(PacketType.MODULE_INTERACTION, controller.roomCode, id, (int)ActiveNum.INCREASE_OXYGEN);
        
    }

    public void IncreaseOxygen_RECEIVE(int id)
    {
        if (controller.userId != id)
        {
            FindAnyObjectByType<Oxygenator>().Increase(id);
        }
    }

    public void Repair_SEND(int id, int xIdx, int zIdx)
    {
        controller.Send(PacketType.MODULE_REPAIR, controller.roomCode, id, xIdx, zIdx);
    }

    public void Repair_RECEIVE(int id, int xIdx, int zIdx)
    {
        float repairSpeed = 0.5f;

        if (controller.userId != id)
        {
            Module struckModule = spaceship.modules[zIdx, xIdx].GetComponent<Module>();
            if (struckModule.hp < struckModule.maxHp)
            {
                struckModule.hp += repairSpeed * Time.deltaTime; // 기본 수리량 0.1f 에 증가량 더해서 총 수리량 계산
                struckModule.Checked();
            }
        }
    }

    public void Respawn_SEND(int id, int activeNum)
    {
        controller.Send(PacketType.MODULE_INTERACTION, controller.roomCode, id, activeNum);
    }

    public void Respawn_RECEIVE(int id)
    {
        if (controller.userId != id)
        {
            GameObject.Find("Player" + id).transform.position = new Vector3(0, 0, -2);
        }
    }

    public void FactoryInput_SEND(int id, int resourceType)
    {
        controller.Send(PacketType.FACTORY_INPUT, controller.roomCode, id, resourceType);
    }

    public void FactoryInput_RECEIVE(int ore, int fuel, bool isMade, int type)
    {
        Factory factory = FindAnyObjectByType<Factory>();

        if (isMade)
        {
            factory.ProduceModule(type);
            factory.destroyOre = ore;
            factory.destroyFuel = fuel;
        }
        else
        {
            factory.destroyOre = ore;
            factory.destroyFuel = fuel;
        }
    }

    public void ModuleUpgrade_SEND(int id, int x, int z)
    {
        controller.Send(PacketType.MODULE_UPGRADE, controller.roomCode, id, x, z);
    }

    public void ModuleUpgrade_RECEIVE(int id, int x, int z)
    {
        if (controller.userId != id)
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
                module.transform.GetComponentInChildren<ShieldTurret>().maxShieldHealth += 10;
            }
        }
    }
    // 소켓이 막혀서 일단 정지
    IEnumerator SendCreateResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f); // 0.1초마다 반복
                                                    // 반복해서 호출할 함수 호출
            if (multiplayer.isMultiplayer && multiplayer.isHost == true)
            {
                //controller.Send(PacketType.RESOURCE_CREATE, resourceCount);
                //resourceCount++;
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
                List<object> list = new();
                //int rCount = 0;
                //for (int i = 0; i < resourceCount; i++)
                //{
                //    if (resourceList[i] != null)
                //    {
                //        rCount++;
                //    }
                //}
                //list.Add(rCount);
                //for (int i = 0; i < resourceCount; i++)
                //{
                //    if (resourceList[i] != null)
                //    {
                //        list.Add(i);
                //        Vector3 a = resourceList[i].transform.position;
                //        list.Add(a.x);
                //        list.Add(a.y);
                //        list.Add(a.z);
                //        Quaternion q = resourceList[i].transform.rotation;
                //        list.Add(q.x);
                //        list.Add(q.y);
                //        list.Add(q.z);
                //        list.Add(q.w);
                //    }
                //}
                //controller.ListSend(PacketType.RESOURCE_MOVE, list);
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
            Vector3 v = new(DTOresourcemove[i].px, DTOresourcemove[i].py, DTOresourcemove[i].pz);
            Quaternion q = new(DTOresourcemove[i].rx, DTOresourcemove[i].ry, DTOresourcemove[i].rz, DTOresourcemove[i].rw);
            targetPosition[DTOresourcemove[i].idxR] = v;
            targetRotation[DTOresourcemove[i].idxR] = q;
        }
    }
}