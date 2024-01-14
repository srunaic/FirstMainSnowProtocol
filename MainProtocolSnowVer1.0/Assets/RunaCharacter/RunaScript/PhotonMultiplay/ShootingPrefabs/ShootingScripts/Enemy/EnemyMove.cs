using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using ShootingManager;
public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigidbody2D;

    [Header("����� ������")]
    public GameObject[] DropItem;
    public float dropRate = 0.3f;//��� Ȯ��. // 0~100%

    [Header("���ߵ� ��ƼŬ")]
    public ParticleSystem ExEffect;

    [Header("ü�°���")]
    public int EnemyMaxHealth = 0;//���� ������ 
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
        //���� �÷���
        GameManager.Score += getScore;
        Boss.BossCount += 1;
        //ī��Ʈ�� �����ϴ� ��ũ��Ʈ
        //ī��Ʈ Ƚ���� ������ ���� ����.

        if (DropItem != null)//������ ���� Ȯ��
        {
            float rand = Random.Range(0f, 100f);
            Debug.Log("Ȯ������" + rand);
            if (rand <= dropRate)
            {
                int randomIndex = Random.Range(0, DropItem.Length);
                Instantiate(DropItem[randomIndex], transform.position, Quaternion.identity);
            }
        }

        Instantiate(ExEffect, transform.position, Quaternion.identity);
        ExEffect.Play();
        Destroy(gameObject);//�״� ������
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

            //�߻�� ȸ�� �� = ��ü ȸ���� / �߻� Ƚ�� 
            float oneRotate = shotRotate / (shotCount-1);
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
}
