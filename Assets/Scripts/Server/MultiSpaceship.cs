using ResourceNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Module;

public class MultiSpaceship : MonoBehaviour
{
    public enum ActiveNum
    {
        RESOURCE_CHANGE,
        FACTORY_CHANGE,
        FACTORY_PRODUCE,
        INCREASE_OXYGEN,
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
        controller = GetComponent<Controller>();
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

    // supplier가 바뀌는 것을 전달
    public void SendChangeSupplier(int type)
    {
        controller.Send(PacketType.SUPPLIER_CHANGE, type);
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
        controller.Send(PacketType.OXYGEN_INCREASE, id, moduleType, ActiveNum.INCREASE_OXYGEN);
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

    public void ReceiveChangeSupplier()
    {

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
        for (int i = 0;i < DTOresourcemove.Length;i++)
        {
            Vector3 v = new Vector3(DTOresourcemove[i].px, DTOresourcemove[i].py, DTOresourcemove[i].pz);
            Quaternion q = new Quaternion(DTOresourcemove[i].rx, DTOresourcemove[i].ry, DTOresourcemove[i].rz, DTOresourcemove[i].rw);
            targetPosition[DTOresourcemove[i].idxR] = v;
            targetRotation[DTOresourcemove[i].idxR] = q;
        }
    }
}
