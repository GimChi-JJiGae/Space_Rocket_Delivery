using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Module;

public class MultiSpaceship : MonoBehaviour
{
    Spaceship spaceship;
    Controller controller;
    InteractionModule interactionModule;
    
    void Start()
    {
        spaceship = GameObject.Find("Spaceship").GetComponent<Spaceship>();
        controller = GetComponent<Controller>();
    }

    void Update()
    {
        
    }

    public void SendCreateModule(int xIdx, int zIdx, int moduleType)
    {
        controller.Send(PacketType.MODULE_CONTROL, xIdx, zIdx, moduleType);
    }

    public void ReceiveCreateModule(int xIdx, int zIdx, int moduleType)
    {
        GameObject targetObject = spaceship.modules[zIdx, xIdx];
        targetObject.GetComponent<Module>().CreateFloor((ModuleType)moduleType); ;
        spaceship.MakeWall(targetObject);
    }

    IEnumerator SendCreateResource(int xIdx, int zIdx, int moduleType)
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f); // 0.1초마다 반복
                                                    // 반복해서 호출할 함수 호출
            controller.Send(PacketType.MODULE_CONTROL, xIdx, zIdx, moduleType);
        }
    }
}
