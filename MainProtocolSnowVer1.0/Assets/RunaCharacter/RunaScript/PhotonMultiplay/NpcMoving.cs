using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMoving : MonoBehaviour
{
    [Header("Npc Moving")]

    Vector3 ReDirect;
    NavMeshAgent agentNpc;

    private float speed = 2f;

    private Animator Npcanim;

    #region

    private void Start()
    {
        agentNpc = GetComponent<NavMeshAgent>();
        ReDirect = transform.position;
        agentNpc.SetDestination(transform.position);
        Npcanim = GetComponent<Animator>();

        StartCoroutine(MoveNpcPattern());
        StartCoroutine(DistTalk());
    }

    IEnumerator MoveNpcPattern()
    {
        while (true)
        {
            float dist = Vector3.Distance(transform.position, ReDirect);//플레이어의 위치와 목적지 위치 비교.

            if (dist < 2f) // 거리
            {
                  Npcanim.SetBool("BackhoWalk",true);
                  Vector3 nextPos =
                    transform.position + Random.insideUnitSphere * 5f; //랜덤 범위 설정.

                if (NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 10f, NavMesh.AllAreas)) //모든 지역내에서 적용되는 로직
                {
                    nextPos = hit.position *Time.deltaTime * speed;
                }

                ReDirect = nextPos;//처음 목적지에서 다음 목적지 가는 방향.

                yield return new WaitForSeconds(1f);
                agentNpc.SetDestination(nextPos);
            }

            yield return null;
        }
    }

    IEnumerator DistTalk()
    {
        yield return new WaitForSeconds(1f);
    }

    #endregion

}
