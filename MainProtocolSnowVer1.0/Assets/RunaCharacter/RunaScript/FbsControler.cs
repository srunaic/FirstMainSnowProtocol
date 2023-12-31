using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
public class FbsControler : MonoBehaviour
{
    [Header("�����Ӱ���")]
    public float moveSpeed = 20f; // �̵� �ӵ� ���� ����
    public float jumpHeight = 2f; // ���� �� ���� ����
    public float feetHeight = 1f; // �� ���� ���� ����
    public float checkHeight = 0.4f; // üũ ���� ���� ����
    public float rotateSpeed = 100f;

    public float rotLookSpeed = 3f;//ȸ���ӵ�
    [Header("�� ó�� �κ�")]
    [SerializeField]
    private bool isGrounded; // ĳ���Ͱ� ���� ��� �ִ��� ���θ� �Ǵ��ϱ� ���� ����
    public bool onGround;
    
    [Header("�߷� ���ӵ�")]
    private Rigidbody rb;
    private float gravity;
    public float VelocityY = -22.0f;

    Transform camTransform;

    [Header("ĳ���� �ִϸ��̼� ����")]

    public Animator RunaAnim;

    [Header("ĳ���� �ִϸ��̼� bool�� ����")]
    public bool onTarget = false;//Ÿ���� ������, ������ ����.
    public bool isMove = false;

    public bool onMoveable = true;

    public bool OnSit = false;

    private float RunSpeed = 4f;
    private float BaseSpeed = 2f;

    private void Start()
    {
        gravity = -Physics.gravity.y;
        rb = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;

        RunaAnim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            // ĳ���Ͱ� ���� ��� �ִ��� �˻�
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }
    }
    private void Update()
    {
        if (onMoveable)//�����̴� �����ΰ�?.
        {
            Move(); //������.
        }
        else
            rb.velocity = Vector3.zero + Vector3.up * VelocityY;

        //��簢�ε� �÷��̾ ������ ��� �ִٸ� ����.
        if (isGrounded)
        {
            SlopeGrounded();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(CheckPose());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            RunaAnim.SetTrigger("Hi");
        }
    }
    IEnumerator CheckPose() //�� ���ǵ�
    {
        yield return new WaitForSeconds(1f);
        onMoveable = false;
        RunaAnim.SetTrigger("isPose");
        yield return new WaitForSeconds(6.7f);
        onMoveable = true;
    }

    public void Move()
    {
        // Ű���� �Է��� �޾� �̵� �� ���� ����
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //�⺻ ������ ��.
        Vector3 MovePlayer = new Vector3(horizontalInput, verticalInput);

        Vector3 cameraForward = Camera.main.transform.forward;//ī�޶� ���� ������
        cameraForward.y = 0f; // y �� ������ ������� ����

        Vector3 moveDirection = (cameraForward * verticalInput + Camera.main.transform.right * horizontalInput).normalized;
        //ķ�� ���� ����� �÷��̾� ������ �ʱ�ȭ ���ִ� ������.

        Vector3 movement = moveDirection * moveSpeed; //�⺻�ӷ�

        //�ִϸ��̼� ����.
        if (moveDirection != Vector3.zero) //���� �ʱ�ȭ ���.
        {
            isMove = true;
            if (isMove)
            {
                RunaAnim.SetFloat("InputX", MovePlayer.x);
                RunaAnim.SetFloat("InputY", MovePlayer.y);
            }
            /*//ĳ���� ȸ������ ī�޶� ���� ��������
            Quaternion rotNext = Quaternion.LookRotation(MovePlayer);

            Quaternion camFront
                = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
            transform.rotation
                = Quaternion.Lerp(transform.rotation, camFront,
                 Time.deltaTime * rotateSpeed);*/
        }

        //ó�� ������ �ʱ�ȭ ��Ŵ. ����Ʈ ����Ʈ �����ÿ� �޸�.
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            RunaAnim.SetBool("isRun", true);
            moveSpeed *= RunSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunaAnim.SetBool("isRun", false);
            moveSpeed = BaseSpeed;
        }
        //ĳ���Ϳ� ī�޶� �پ� �ִٸ� �̷��� ����
        //Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y�� Quaternion ī�޶� �ڵ忡 ����ؼ� �����ش�.
        //Vector3 movement = camRoty * moveDirection * moveSpeed;

        if (horizontalInput != 0 || verticalInput != 0)//Ű�� �Է� �޾�����,
        {
            Quaternion rotMove = Quaternion.LookRotation(moveDirection); //����ȸ�� �ٶ󺸵��� �ϱ�.
            transform.rotation = rotMove;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);//������ �ٶ󺸵���.
        }

        // ĳ������ ���� ó��
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            RunaAnim.SetTrigger("Jump");
            float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

            rb.velocity = movement + Vector3.up * jumpSpeed * 1f;
            isGrounded = false;
        }
        rb.velocity = movement + Vector3.up * rb.velocity.y;

        //���� �ƴҶ�,
        if(!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                RunaAnim.SetTrigger("highLanding");
                Physics.gravity = new Vector3(0, VelocityY, 0);//��ü �߷� ���� ���
     
                //�� �ڵ忡 ���� ���߷°� �߷����� �ٲ�.
            }
        }
        /*
        // ���� ���� �ƴ϶�� ������ ���� �ǵ���
        if (!isGrounded)
        {
            int MaxHeight = 50; //�ִ�ġ ���� ����
            int LimitHeight = 0;

            if (Input.GetKey(KeyCode.F))
            {
                float flySpeed = 2f;
                RunaAnim.SetBool("isRun", false);
                RunaAnim.SetTrigger("Flying");
   
                Vector3 _flyVec = Vector3.up + transform.position;
                rb.velocity = _flyVec * flySpeed;
                Quaternion FlyingMove = Quaternion.Euler(0f, _flyVec.y, 0f);

            }

            else if (Input.GetKeyDown(KeyCode.G))
            {
                float DownSpeed = 2f;

                if (LimitHeight <= MaxHeight)
                {
                    LimitHeight--;
                    Debug.Log("�� ���� ������" + LimitHeight);
                    Vector3 _flyVec2 = Vector3.down + transform.position.normalized;
                    rb.velocity = _flyVec2 * DownSpeed;
                    Physics.gravity = new Vector3(0, -2f, 0);
                    Quaternion FlyingMove = Quaternion.Euler(0f, _flyVec2.y, 0f);
                }

            }
        
            // �߰�: ĳ���Ͱ� ���� �ƴ� ���, ���� �ӵ��� ������ 0���� ����
            if (rb.velocity.y > 0)
            {
                RunaAnim.SetTrigger("highLanding");
                Physics.gravity = new Vector3(0, -22.0f, 0);//�߷� ����
                //�� �ڵ忡 ���� ���߷°� �߷����� �ٲ�.
            }
        }
        else if (isGrounded)
        {
            RunaAnim.ResetTrigger("Flying");
        }
    
        Vector3 feetPos = transform.position + Vector3.down * feetHeight;
        Color color = isGrounded ? Color.green : Color.red;
        Debug.DrawLine(feetPos, feetPos + Vector3.down * checkHeight, color);
        */
    }
    //���鿡���� ������ ����
    public void SlopeGrounded()
    {
        RaycastHit hit;
        float slopeAngle = 0f;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, checkHeight + 0.1f))
        {
            slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
        }

        if (slopeAngle > 0.1f && slopeAngle < 45f)
        {
            Vector3 slopeDirection = Vector3.Cross(hit.normal, -transform.right);

            float slopeForce = Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * moveSpeed;

            Vector3 slopeForceVector = slopeDirection * slopeForce;

            rb.AddForce(slopeForceVector, ForceMode.Acceleration);
        }

    }
    public void Rotate()
    {
        /*// ���ʹϿ� ȸ���� �����
         Quaternion rotY = Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0);//y���� �������� ȸ���Ѵ�.
         transform.rotation = rotY * transform.rotation; //���� ȸ���ӵ����� ���ʹϿ� ���� ������.*/

        /*
        transform.RotateAround(Vector3.zero,Vector3.up, rotateSpeed * Time.deltaTime);
        //ī�޶�� ȸ���� �����ֱ�.
        */

        /*float InputKey = 0;
        if(Input.GetKey(KeyCode.Q))
            InputKey = -1f;
        if (Input.GetKey(KeyCode.E))
            InputKey = 1f;*/

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime);//Vector3.up�� new Vector3(0,y��,0)
    }
}
