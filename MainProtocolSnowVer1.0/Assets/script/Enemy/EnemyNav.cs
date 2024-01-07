using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    private NavMeshAgent nav;
    public Transform target; //플레이어 위치 저장할 Transform 생성
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
        nav.SetDestination(target.position); //다음 타겟 고정.
    }
    IEnumerator CheckEnemyBullet()
    {
        while (true)
        {
            GameObject ins = Instantiate(Fire1, transform.position, transform.rotation) as GameObject;
            //총알 오브젝트 를 생성하고 앞으로 발사함

            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            //위치 정확도,

            float targetHeight = target.transform.position.y; // 타겟의 높이
            float bulletHeight = transform.position.y; // 발사 지점의 높이
            Vector3 targetPositionAdjusted = new Vector3(target.transform.position.x, bulletHeight, target.transform.position.z);


            ins.GetComponent<Rigidbody>().AddForce(target.transform.forward * power, ForceMode.Impulse);
            //유저를 향해 타겟 조준

            yield return new WaitForSeconds(times);
        }

    }

}
