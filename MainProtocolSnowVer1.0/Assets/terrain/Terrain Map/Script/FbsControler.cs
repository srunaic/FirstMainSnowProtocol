using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Rigidbody rb;
    private float gravity;

    Transform camTransform;

    [Header("ĳ���� �ִϸ��̼� ����")]
    [SerializeField]
    private Animator RunaAnim;
    private bool isWalk = false;

    private void Start()
    {
        gravity = -Physics.gravity.y;
        rb = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;

        RunaAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
        //Rotate();
    }
    public void Move()
    {
        // Ű���� �Է��� �޾� �̵� �� ���� ����
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;//ī�޶� ���� ������
        cameraForward.y = 0f; // y �� ������ ������� ����

        Vector3 moveDirection = (cameraForward * verticalInput + Camera.main.transform.right * horizontalInput).normalized;
        //ķ�� ���� ����� �÷��̾� ������ �ʱ�ȭ ���ִ� ������.

        Vector3 movement = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero) //���� �ʱ�ȭ ���.
        {
            isWalk = true;
            RunaAnim.SetBool("RunaWalk", isWalk);
        }
        else
        {
            isWalk = false;
            RunaAnim.SetBool("RunaWalk", isWalk);
        }
        //Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y�� Quaternion ī�޶� �ڵ忡 ����ؼ� �����ش�.
        //Vector3 movement = camRoty * moveDirection * moveSpeed;

        if (horizontalInput !=0 || verticalInput != 0)//Ű�� �Է� �޾�����,
        {
            Quaternion rotMove = Quaternion.LookRotation(moveDirection); //����ȸ�� �ٶ󺸵��� �ϱ�.
            transform.rotation = rotMove;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);//������ �ٶ󺸵���.
        }
       
        // ĳ������ ���� ó��
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

            rb.velocity = movement + Vector3.up * jumpSpeed;
            isGrounded = false;
        }
        rb.velocity = movement + Vector3.up * rb.velocity.y;

        Vector3 feetPos = transform.position + Vector3.down * feetHeight;
        Color color = Color.red;
        if (isGrounded) color = Color.green;
        Debug.DrawLine(feetPos, feetPos + Vector3.down * checkHeight, color);

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

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            // ĳ���Ͱ� ���� ��� �ִ��� �˻�
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }
    }
}
