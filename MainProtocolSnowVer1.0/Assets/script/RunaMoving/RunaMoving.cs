using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunaMoving : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ� ���� ����
    public float jumpHeight = 2f; // ���� �� ���� ����
    public float feetHeight = 1f; // �� ���� ���� ����
    public float checkHeight = 0.2f; // üũ ���� ���� ����

    [Header("�ݶ��̴� �浹 ó�� ����")]
    [Space(10f)]
    [SerializeField]

    private bool isGrounded; // ĳ���Ͱ� ���� ��� �ִ��� ���θ� �Ǵ��ϱ� ���� ����
    private Rigidbody rb;
    private float gravity;

    [SerializeField]
    private Animator anim;

    private bool isGoal = false;

    Transform camTransform;

    Vector3 moveDirection;
    private bool isWalk = false;

    private void Start()
    {
        gravity = -Physics.gravity.y;
        rb = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            // ĳ���Ͱ� ���� ��� �ִ��� �˻� �������� �����Ÿ� ǥ��
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }

        MoveCheck();
    }

    public void ToPose()//������ ����1�� 
    {
        if (isGoal) //��ǥ�� �� �����ߴٸ� �۵�. 
        {
            anim.SetTrigger("isPose");
        }
    }

    public void MoveCheck()
    {
        // Ű���� �Է��� �޾� �̵� �� ���� ����
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
           
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //���� ����
        if(moveDirection != Vector3.zero) //���� �ʱ�ȭ ���.
        {
            isWalk = true;
            anim.SetBool("RunaWalk", isWalk);
        }
        else
        {
            isWalk = false;
            anim.SetBool("RunaWalk", isWalk);
        }
            //ī�޶� ����� ������.
            Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y�� Quaternion ī�޶� �ڵ忡 ����ؼ� �����ش�.
            Vector3 movement = camRoty * moveDirection * moveSpeed;

            












            // ĳ������ ���� ó��.
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

                rb.velocity = movement + Vector3.up * jumpSpeed;
            }

            rb.velocity = movement + Vector3.up * rb.velocity.y;//ĳ���Ͱ� �߷¿� ������ ���� ������.


            //������ �������� ��� ������ ��ũx �������� ��Ҵٸ� �۵���.
            //������ �Ÿ��� ���� ������ ������,���� �Ÿ��� ������ 2�� ������ ����.
            Vector3 feetPos = transform.position + Vector3.down * feetHeight;
            Color color = Color.red;
            if (isGrounded) color = Color.green;
            Debug.DrawLine(feetPos, feetPos + Vector3.down * checkHeight, color);


    }
    
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Goal"))
        {
            isGoal = true; // ���⿡ �浹�Ǿ�����, true
            ToPose();
        }
    }
    public void OnTriggerExit(Collider col)
    {
        isGoal = false;
    }

}
