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
            float dist = Vector3.Distance(transform.position, ReDirect);//�÷��̾��� ��ġ�� ������ ��ġ ��.

            if (dist < 2f) // �Ÿ�
            {
                  Npcanim.SetBool("BackhoWalk",true);
                  Vector3 nextPos =
                    transform.position + Random.insideUnitSphere * 5f; //���� ���� ����.

                if (NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 10f, NavMesh.AllAreas)) //��� ���������� ����Ǵ� ����
                {
                    nextPos = hit.position *Time.deltaTime * speed;
                }

                ReDirect = nextPos;//ó�� ���������� ���� ������ ���� ����.

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
