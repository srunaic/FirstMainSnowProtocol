using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using ShootingManager;
public enum BulletType //레시피 개념
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

        if (bulletType == BulletType.PlayerBullet) //플레이어라는 그룹으로 지정을 해줌.
        {
            if (collision.tag == "Enemy")
            {
                EnemyMove hitEnemy = collision.GetComponent<EnemyMove>();
           
                //일반몹 데미지
                hitEnemy.EnemyMaxHealth--;

                if (hitEnemy.EnemyMaxHealth <= 0)
                {
                    hitEnemy.Die(); //A 스크립트에서 죽는다는 함수를 들고옴.
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
                    hitBoss.Die(); //A 스크립트에서 죽는다는 함수를 들고옴.
                    manager.ResultPnl.SetActive(true);
                    manager.GameWinPnl.SetActive(true);
                    manager.ResultTxt.text
                       = "" + LobbyManager.PlayerName +
                       "\n최고 점수:" + GameManager.myBestScore +
                       "\n최근 점수:" + GameManager.myLastScore +
                       "\n이번 최종기록:" + GameManager.Score;

                    Time.timeScale = 0f;//게임 정지
                }
                Destroy(gameObject);
                //죽음 관리는 무조건 여기서 해야됨.
            }
        }
        else if (bulletType == BulletType.EnemyBullet)
        {
            if (collision.tag == "Player")//적의 총알이 플레이어와 충돌하게 된다면,
            {
                PlayerMove hitPlayer = collision.GetComponent<PlayerMove>();

                hitPlayer.MaxHealth--;
                hitPlayer.TakeDamage(1);//hp가 따르되 현재 hp에서 1씩 따르도록 만들기.

                if (hitPlayer.MaxHealth <= 0)
                {
                    manager.ResultPnl.SetActive(true);
                    manager.LosePanel.SetActive(true);
                    manager.ResultTxt.text
                       = "" + LobbyManager.PlayerName +
                       "\n최고 점수:" + GameManager.myBestScore +
                       "\n최근 점수:" + GameManager.myLastScore +
                       "\n이번 최종기록:" + GameManager.Score;

                    Time.timeScale = 0f;//게임 정지


                    Debug.Log("데미지 받음");
                    hitPlayer.Die(); //A 스크립트에서 죽는다는 함수를 들고옴.

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
