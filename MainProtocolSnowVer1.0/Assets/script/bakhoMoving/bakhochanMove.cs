using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class bakhochanMove : MonoBehaviour
{
    [Header("캐릭터 움직임 구현")]
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;

    public GameObject FlyDevice;
    public GameObject Flybone;
    public ParticleSystem fire;
    public ParticleSystem fire2;

    public bool isJump = false;
    public bool ishigh = false;
    private bool isfly = false; //두 번 눌렸을 시,
    private int Count2 = 0; // 플라이 시 카운트  다운 
    //---------------------------
    Vector2 runInput;
    Vector2 walkInput;

    public Animator moveAni;

    [Header("블랜드 트리 움직임 제어.")]
    public float RunSpeed = 1f;
    public float Movespeed = 1f;
    bool isWalk = false;

    [Header("민감도 관리")]
    public Rigidbody rigid;
    private float sensitity = 3f; //스무스.
    private bool grounded = false;
    private float timer;//타이밍 설정.

    private bool jDown; //점프키 지정.

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        moveAni = characterBody.GetComponent<Animator>();//이 부모의 축으로 설정.
    }
    void FixedUpdate()
    {
        if (characterBody == null)
        {
            gameObject.transform.position = characterBody.transform.position;
            characterBody.transform.parent = gameObject.transform;
            return;
        }
        if (moveAni == null)
        {
            moveAni = characterBody.GetComponent<Animator>();
        }

        GetInput(); //버튼 이벤트들
        Floor();
        gravit();//받는 중력의 힘.
        Move();
        Jump();
        LookAround();

    }

    void gravit()
    {
        Physics.gravity = new Vector3(0, -22.0f, 0); //낙하 속도 중력 가속도 조절가능.
    }
    void GetInput()
    {
        jDown = Input.GetButtonDown("Jump"); //점프키 project세팅
    }
    //카메라 바로 앞을 보게 만드는 코드
    void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x * sensitity, camAngle.z);

    }
    void Move()
    {
        walkInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isWalk = true;
            if(isWalk) 
            {
                moveAni.SetFloat("speed", Movespeed);
            }
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            isWalk = false;
        }

        Vector3 lookforward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        Vector3 walkDir = lookforward * walkInput.y + lookRight * walkInput.x;
      
        float currentSpeed = Movespeed;

        if (Input.GetKey(KeyCode.LeftShift))
        { 
            currentSpeed = RunSpeed; // 뛰는 속도로 변경
        }

        characterBody.forward = walkDir;

        transform.transform.position += Vector3.ClampMagnitude(walkDir, 1f) * Time.deltaTime * 5;
    }
    void Floor()
    {
        float rayDistance = 0.8f; //레이 케스트 해당 레이어에 걸렸을때, 작동함.
        Vector3 rayOrigin = (this.transform.position + (Vector3.up * rayDistance * 0.5f));//해당 구역 위치값
        grounded = Physics.Raycast(rayOrigin, Vector3.down, rayDistance, LayerMask.GetMask("BlockingLayer"));
        //위에 생성한 bool 값의 grounded plane에 레이어 설정.해줘야됨.

    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            moveAni.SetBool("isJump", true);
            moveAni.SetTrigger("Jump");
            isJump = true;
        }
        timer += Time.deltaTime;

        if (timer >= 2f && isJump)//점프를 부드럽게.
        {
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);

            timer = 0.0f;
        }

    }
    void OnTriggerStay(Collider other) //flying mode
    {
        if (Count2 == 0 && isfly == true && Input.GetKey(KeyCode.Space))
        {
            if (300 >= transform.position.y) //높이제한
            {

                //rigid.AddForce(transform.up * 17.0f, ForceMode.Acceleration);//이륙
                rigid.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
            }

        }

        if (Input.GetKeyDown(KeyCode.F) && grounded == false) //f를 눌렀을때
        {

            moveAni.SetTrigger("flying");
            moveAni.SetBool("isfly", true);
            isfly = true; //나는 모션만


            fire.gameObject.SetActive(true);
            FlyDevice.gameObject.SetActive(true);
            Flybone.gameObject.SetActive(true);

            if (FlyDevice == true && Flybone == true && grounded == true) //f가 눌렸을때 만약 점프 발버둥 되면
            {
                FlyDevice.gameObject.SetActive(false);
                Flybone.gameObject.SetActive(false);

                moveAni.SetTrigger("flying");
                moveAni.SetBool("isfly", false);//한번더 명시
                isfly = false; //나는 모션 하지마라.

                moveAni.SetBool("isJump", false); //걍 꺼라.
                isJump = false;
                moveAni.SetBool("ishigh", false); //착지 땅일시에 실행
                ishigh = false;
            }
        }


        if (Input.GetKey(KeyCode.G) && other.gameObject.tag == "flyingZone")
        {

            rigid.AddForce(Vector3.down * 10.0f, ForceMode.Acceleration); //강하 G 키 꾹 누르고 있으면 천천히 내려감..

            if (grounded == true)
            {
                moveAni.SetBool("isJump", true);
                isJump = true; //착지 동작 한번더 명시
            }

        }

        else if (isfly == true && Input.GetKeyDown(KeyCode.LeftShift))
        {

            fire.gameObject.SetActive(false);
            fire2.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            fire.gameObject.SetActive(true);
            fire2.gameObject.SetActive(false);
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (grounded == true)
        {
            FlyDevice.gameObject.SetActive(false);
            Flybone.gameObject.SetActive(false);
            moveAni.SetBool("isJump", false); //착지 땅일시에 실행  
            isJump = false;

            moveAni.SetBool("isfly", false);
            isfly = false; //flying
        }


        else if (isfly == true && col.gameObject.tag == "Ground")
        {

            moveAni.SetBool("isJump", true);
            isJump = true; //착지 동작


            if (col.gameObject.tag != "Ground")
            {

                moveAni.SetBool("isJump", false);
                isJump = false; //착지 동작

                moveAni.SetBool("ishigh", false);
                ishigh = false;
            }

        }

    }




}





