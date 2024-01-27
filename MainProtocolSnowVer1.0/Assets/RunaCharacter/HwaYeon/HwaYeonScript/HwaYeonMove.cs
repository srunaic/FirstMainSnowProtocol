using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeonghyoGameManagerGroup;

public enum CheckHwaYeonState //���� ���� �÷��̾� ���°�.
{
    None,
    ChatState,
    Sitting,
    DollGames,
    ShotGames
}
public class HwaYeonMove : MonoBehaviour
{
    [Header("�÷��̾��� ���� ������ǥ��")]
    public CheckHwaYeonState _checkstate = CheckHwaYeonState.None;

    [Header("�̱� �񵿱��� �����Ӱ���")]
    public float moveSpeed = 20f; // �̵� �ӵ� ���� ����
    public float jumpHeight = 2f; // ���� �� ���� ����
    public float feetHeight = 1f; // �� ���� ���� ����
    public float checkHeight = 0.4f; // üũ ���� ���� ����
    public float rotateSpeed = 100f;

    public float rotLookSpeed = 0.1f;//ȸ���ӵ�

    [Header("�� ó�� �κ�")]
    [SerializeField]
    private bool isGrounded; // ĳ���Ͱ� ���� ��� �ִ��� ���θ� �Ǵ��ϱ� ���� ����
    public bool onGround;

    [Header("�߷� ���ӵ�")]
    private Rigidbody rb;
    private float gravity;
    public float VelocityY = -22.0f;

    [Header("ĳ���� �ִϸ��̼� ����")]
    public Animator HwaAnim;

    [Header("ĳ���� �ִϸ��̼� bool�� ����")]
    public bool isMove = false;
    public bool OnSit = false;

    [Header("���� �ȱ� �� �޸���")]
    private float RunSpeed = 4f;
    private float BaseSpeed = 2f;

    public bool onMoveable = true;

    void Start()
    {
        _checkstate = CheckHwaYeonState.None;

        gravity = -Physics.gravity.y;
        rb = GetComponent<Rigidbody>();
        HwaAnim = GetComponent<Animator>();

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
        if (onMoveable)
        {
            Move(); // ������ ó��.   
        }
        else
           rb.velocity = Vector3.zero + Vector3.up * VelocityY;

        if (isGrounded)
        {
            SlopeGrounded();
        }
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
        //ķ�� ���� ����� �÷��̾� ������ �ʱ�ȭ ���ִ� ������. ķ���� ������ Right��

        Vector3 movement = moveDirection * moveSpeed; //�⺻�ӷ�

        //�ִϸ��̼� ����.
        if (moveDirection != Vector3.zero) //���� �ʱ�ȭ ���.
        {
            isMove = true;
            if (isMove)
            {
                HwaAnim.SetFloat("MoveDirX", MovePlayer.x);
                HwaAnim.SetFloat("MoveDirY", MovePlayer.y);
            }

        }
        //ó�� ������ �ʱ�ȭ ��Ŵ. ����Ʈ ����Ʈ �����ÿ� �޸�.
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            HwaAnim.SetBool("isRun", true);
            moveSpeed *= RunSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            HwaAnim.SetBool("isRun", false);
            moveSpeed = BaseSpeed;
        }

        if (horizontalInput != 0 || verticalInput != 0)//������ ��,
        {
            //Quaternion rotMove = Quaternion.LookRotation(moveDirection); // �̵� �������� ȸ��
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);

            Quaternion rotMove = Quaternion.LookRotation(moveDirection);//ī�޶� ȸ���������� �ʱ�ȭ.
            transform.rotation = rotMove;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);//������ �ٶ󺸵���.*/

        }


        // ĳ������ ���� ó��
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            HwaAnim.SetTrigger("Jump");
            float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

            rb.velocity = movement + Vector3.up * jumpSpeed * 1f;
            isGrounded = false;
        }
        rb.velocity = movement + Vector3.up * rb.velocity.y;

        //���� �ƴҶ�,
        if (!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                Physics.gravity = new Vector3(0, VelocityY, 0);//��ü �߷� ���� ���

                //�� �ڵ忡 ���� ���߷°� �߷����� �ٲ�.
            }
        }

    }


}
