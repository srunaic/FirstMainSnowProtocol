using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMov : MonoBehaviour
{
    [Header("Npc������")]
    
    Vector3 ReDestination; // ������
    
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
            float dist = Vector3.Distance(transform.position, ReDestination);//�÷��̾��� ��ġ�� ������ ��ġ ��.
            
            if (dist < 2f) // �Ÿ�
            {
                Vector3 nextPos = 
                    transform.position + Random.insideUnitSphere * 10f; //���� ���� ����.

                if (NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 10f, NavMesh.AllAreas)) //��� ���������� ����Ǵ� ����
                {
                    nextPos = hit.position;
                }

                ReDestination = nextPos;//ó�� ���������� ���� ������ ���� ����.

                yield return new WaitForSeconds(1f);
                agentNpc.SetDestination(nextPos);
            }

            yield return null;
        }
    }
}
