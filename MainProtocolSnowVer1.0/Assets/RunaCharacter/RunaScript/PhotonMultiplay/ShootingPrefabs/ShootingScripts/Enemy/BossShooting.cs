using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ShootingManager;
public class BossShooting : MonoBehaviour
{ 
    Rigidbody2D rigidbody2D;

    [Header("드랍될 아이템")]
    public GameObject DropItem;
    public float dropRate = 1.0f;//드랍 확률. // 0~100%

    [Header("폭발될 파티클")]
    public ParticleSystem ExEffect;

    [Header("체력관리")]
    public int BossMaxHealth;//몹들 라이프 5
    public int currentHealth;

    public HealthBar healthBar;

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

    [Header("적한테 맞는 로직")]
    private SpriteRenderer spriteRenderer;

    public Transform firePoint;

    [Header("방사형 미사일")]
    public GameObject radialBulletPrefab;
    public float radialShotDelay = 3.0f; // 방사형 미사일 발사 주기
    public int radialBulletCount = 8; // 방사형 미사일 개수
    public float radialBulletSpeed = 8.0f; // 방사형 미사일 속도
    public float speed;

    [Header("도넛 미사일")]
    public GameObject donutBulletPrefab;
    public float donutBulletCount = 12;
    public float donutRadius = 2.0f;
    public float donutShotDelay = 1.0f;
    public float donutBulletSpeed;

    void Start()
    {
        BossMaxHealth = 100;
        currentHealth = BossMaxHealth; //현재 hp = 최대치
        healthBar.setMaxHealth(BossMaxHealth);

        spriteRenderer = GetComponent<SpriteRenderer>();

        ExEffect.Stop();
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.gravityScale = 0;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        rigidbody2D.velocity = Vector2.down * MoveSpeed;

        StartCoroutine(ShotAct());
        StartCoroutine(RadialShot());
        StartCoroutine(DonutShot());
    }
    //보스가 받는 데미지.
    public void BossTakeDamage(int PlayerdamageAmount)
    {
        currentHealth -= PlayerdamageAmount;//플레이어 한테 받는 데미지.
        healthBar.setHealth(currentHealth); // Update the health bar when taking damage.

        // 데미지를 받으면 빨갛게 변하도록 색상 변경
        spriteRenderer.color = Color.red;
        //무적상태 설정 함수.
        SetInvincible();
        // 데미지를 받은 후 체크하여 다시 원래 색상으로 되돌리기
        Invoke("ResetColor", 0.2f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void FixedUpdate()
    {
        ResetColor();
    }

    private void SetInvincible()
    {
        Invoke("ResetColor", 2.0f);
    }
    private void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        //점수 올려줌
        GameManager.Score += getScore;
        Boss.BossCount += 1;
        //카운트를 관리하는 스크립트
        //카운트 횟수가 지나면 보스 등장.

        CancelInvoke(); // 모든 Invoke 호출 취소

        if (DropItem != null)//아이템 나올 확률
        {
            float rand = Random.Range(0f, 100f);
            Debug.Log("확률공개" + rand);

            if (rand <= dropRate)
                Instantiate(DropItem, transform.position, Quaternion.identity);
        }

        Instantiate(ExEffect, transform.position, Quaternion.identity);
        ExEffect.Play();
        //SceneManager.LoadScene("Lobby");
        Destroy(gameObject);//죽는 딜레이
    }

    private void OnCollisionEnter2D(Collision2D col)
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
            float oneRotate = shotRotate / (shotCount - 1);
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
    IEnumerator RadialShot()
    {
        while (true)
        {
            for (int i = 0; i < radialBulletCount; i++)
            {
                // 각도 계산
                float angle = i * (360f / radialBulletCount);

                // 방사형 미사일 생성 위치 계산
                Vector3 bulletPosition = firePoint.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);

                // 방사형 미사일 생성
                GameObject newRadialBullet = Instantiate(radialBulletPrefab, bulletPosition, Quaternion.Euler(0, 0, angle));

                // 방사형 미사일 설정
                newRadialBullet.name = radialBulletPrefab.name;
              
                newRadialBullet.GetComponent<Bullet>().speed = radialBulletSpeed;
            }

            yield return new WaitForSeconds(radialShotDelay);
        }
    }
    IEnumerator DonutShot()
    {
        while (true)
        {
            for (int i = 0; i < donutBulletCount; i++)
            {

                float angle = i * (5f * Mathf.PI / donutBulletCount);

                float x = Mathf.Cos(angle) * donutRadius;
                float y = Mathf.Sin(angle) * donutRadius;

                GameObject newDonutBullet = Instantiate(donutBulletPrefab, new Vector3(x, y, 0) + firePoint.position, Quaternion.identity);


                newDonutBullet.name = donutBulletPrefab.name;
                newDonutBullet.GetComponent<Bullet>().speed = donutBulletSpeed;
            }

            yield return new WaitForSeconds(donutShotDelay);
        }
    }


}


