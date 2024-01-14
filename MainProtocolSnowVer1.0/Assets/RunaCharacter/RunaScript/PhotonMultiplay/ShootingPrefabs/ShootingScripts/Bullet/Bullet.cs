using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using ShootingManager;
public enum BulletType //������ ����
{ 
    PlayerBullet,
    EnemyBullet
}
public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidbody2D;

    public BulletType bulletType;

    public float speed = 1.0f;
    public Vector2 direction = Vector2.up;

    public GameManager manager;
    public LobbyManager Lobby;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;

        rigidbody2D.velocity = direction.normalized * speed;
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "UltimateBullet")
        {
            Destroy(gameObject);
        }

        if (bulletType == BulletType.PlayerBullet) //�÷��̾��� �׷����� ������ ����.
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
                Destroy(gameObject);
            }
            else if (collision.tag == "Boss")
            {
                BossShooting hitBoss = collision.GetComponent<BossShooting>();

                hitBoss.BossMaxHealth--;
                hitBoss.BossTakeDamage(1);

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
                Destroy(gameObject);
                //���� ������ ������ ���⼭ �ؾߵ�.
            }
        }
        else if (bulletType == BulletType.EnemyBullet)
        {
            if (collision.tag == "Player")//���� �Ѿ��� �÷��̾�� �浹�ϰ� �ȴٸ�,
            {
                PlayerMove hitPlayer = collision.GetComponent<PlayerMove>();

                hitPlayer.MaxHealth--;
                hitPlayer.TakeDamage(1);//hp�� ������ ���� hp���� 1�� �������� �����.

                if (hitPlayer.MaxHealth <= 0)
                {
                    manager.ResultPnl.SetActive(true);
                    manager.LosePanel.SetActive(true);
                    manager.ResultTxt.text
                       = "" + LobbyManager.PlayerName +
                       "\n�ְ� ����:" + GameManager.myBestScore +
                       "\n�ֱ� ����:" + GameManager.myLastScore +
                       "\n�̹� �������:" + GameManager.Score;

                    Time.timeScale = 0f;//���� ����


                    Debug.Log("������ ����");
                    hitPlayer.Die(); //A ��ũ��Ʈ���� �״´ٴ� �Լ��� ����.

                }
               Destroy(gameObject);
            }
        }

        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }

    }

}
