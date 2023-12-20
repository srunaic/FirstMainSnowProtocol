using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunaMoving : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도 조절 변수
    public float jumpHeight = 2f; // 점프 힘 조절 변수
    public float feetHeight = 1f; // 발 높이 설정 변수
    public float checkHeight = 0.2f; // 체크 범위 설정 변수

    [Header("콜라이더 충돌 처리 관리")]
    [Space(10f)]
    [SerializeField]

    private bool isGrounded; // 캐릭터가 땅에 닿아 있는지 여부를 판단하기 위한 변수
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
            // 캐릭터가 땅에 닿아 있는지 검사 레이저의 사정거리 표시
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }

        MoveCheck();
    }

    public void ToPose()//제어할 포즈1번 
    {
        if (isGoal) //목표한 골에 도착했다면 작동. 
        {
            anim.SetTrigger("isPose");
        }
    }

    public void MoveCheck()
    {
        // 키보드 입력을 받아 이동 및 점프 제어
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
           
        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //동작 패턴
        if(moveDirection != Vector3.zero) //벡터 초기화 방식.
        {
            isWalk = true;
            anim.SetBool("RunaWalk", isWalk);
        }
        else
        {
            isWalk = false;
            anim.SetBool("RunaWalk", isWalk);
        }
            //카메라 방향과 움직임.
            Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y축 Quaternion 카메라 코드에 계산해서 맞춰준다.
            Vector3 movement = camRoty * moveDirection * moveSpeed;

            












            // 캐릭터의 점프 처리.
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

                rb.velocity = movement + Vector3.up * jumpSpeed;
            }

            rb.velocity = movement + Vector3.up * rb.velocity.y;//캐릭터가 중력에 영향을 받음 점프함.


            //땅위에 레이저를 쏘고 감지함 태크x 레이저가 닿았다면 작동됨.
            //레이저 거리에 따라 점프가 됨으로,땅에 거리가 닿으면 2번 점프도 가능.
            Vector3 feetPos = transform.position + Vector3.down * feetHeight;
            Color color = Color.red;
            if (isGrounded) color = Color.green;
            Debug.DrawLine(feetPos, feetPos + Vector3.down * checkHeight, color);


    }
    
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Goal"))
        {
            isGoal = true; // 여기에 충돌되었을때, true
            ToPose();
        }
    }
    public void OnTriggerExit(Collider col)
    {
        isGoal = false;
    }

}
