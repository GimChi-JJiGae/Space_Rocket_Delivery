using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Module;

public class PlayerModule : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private Spaceship spaceship;
    private GameObject matchObject;
    private GameObject targetObject;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        spaceship = FindAnyObjectByType<Spaceship>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Edge()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEdgeEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        OnEdgeStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnEdgeExit(other);
    }

    // �𼭸��� ���� û������ ������
    private void OnEdgeEnter(Collider other)
    {
        // �𼭸��� �������� ��
        if (other.gameObject.tag == "Edge")
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
            // Ȱ��ȭ�� ��Ų��
            targetObject = spaceship.modules[idxZ, idxX];
            targetObject.GetComponent<Module>().floorModule.SetActive(true);
        }
    }

    // �𼭸� �ȿ� ���� �� �Է��� üũ��
    private void OnEdgeStay(Collider other)
    {
        // �𼭸��� && �Է� && �������Ʈ ����� ��
        if (other.gameObject.tag == "Edge" && playerInput.Interact && targetObject.GetComponent<Module>().moduleType == ModuleType.Blueprint)
        {
            Debug.Log("�����Ǿ�� ��");
            matchObject = other.gameObject;
            // �ٴڻ���
            targetObject.GetComponent<Module>().CreateFloor(ModuleType.LaserTurret);
            // ������ (����Ǿ��ִ� ����� ���� ���ּ����� ����)
            spaceship.MakeWall(targetObject);
        }
    }

    // �𼭸��� ���� �� û������ �Ⱥ�����
    private void OnEdgeExit(Collider other)
    {
        if (other.gameObject.tag == "Edge")
        {
            Module module = targetObject.GetComponentInParent<Module>();
            if (module.moduleType == ModuleType.Blueprint)
            {
                targetObject.GetComponent<Module>().floorModule.SetActive(false);
            }
            matchObject = null;
            targetObject = null;
            
        }
    }
}
