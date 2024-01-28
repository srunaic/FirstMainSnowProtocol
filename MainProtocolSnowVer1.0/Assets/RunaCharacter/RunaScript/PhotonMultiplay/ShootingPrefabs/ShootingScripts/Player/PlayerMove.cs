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
    // �÷��̾� �ʱ� ��ġ ����
    static public Vector2 InitialPosition { get; private set; }

    [Header("ü�°���")]
    public int MaxHealth;
    public int currentHealth;

    public HealthBar healthBar;

    [Header("MoveOption")]
    public bool movable = true;
    public float moveSpeed = 5f;

    [Header("ShotOption")]
    public GameObject bulletPrefab; //�߻��� �Ѿ������� ���

    //�ѽ� ��ġ
    public Transform[] shotPos;
    public int shotLevel = 1;//���� ���ⷡ��1
    int maxLevel = 5;//�ְ��� 3���� ����.

    bool onShot = false; //�߻�������
    //�÷��̾� ���� ó��.
    public bool isDie = false;

    [Header("������ �´� ����")]

    //������ �´� �ð�
    private bool isInvincible = false;
    public float invincibleDuration = 2f;
    private float invincibleTimer;
    //�̹���
    private SpriteRenderer spriteRenderer;
    [Header("���� ����")]
    public GameObject UltimiteWeapon;//�ñ��� ���� �ߵ�.
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

        currentHealth = MaxHealth; //���� hp = �ִ�ġ
        healthBar.setMaxHealth(MaxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        healthBar.setMaxHealth(MaxHealth); //����
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
        //��������
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            // ���� �ð��� ������ ��
            if (invincibleTimer <= 0)
            {
                isInvincible = false;

                // ���� �ð��� ������ �� ������ �ǵ����� ����
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
                int shotCount = 0;//���� ���� �ö����� ī��Ʈ��.

                while(shotCount < shotLevel) //���� ���ⷡ���� ���緡������ ���ٸ�,
                {
                    if (shotLevel == 1)//����1 �϶�,
                    {
                        if (shotCount == 0) //���� ���ⷡ�� 0�̶��,
                            shotPosNow = shotPos[0].position; //�� ��ġ���� ���.
                          
                    }
                    else if (shotLevel == 2)//����2 �϶�,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 ���� ��� firePoint
                        else if(shotCount == 1)
                            shotPosNow = shotPos[1].position; //1 ���� ��� firePoint
                    }
                    else if (shotLevel == 3) //���� 3�϶�,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 ���� ��� firePoint
                        else if (shotCount == 1)
                            shotPosNow = shotPos[1].position; //2 ���� ��� firePoint
                        else if (shotCount == 2)
                            shotPosNow = shotPos[2].position; //3 ���� ��� firePoint

                    }
                    else if (shotLevel == 4) //���� 3�϶�,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 ���� ��� firePoint
                        else if (shotCount == 1)
                            shotPosNow = shotPos[1].position; //2 ���� ��� firePoint
                        else if (shotCount == 2)
                            shotPosNow = shotPos[2].position; //3 ���� ��� firePoint
                        else if (shotCount == 3)
                            shotPosNow = shotPos[3].position; //3 ���� ��� firePoint
                    }
                    else if (shotLevel == 5) //���� 3�϶�,
                    {
                        if (shotCount == 0)
                            shotPosNow = shotPos[0].position; //1 ���� ��� firePoint
                        else if (shotCount == 1)
                            shotPosNow = shotPos[1].position; //2 ���� ��� firePoint
                        else if (shotCount == 2)
                            shotPosNow = shotPos[2].position; //3 ���� ��� firePoint
                        else if (shotCount == 3)
                            shotPosNow = shotPos[3].position; //3 ���� ��� firePoint
                        else if (shotCount == 4)
                            shotPosNow = shotPos[4].position; //3 ���� ��� firePoint
                    }
                    //�߻�
                    GameObject newBullet
                        = Instantiate(bulletPrefab, shotPosNow, Quaternion.identity);

                    newBullet.SetActive(false);
                    newBullet.name = bulletPrefab.name;
                    newBullet.GetComponent<Bullet>().speed = 10f;

                    newBullet.SetActive(true);

                    shotCount++;
                }
                 //���� �ѽ�� ������ �ð�.
                 yield return new WaitForSeconds(0.3f);
            }
            else
                yield return null;
        }
    }
    public void SetShotLevel(int addLevel) 
    {
        if (shotLevel + addLevel > 3) //���� ���� ������ 3���� ��.
        {
            shotLevel = maxLevel; 
        }
        else
           shotLevel += addLevel;

        if (shotLevel == 1)//���� 1�϶�,
        {
            shotPos[0].gameObject.SetActive(true); //�̹��� ���� �ٸ� �̹��� ó��
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
        }
        else if (shotLevel == 2)
        {
            shotPos[0].gameObject.SetActive(true); //�̹������� ó������.
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
        }
        else if (shotLevel == 3)
        {
            shotPos[0].gameObject.SetActive(true); //�̹������� ó������.
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
        }
        else if (shotLevel == 4)
        {
            shotPos[0].gameObject.SetActive(true); //�̹������� ó������.
            shotPos[1].gameObject.SetActive(true);
            shotPos[2].gameObject.SetActive(true);
            shotPos[3].gameObject.SetActive(true);
        }
        else if (shotLevel == 5)
        {
            shotPos[0].gameObject.SetActive(true); //�̹������� ó������.
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

        // ���� ���¿����� �������� ���� ����
        if (isInvincible)
            return;
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
            gameManager.UltimateWeaponTxt.SetActive(true);//�ñر� ����.

            yield return new WaitForSeconds(3f);
            gameManager.UltimateWeaponTxt.SetActive(false);//�ñر� ����.

        }
    }

    IEnumerator CheckBoomer()
    {
        if (shotLevel == 4)
        {
            Debug.Log("�ñر� �۵�");
            UsingWeapon = true;//�ñر� ������Դϴ�.

            UltimiteWeapon.SetActive(true);//�ñر� �ϳ��� ����ɶ�,
            WeaponMaxCount += 1; //���� ��� ���� ī��Ʈ 1 ����.
            Debug.Log("���� ���� ī��Ʈ" + WeaponMaxCount);
            yield return new WaitForSeconds(3f);
            UltimiteWeapon.SetActive(false);

            if (WeaponMaxCount == 3) //3�� ����.
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
            Debug.Log("���� hp ��" + currentHealth);
            
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            MaxHealth--;
            TakeDamage(1); //���̶� �ε����� 1�� �������� ����.
        }
   
            
    }

    public void Die()
    {
        if (!isDie)
        {
            //SceneManager.LoadScene("Lobby");
            gameManager.SaveGame();//�÷��̾ ������, ���������� ���� ����.

            CancelInvoke(); // ��� Invoke ȣ�� ���

            Destroy(gameObject);

            isDie = true;
        }
    }

}
