using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class bakhochanMove : MonoBehaviour
{
    [Header("ĳ���� ������ ����")]
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
    private bool isfly = false; //�� �� ������ ��,
    private int Count2 = 0; // �ö��� �� ī��Ʈ  �ٿ� 
    //---------------------------
    Vector2 runInput;
    Vector2 walkInput;

    public Animator moveAni;

    [Header("���� Ʈ�� ������ ����.")]
    public float RunSpeed = 1f;
    public float Movespeed = 1f;
    bool isWalk = false;

    [Header("�ΰ��� ����")]
    public Rigidbody rigid;
    private float sensitity = 3f; //������.
    private bool grounded = false;
    private float timer;//Ÿ�̹� ����.

    private bool jDown; //����Ű ����.

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        moveAni = characterBody.GetComponent<Animator>();//�� �θ��� ������ ����.
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

        GetInput(); //��ư �̺�Ʈ��
        Floor();
        gravit();//�޴� �߷��� ��.
        Move();
        Jump();
        LookAround();

    }

    void gravit()
    {
        Physics.gravity = new Vector3(0, -22.0f, 0); //���� �ӵ� �߷� ���ӵ� ��������.
    }
    void GetInput()
    {
        jDown = Input.GetButtonDown("Jump"); //����Ű project����
    }
    //ī�޶� �ٷ� ���� ���� ����� �ڵ�
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
            currentSpeed = RunSpeed; // �ٴ� �ӵ��� ����
        }

        characterBody.forward = walkDir;

        transform.transform.position += Vector3.ClampMagnitude(walkDir, 1f) * Time.deltaTime * 5;
    }
    void Floor()
    {
        float rayDistance = 0.8f; //���� �ɽ�Ʈ �ش� ���̾ �ɷ�����, �۵���.
        Vector3 rayOrigin = (this.transform.position + (Vector3.up * rayDistance * 0.5f));//�ش� ���� ��ġ��
        grounded = Physics.Raycast(rayOrigin, Vector3.down, rayDistance, LayerMask.GetMask("BlockingLayer"));
        //���� ������ bool ���� grounded plane�� ���̾� ����.����ߵ�.

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

        if (timer >= 2f && isJump)//������ �ε巴��.
        {
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);

            timer = 0.0f;
        }

    }
    void OnTriggerStay(Collider other) //flying mode
    {
        if (Count2 == 0 && isfly == true && Input.GetKey(KeyCode.Space))
        {
            if (300 >= transform.position.y) //��������
            {

                //rigid.AddForce(transform.up * 17.0f, ForceMode.Acceleration);//�̷�
                rigid.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
            }

        }

        if (Input.GetKeyDown(KeyCode.F) && grounded == false) //f�� ��������
        {

            moveAni.SetTrigger("flying");
            moveAni.SetBool("isfly", true);
            isfly = true; //���� ��Ǹ�


            fire.gameObject.SetActive(true);
            FlyDevice.gameObject.SetActive(true);
            Flybone.gameObject.SetActive(true);

            if (FlyDevice == true && Flybone == true && grounded == true) //f�� �������� ���� ���� �߹��� �Ǹ�
            {
                FlyDevice.gameObject.SetActive(false);
                Flybone.gameObject.SetActive(false);

                moveAni.SetTrigger("flying");
                moveAni.SetBool("isfly", false);//�ѹ��� ���
                isfly = false; //���� ��� ��������.

                moveAni.SetBool("isJump", false); //�� ����.
                isJump = false;
                moveAni.SetBool("ishigh", false); //���� ���Ͻÿ� ����
                ishigh = false;
            }
        }


        if (Input.GetKey(KeyCode.G) && other.gameObject.tag == "flyingZone")
        {

            rigid.AddForce(Vector3.down * 10.0f, ForceMode.Acceleration); //���� G Ű �� ������ ������ õõ�� ������..

            if (grounded == true)
            {
                moveAni.SetBool("isJump", true);
                isJump = true; //���� ���� �ѹ��� ���
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
            moveAni.SetBool("isJump", false); //���� ���Ͻÿ� ����  
            isJump = false;

            moveAni.SetBool("isfly", false);
            isfly = false; //flying
        }


        else if (isfly == true && col.gameObject.tag == "Ground")
        {

            moveAni.SetBool("isJump", true);
            isJump = true; //���� ����


            if (col.gameObject.tag != "Ground")
            {

                moveAni.SetBool("isJump", false);
                isJump = false; //���� ����

                moveAni.SetBool("ishigh", false);
                ishigh = false;
            }

        }

    }




}





