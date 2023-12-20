using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbsControler : MonoBehaviour
{
    [Header("움직임관리")]
    public float moveSpeed = 20f; // 이동 속도 조절 변수
    public float jumpHeight = 2f; // 점프 힘 조절 변수
    public float feetHeight = 1f; // 발 높이 설정 변수
    public float checkHeight = 0.4f; // 체크 범위 설정 변수
    public float rotateSpeed = 100f;
   
    public float rotLookSpeed = 3f;//회전속도

    [Header("땅 처리 부분")]
    [SerializeField]
    private bool isGrounded; // 캐릭터가 땅에 닿아 있는지 여부를 판단하기 위한 변수
    private Rigidbody rb;
    private float gravity;

    Transform camTransform;

    [Header("캐릭터 애니메이션 관리")]
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
        // 키보드 입력을 받아 이동 및 점프 제어
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;//카메라 방향 앞으로
        cameraForward.y = 0f; // y 축 방향은 고려하지 않음

        Vector3 moveDirection = (cameraForward * verticalInput + Camera.main.transform.right * horizontalInput).normalized;
        //캠이 보는 방향과 플레이어 방향을 초기화 해주는 방정식.

        Vector3 movement = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero) //벡터 초기화 방식.
        {
            isWalk = true;
            RunaAnim.SetBool("RunaWalk", isWalk);
        }
        else
        {
            isWalk = false;
            RunaAnim.SetBool("RunaWalk", isWalk);
        }
        //Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y축 Quaternion 카메라 코드에 계산해서 맞춰준다.
        //Vector3 movement = camRoty * moveDirection * moveSpeed;

        if (horizontalInput !=0 || verticalInput != 0)//키를 입력 받았을때,
        {
            Quaternion rotMove = Quaternion.LookRotation(moveDirection); //방향회전 바라보도록 하기.
            transform.rotation = rotMove;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);//서서히 바라보도록.
        }
       
        // 캐릭터의 점프 처리
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
        /*// 쿼터니온 회전각 계산방법
         Quaternion rotY = Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0);//y축을 기준으로 회전한다.
         transform.rotation = rotY * transform.rotation; //현재 회전속도에서 쿼터니온 값을 곱해줌.*/

        /*
        transform.RotateAround(Vector3.zero,Vector3.up, rotateSpeed * Time.deltaTime);
        //카메라와 회전각 정해주기.
        */

        /*float InputKey = 0;
        if(Input.GetKey(KeyCode.Q))
            InputKey = -1f;
        if (Input.GetKey(KeyCode.E))
            InputKey = 1f;*/

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime);//Vector3.up은 new Vector3(0,y축,0)
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            // 캐릭터가 땅에 닿아 있는지 검사
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }
    }
}
