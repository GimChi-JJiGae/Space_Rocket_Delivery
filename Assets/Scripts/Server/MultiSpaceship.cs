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
        controller.Send(301, xIdx, zIdx, moduleType);
    }

    public void ReceiveCreateModule(int xIdx, int zIdx, int moduleType)
    {
        GameObject targetObject = spaceship.modules[zIdx, xIdx];
        targetObject.GetComponent<Module>().CreateFloor((ModuleType)moduleType); ;
        spaceship.MakeWall(targetObject);
    }
}
