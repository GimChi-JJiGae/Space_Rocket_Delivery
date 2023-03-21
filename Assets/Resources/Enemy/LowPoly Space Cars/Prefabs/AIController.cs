using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform target; // 캐릭터를 따라가도록 할 대상의 Transform 컴포넌트
    private NavMeshAgent agent; // 네비게이션 시스템을 제어하는 NavMeshAgent 컴포넌트

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
    }

    void Update()
    {
        agent.SetDestination(target.position); // 대상의 위치로 이동 목적지 설정
    }
}