using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMov : MonoBehaviour
{
    [Header("Npc움직임")]
    
    Vector3 ReDestination; // 목적지
    
    NavMeshAgent agentNpc;
    void Start()
    {
        agentNpc = GetComponent<NavMeshAgent>();
        ReDestination = transform.position;
        agentNpc.SetDestination(transform.position);

        StartCoroutine(MoveNpcPattern());
    }

    IEnumerator MoveNpcPattern()
    {
        while (true)
        {
            float dist = Vector3.Distance(transform.position, ReDestination);//플레이어의 위치와 목적지 위치 비교.
            
            if (dist < 2f) // 거리
            {
                Vector3 nextPos = 
                    transform.position + Random.insideUnitSphere * 10f; //랜덤 범위 설정.

                if (NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 10f, NavMesh.AllAreas)) //모든 지역내에서 적용되는 로직
                {
                    nextPos = hit.position;
                }

                ReDestination = nextPos;//처음 목적지에서 다음 목적지 가는 방향.

                yield return new WaitForSeconds(1f);
                agentNpc.SetDestination(nextPos);
            }

            yield return null;
        }
    }
}
