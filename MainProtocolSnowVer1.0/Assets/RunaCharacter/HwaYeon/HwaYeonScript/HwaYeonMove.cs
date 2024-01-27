using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeonghyoGameManagerGroup;

public enum CheckHwaYeonState //포톤 상의 플레이어 상태값.
{
    None,
    ChatState,
    Sitting,
    DollGames,
    ShotGames
}
public class HwaYeonMove : MonoBehaviour
{
    [Header("플레이어의 현재 상태줄표시")]
    public CheckHwaYeonState _checkstate = CheckHwaYeonState.None;

    [Header("싱글 비동기적 움직임관리")]
    public float moveSpeed = 20f; // 이동 속도 조절 변수
    public float jumpHeight = 2f; // 점프 힘 조절 변수
    public float feetHeight = 1f; // 발 높이 설정 변수
    public float checkHeight = 0.4f; // 체크 범위 설정 변수
    public float rotateSpeed = 100f;

    public float rotLookSpeed = 0.1f;//회전속도

    [Header("땅 처리 부분")]
    [SerializeField]
    private bool isGrounded; // 캐릭터가 땅에 닿아 있는지 여부를 판단하기 위한 변수
    public bool onGround;

    [Header("중력 가속도")]
    private Rigidbody rb;
    private float gravity;
    public float VelocityY = -22.0f;

    [Header("캐릭터 애니메이션 관리")]
    public Animator HwaAnim;

    [Header("캐릭터 애니메이션 bool값 저장")]
    public bool isMove = false;
    public bool OnSit = false;

    [Header("평상시 걷기 및 달리기")]
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
            // 캐릭터가 땅에 닿아 있는지 검사
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }
    }
    private void Update()
    {
        if (onMoveable)
        {
            Move(); // 움직임 처리.   
        }
        else
           rb.velocity = Vector3.zero + Vector3.up * VelocityY;

        if (isGrounded)
        {
            SlopeGrounded();
        }
    }

    //경사면에서의 움직임 제어
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
        // 키보드 입력을 받아 이동 및 점프 제어
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //기본 움직임 값.
        Vector3 MovePlayer = new Vector3(horizontalInput, verticalInput);

        Vector3 cameraForward = Camera.main.transform.forward;//카메라 방향 앞으로
        cameraForward.y = 0f; // y 축 방향은 고려하지 않음

        Vector3 moveDirection = (cameraForward * verticalInput + Camera.main.transform.right * horizontalInput).normalized;
        //캠이 보는 방향과 플레이어 방향을 초기화 해주는 방정식. 캠방향 앞쪽이 Right임

        Vector3 movement = moveDirection * moveSpeed; //기본속력

        //애니메이션 실행.
        if (moveDirection != Vector3.zero) //벡터 초기화 방식.
        {
            isMove = true;
            if (isMove)
            {
                HwaAnim.SetFloat("MoveDirX", MovePlayer.x);
                HwaAnim.SetFloat("MoveDirY", MovePlayer.y);
            }

        }
        //처음 움직임 초기화 시킴. 레프트 쉬프트 누를시에 달림.
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

        if (horizontalInput != 0 || verticalInput != 0)//움직일 때,
        {
            //Quaternion rotMove = Quaternion.LookRotation(moveDirection); // 이동 방향으로 회전
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);

            Quaternion rotMove = Quaternion.LookRotation(moveDirection);//카메라 회전방향으로 초기화.
            transform.rotation = rotMove;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);//서서히 바라보도록.*/

        }


        // 캐릭터의 점프 처리
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            HwaAnim.SetTrigger("Jump");
            float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

            rb.velocity = movement + Vector3.up * jumpSpeed * 1f;
            isGrounded = false;
        }
        rb.velocity = movement + Vector3.up * rb.velocity.y;

        //땅이 아닐때,
        if (!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                Physics.gravity = new Vector3(0, VelocityY, 0);//물체 중력 제어 계산

                //이 코드에 따라 무중력과 중력으로 바뀜.
            }
        }

    }


}
