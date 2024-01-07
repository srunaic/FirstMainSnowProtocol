using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    private NavMeshAgent nav;
    public Transform target; //�÷��̾� ��ġ ������ Transform ����
    public GameObject Fire1;

    private float times = 5f;
    private float power = 500f;

    private Rigidbody rigid;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(CheckEnemyBullet());
    }
    void Update()
    {
        nav.SetDestination(target.position); //���� Ÿ�� ����.
    }
    IEnumerator CheckEnemyBullet()
    {
        while (true)
        {
            GameObject ins = Instantiate(Fire1, transform.position, transform.rotation) as GameObject;
            //�Ѿ� ������Ʈ �� �����ϰ� ������ �߻���

            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            //��ġ ��Ȯ��,

            float targetHeight = target.transform.position.y; // Ÿ���� ����
            float bulletHeight = transform.position.y; // �߻� ������ ����
            Vector3 targetPositionAdjusted = new Vector3(target.transform.position.x, bulletHeight, target.transform.position.z);


            ins.GetComponent<Rigidbody>().AddForce(target.transform.forward * power, ForceMode.Impulse);
            //������ ���� Ÿ�� ����

            yield return new WaitForSeconds(times);
        }

    }

}
