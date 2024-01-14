using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootingManager;

public class UltimateBullet : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    [SerializeField]
    private PlayerMove playermove;
    public float speed = 1.0f;
    public Vector2 direction = Vector2.up;

    public GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        playermove = FindObjectOfType<PlayerMove>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;

        rigidbody2D.velocity = direction.normalized * speed;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyMove hitEnemy = collision.GetComponent<EnemyMove>();
            //�Ϲݸ� ������
            hitEnemy.EnemyMaxHealth--;

            if (hitEnemy.EnemyMaxHealth <= 0)
            {
                hitEnemy.Die(); //A ��ũ��Ʈ���� �״´ٴ� �Լ��� ����.
            }
        }
      
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            BossShooting hitBoss = collision.GetComponent<BossShooting>();

            hitBoss.BossTakeDamage(1);//������ ������ ���� �ؾߵ� �� �׷� ���� �۵�����.
            hitBoss.BossMaxHealth--;

            if (hitBoss.BossMaxHealth <= 0)
            {
                hitBoss.Die(); //A ��ũ��Ʈ���� �״´ٴ� �Լ��� ����.
                manager.ResultPnl.SetActive(true);
                manager.GameWinPnl.SetActive(true);
                manager.ResultTxt.text
                   = "" + LobbyManager.PlayerName +
                   "\n�ְ� ����:" + GameManager.myBestScore +
                   "\n�ֱ� ����:" + GameManager.myLastScore +
                   "\n�̹� �������:" + GameManager.Score;

                Time.timeScale = 0f;//���� ����
            }
        }
    }
}
