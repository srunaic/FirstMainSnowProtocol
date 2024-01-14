using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using ShootingManager;
public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigidbody2D;

    [Header("드랍될 아이템")]
    public GameObject[] DropItem;
    public float dropRate = 0.3f;//드랍 확률. // 0~100%

    [Header("폭발될 파티클")]
    public ParticleSystem ExEffect;

    [Header("체력관리")]
    public int EnemyMaxHealth = 0;//몹들 라이프 
    [Header("적 움직임")]
    public float MoveSpeed = 0.5f;
    [Header("총쏘기 옵션")]
    public GameObject bulletPrefab;
    public float shotDelay = 1.0f; //발사 주기
    public int shotCount = 3;//연속발사 횟수
    public float shotDist = 0.5f; //연속발사 간격

    public float shotRotate = 90f;//모든 총알을 쏘는 각
    public float startRotate = -45f;//시작 총알 각
    [Header("죽음관리")]
    public int getScore = 10;

    public Vector2 ShotDirection = Vector2.down;

    void Start()
    {
        ExEffect.Stop();
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.gravityScale = 0;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        rigidbody2D.velocity = Vector2.down * MoveSpeed;

        StartCoroutine(ShotAct());
    }

    public void Die()
    {
        //점수 올려줌
        GameManager.Score += getScore;
        Boss.BossCount += 1;
        //카운트를 관리하는 스크립트
        //카운트 횟수가 지나면 보스 등장.

        if (DropItem != null)//아이템 나올 확률
        {
            float rand = Random.Range(0f, 100f);
            Debug.Log("확률공개" + rand);
            if (rand <= dropRate)
            {
                int randomIndex = Random.Range(0, DropItem.Length);
                Instantiate(DropItem[randomIndex], transform.position, Quaternion.identity);
            }
        }

        Instantiate(ExEffect, transform.position, Quaternion.identity);
        ExEffect.Play();
        Destroy(gameObject);//죽는 딜레이
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ShotAct()
    {
        while (true)
        {
            int nowCount = 0;

            //발사당 회전 각 = 전체 회전각 / 발사 횟수 
            float oneRotate = shotRotate / (shotCount-1);
            //shotCount == 3이라면 0,1,2까지 반복
            while (nowCount < shotCount)
            {
                //새 총알 생성(발사)
                GameObject newBullet
                = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                //발사 각도 계산
                Vector3 shotDirNow = ShotDirection;
                float shotAxis = startRotate + oneRotate * nowCount;
                Quaternion ShotRotQuat = Quaternion.Euler(0, 0, shotAxis);

                shotDirNow = ShotRotQuat * shotDirNow;

                //새 총알 세팅
                newBullet.name = bulletPrefab.name;
                newBullet.GetComponent<Bullet>().speed = 10f;
                newBullet.GetComponent<Bullet>().direction = shotDirNow;

                //총알의 회전적용
                newBullet.transform.rotation = Quaternion.Euler(0, 0, startRotate + oneRotate * nowCount);

                if (shotDist > 0)
                    yield return new WaitForSeconds(shotDist);
                //발사 간격 대기

                nowCount++;
            }
            yield return new WaitForSeconds(shotDelay);

       
        }


    }
}
