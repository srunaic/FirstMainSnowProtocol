using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using ShootingManager;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    GameManager gameManager;
    
    Rigidbody2D rigidbody2D;
    SpriteRenderer pRender;
    // 플레이어 초기 위치 저장
    static public Vector2 InitialPosition { get; private set; }

    [Header("체력관리")]
    public int MaxHealth;
    public int currentHealth;

    public HealthBar healthBar;

    [Header("MoveOption")]
    public bool movable = true;
    public float moveSpeed = 5f;

    [Header("ShotOption")]
    public GameObject bulletPrefab; //발사할 총알프리팹 기억

    //총쏠 위치
    public Transform[] shotPos;
    public int shotLevel = 1;//현재 무기래벨1
    int maxLevel = 5;//최고래벨 3으로 제한.

    bool onShot = false; //발사중인지
    //플레이어 죽음 처리.
    public bool isDie = false;

    [Header("적한테 맞는 로직")]

    //적한테 맞는 시간
    private bool isInvincible = false;
    public float invincibleDuration = 2f;
    private float invincibleTimer;
    //이미지
    private SpriteRenderer spriteRenderer;
    [Header("무기 생성")]
    public GameObject UltimiteWeapon;//궁극의 무기 발동.
    public float WeaponMaxCount = 0f;
    public bool UsingWeapon = false;

    private float timer = 0f;
    private float InTime = 3f;

    void Start()
    {
        StartCoroutine(CheckBoomTxt());
        StartCoroutine(CheckBoomer());

        currentHealth = 100;
        InitialPosition = transform.position;

        currentHealth = MaxHealth; //현재 hp = 최대치
        healthBar.setMaxHealth(MaxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        healthBar.setMaxHealth(MaxHealth); //참조
    }

    private void OnEnable() 
    {
        StartCoroutine(ShotAct());
    }
    void Update()
    {
        if (movable)
        {
            Move();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            onShot = true;
        }
        else
            onShot = false;
    }

    private void FixedUpdate()
    {
        //무적상태
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            // 무적 시간이 끝났을 때
            if (invincibleTimer <= 0)
            {
                isInvincible = false;

                // 무적 시간이 끝났을 때 색상을 되돌리는 로직
                ResetColor();
            }
        }
    }
    void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        rigidbody2D.velocity = new Vector3(inputX, verticalInput) * moveSpeed;
    }

    IEnumerator ShotAct() 
    {
        while (true)
        {
            if (onShot)
            {
                Vector3 shotPosNow = shotPos[0].position; //shotPos = transform
                int shotCount = 0;//무기 래벨 올랐는지 카운트값.

                while(shotCount < shotLevel) //현재 무기래벨이 현재래벨보다 낮다면,
                {
                    if (shotLevel == 1)//래벨1 일때,
                    {
                        if (shotCount == 0) //현재 무기래벨 0이라면,
                            shotPosNow = shotPos[0].position; //이 위치에서 쏜다.
                          
                    }
                    else if (shotLevel == 2)//래벨2 일때,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 현재 쏘는 firePoint
                        else if(shotCount == 1)
                            shotPosNow = shotPos[1].position; //1 현재 쏘는 firePoint
                    }
                    else if (shotLevel == 3) //래벨 3일때,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 현재 쏘는 firePoint
                        else if (shotCount == 1)
                            shotPosNow = shotPos[1].position; //2 현재 쏘는 firePoint
                        else if (shotCount == 2)
                            shotPosNow = shotPos[2].position; //3 현재 쏘는 firePoint

                    }
                    else if (shotLevel == 4) //래벨 3일때,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 현재 쏘는 firePoint
                        else if (shotCount == 1)
                            shotPosNow = shotPos[1].position; //2 현재 쏘는 firePoint
                        else if (shotCount == 2)
                            shotPosNow = shotPos[2].position; //3 현재 쏘는 firePoint
                        else if (shotCount == 3)
                            shotPosNow = shotPos[3].position; //3 현재 쏘는 firePoint
                    }
                    else if (shotLevel == 5) //래벨 3일때,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 현재 쏘는 firePoint
                        else if (shotCount == 1)
                            shotPosNow = shotPos[1].position; //2 현재 쏘는 firePoint
                        else if (shotCount == 2)
                            shotPosNow = shotPos[2].position; //3 현재 쏘는 firePoint
                        else if (shotCount == 3)
                            shotPosNow = shotPos[3].position; //3 현재 쏘는 firePoint
                        else if (shotCount == 4)
                            shotPosNow = shotPos[4].position; //3 현재 쏘는 firePoint
                    }
                    //발사
                    GameObject newBullet
                        = Instantiate(bulletPrefab, shotPosNow, Quaternion.identity);

                    newBullet.SetActive(false);
                    newBullet.name = bulletPrefab.name;
                    newBullet.GetComponent<Bullet>().speed = 10f;

                    newBullet.SetActive(true);

                    shotCount++;
                }
                 //최초 총쏘고 딜레이 시간.
                 yield return new WaitForSeconds(0.3f);
            }
            else
                yield return null;
        }
    }
    public void SetShotLevel(int addLevel) 
    {
        if (shotLevel + addLevel > 3) //현재 래벨 제한을 3으로 둠.
        {
            shotLevel = maxLevel; 
        }
        else
           shotLevel += addLevel;

        if (shotLevel == 1)//래벨 1일때,
        {
            shotPos[0].gameObject.SetActive(true); //이미지 마다 다른 이미지 처리
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
        }
        else if (shotLevel == 2)
        {
            shotPos[0].gameObject.SetActive(true); //이미지마다 처리해줌.
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
        }
        else if (shotLevel == 3)
        {
            shotPos[0].gameObject.SetActive(true); //이미지마다 처리해줌.
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
        }
        else if (shotLevel == 4)
        {
            shotPos[0].gameObject.SetActive(true); //이미지마다 처리해줌.
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
            shotPos[3].gameObject.SetActive(true);
        }
        else if (shotLevel == 5)
        {
            shotPos[0].gameObject.SetActive(true); //이미지마다 처리해줌.
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
            shotPos[3].gameObject.SetActive(true);
            shotPos[4].gameObject.SetActive(true);
        }
    }

  
    public void TakeDamage(int EnemydamageAmount)
    {   
        currentHealth -= EnemydamageAmount;
        healthBar.setHealth(currentHealth); // Update the health bar when taking damage.

        // 무적 상태에서는 데미지를 받지 않음
        if (isInvincible)
            return;
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

    private void SetInvincible()
    {
        isInvincible = true;
        invincibleTimer = invincibleDuration;

        Invoke("ResetColor", 1.0f);
    }

    private void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }

    IEnumerator CheckBoomTxt()
    {
        if (shotLevel >= 4)
        {
            yield return new WaitForSeconds(0.2f);
            gameManager.UltimateWeaponTxt.SetActive(true);//궁극기 등장.

            yield return new WaitForSeconds(3f);
            gameManager.UltimateWeaponTxt.SetActive(false);//궁극기 등장.

        }
    }

    IEnumerator CheckBoomer()
    {
        if (shotLevel == 4)
        {
            Debug.Log("궁극기 작동");
            UsingWeapon = true;//궁극기 사용중입니다.

            UltimiteWeapon.SetActive(true);//궁극기 하나가 실행될때,
            WeaponMaxCount += 1; //무기 사용 제한 카운트 1 증가.
            Debug.Log("무기 제한 카운트" + WeaponMaxCount);
            yield return new WaitForSeconds(3f);
            UltimiteWeapon.SetActive(false);

            if (WeaponMaxCount == 3) //3번 제한.
            {
                UsingWeapon = false;
            }
        }
        else if (!UsingWeapon)
        {
            Destroy(UltimiteWeapon);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Potion")&& currentHealth <= 90)
        {
            Potion HPplayer = col.GetComponent<Potion>();
            currentHealth += HPplayer.Healing;
            Debug.Log("현재 hp 량" + currentHealth);
            
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            MaxHealth--;
            TakeDamage(1); //적이랑 부딪힐때 1씩 데미지를 받음.
        }
   
            
    }

    public void Die()
    {
        if (!isDie)
        {
            //SceneManager.LoadScene("Lobby");
            gameManager.SaveGame();//플레이어가 죽을때, 마지막으로 게임 저장.

            CancelInvoke(); // 모든 Invoke 호출 취소

            Destroy(gameObject);

            isDie = true;
        }
    }

}
