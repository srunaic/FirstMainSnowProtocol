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
            //일반몹 데미지
            hitEnemy.EnemyMaxHealth--;

            if (hitEnemy.EnemyMaxHealth <= 0)
            {
                hitEnemy.Die(); //A 스크립트에서 죽는다는 함수를 들고옴.
            }
        }
      
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Boss")
        {
            BossShooting hitBoss = collision.GetComponent<BossShooting>();

            hitBoss.BossTakeDamage(1);//데미지 관리는 따로 해야됨 안 그럼 로직 작동안함.
            hitBoss.BossMaxHealth--;

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
        }
    }
}
