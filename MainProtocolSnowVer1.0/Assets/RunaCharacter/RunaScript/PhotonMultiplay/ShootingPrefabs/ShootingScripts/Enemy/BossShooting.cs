using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ShootingManager;
public class BossShooting : MonoBehaviour
{ 
    Rigidbody2D rigidbody2D;

    [Header("����� ������")]
    public GameObject DropItem;
    public float dropRate = 1.0f;//��� Ȯ��. // 0~100%

    [Header("���ߵ� ��ƼŬ")]
    public ParticleSystem ExEffect;

    [Header("ü�°���")]
    public int BossMaxHealth;//���� ������ 5
    public int currentHealth;

    public HealthBar healthBar;

    [Header("�� ������")]
    public float MoveSpeed = 0.5f;
    [Header("�ѽ�� �ɼ�")]
    public GameObject bulletPrefab;
    public float shotDelay = 1.0f; //�߻� �ֱ�
    public int shotCount = 3;//���ӹ߻� Ƚ��
    public float shotDist = 0.5f; //���ӹ߻� ����

    public float shotRotate = 90f;//��� �Ѿ��� ��� ��
    public float startRotate = -45f;//���� �Ѿ� ��
    [Header("��������")]
    public int getScore = 10;

    public Vector2 ShotDirection = Vector2.down;

    [Header("������ �´� ����")]
    private SpriteRenderer spriteRenderer;

    public Transform firePoint;

    [Header("����� �̻���")]
    public GameObject radialBulletPrefab;
    public float radialShotDelay = 3.0f; // ����� �̻��� �߻� �ֱ�
    public int radialBulletCount = 8; // ����� �̻��� ����
    public float radialBulletSpeed = 8.0f; // ����� �̻��� �ӵ�
    public float speed;

    [Header("���� �̻���")]
    public GameObject donutBulletPrefab;
    public float donutBulletCount = 12;
    public float donutRadius = 2.0f;
    public float donutShotDelay = 1.0f;
    public float donutBulletSpeed;

    void Start()
    {
        BossMaxHealth = 100;
        currentHealth = BossMaxHealth; //���� hp = �ִ�ġ
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
    //������ �޴� ������.
    public void BossTakeDamage(int PlayerdamageAmount)
    {
        currentHealth -= PlayerdamageAmount;//�÷��̾� ���� �޴� ������.
        healthBar.setHealth(currentHealth); // Update the health bar when taking damage.

        // �������� ������ ������ ���ϵ��� ���� ����
        spriteRenderer.color = Color.red;
        //�������� ���� �Լ�.
        SetInvincible();
        // �������� ���� �� üũ�Ͽ� �ٽ� ���� �������� �ǵ�����
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
        //���� �÷���
        GameManager.Score += getScore;
        Boss.BossCount += 1;
        //ī��Ʈ�� �����ϴ� ��ũ��Ʈ
        //ī��Ʈ Ƚ���� ������ ���� ����.

        CancelInvoke(); // ��� Invoke ȣ�� ���

        if (DropItem != null)//������ ���� Ȯ��
        {
            float rand = Random.Range(0f, 100f);
            Debug.Log("Ȯ������" + rand);

            if (rand <= dropRate)
                Instantiate(DropItem, transform.position, Quaternion.identity);
        }

        Instantiate(ExEffect, transform.position, Quaternion.identity);
        ExEffect.Play();
        //SceneManager.LoadScene("Lobby");
        Destroy(gameObject);//�״� ������
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

            //�߻�� ȸ�� �� = ��ü ȸ���� / �߻� Ƚ�� 
            float oneRotate = shotRotate / (shotCount - 1);
            //shotCount == 3�̶�� 0,1,2���� �ݺ�
            while (nowCount < shotCount)
            {
                //�� �Ѿ� ����(�߻�)
                GameObject newBullet
                = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                //�߻� ���� ���
                Vector3 shotDirNow = ShotDirection;
                float shotAxis = startRotate + oneRotate * nowCount;
                Quaternion ShotRotQuat = Quaternion.Euler(0, 0, shotAxis);

                shotDirNow = ShotRotQuat * shotDirNow;

                //�� �Ѿ� ����
                newBullet.name = bulletPrefab.name;
                newBullet.GetComponent<Bullet>().speed = 10f;
                newBullet.GetComponent<Bullet>().direction = shotDirNow;

                //�Ѿ��� ȸ������
                newBullet.transform.rotation = Quaternion.Euler(0, 0, startRotate + oneRotate * nowCount);

                if (shotDist > 0)
                    yield return new WaitForSeconds(shotDist);
                //�߻� ���� ���

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
                // ���� ���
                float angle = i * (360f / radialBulletCount);

                // ����� �̻��� ���� ��ġ ���
                Vector3 bulletPosition = firePoint.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);

                // ����� �̻��� ����
                GameObject newRadialBullet = Instantiate(radialBulletPrefab, bulletPosition, Quaternion.Euler(0, 0, angle));

                // ����� �̻��� ����
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


