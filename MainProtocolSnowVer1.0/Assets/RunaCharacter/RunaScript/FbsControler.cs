using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
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
    public bool onGround;
    
    [Header("중력 가속도")]
    private Rigidbody rb;
    private float gravity;
    public float VelocityY = -22.0f;

    Transform camTransform;

    [Header("캐릭터 애니메이션 관리")]

    public Animator RunaAnim;

    [Header("캐릭터 애니메이션 bool값 저장")]
    public bool onTarget = false;//타겟이 있을때, 없을때 구현.
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
            // 캐릭터가 땅에 닿아 있는지 검사
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }
    }
    private void Update()
    {
        if (onMoveable)//움직이는 상태인가?.
        {
            Move(); //움직임.
        }
        else
            rb.velocity = Vector3.zero + Vector3.up * VelocityY;

        //경사각인데 플레이어가 땅위를 밟고 있다면 실행.
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
    IEnumerator CheckPose() //춤 스피드
    {
        yield return new WaitForSeconds(1f);
        onMoveable = false;
        RunaAnim.SetTrigger("isPose");
        yield return new WaitForSeconds(6.7f);
        onMoveable = true;
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
        //캠이 보는 방향과 플레이어 방향을 초기화 해주는 방정식.

        Vector3 movement = moveDirection * moveSpeed; //기본속력

        //애니메이션 실행.
        if (moveDirection != Vector3.zero) //벡터 초기화 방식.
        {
            isMove = true;
            if (isMove)
            {
                RunaAnim.SetFloat("InputX", MovePlayer.x);
                RunaAnim.SetFloat("InputY", MovePlayer.y);
            }
            /*//캐릭터 회전관리 카메라가 보는 방향으로
            Quaternion rotNext = Quaternion.LookRotation(MovePlayer);

            Quaternion camFront
                = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
            transform.rotation
                = Quaternion.Lerp(transform.rotation, camFront,
                 Time.deltaTime * rotateSpeed);*/
        }

        //처음 움직임 초기화 시킴. 레프트 쉬프트 누를시에 달림.
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
        //캐릭터와 카메라가 붙어 있다면 이렇게 관리
        //Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y축 Quaternion 카메라 코드에 계산해서 맞춰준다.
        //Vector3 movement = camRoty * moveDirection * moveSpeed;

        if (horizontalInput != 0 || verticalInput != 0)//키를 입력 받았을때,
        {
            Quaternion rotMove = Quaternion.LookRotation(moveDirection); //방향회전 바라보도록 하기.
            transform.rotation = rotMove;

            transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);//서서히 바라보도록.
        }

        // 캐릭터의 점프 처리
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            RunaAnim.SetTrigger("Jump");
            float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

            rb.velocity = movement + Vector3.up * jumpSpeed * 1f;
            isGrounded = false;
        }
        rb.velocity = movement + Vector3.up * rb.velocity.y;

        //땅이 아닐때,
        if(!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                RunaAnim.SetTrigger("highLanding");
                Physics.gravity = new Vector3(0, VelocityY, 0);//물체 중력 제어 계산
     
                //이 코드에 따라 무중력과 중력으로 바뀜.
            }
        }
        /*
        // 만약 땅이 아니라면 착지가 빨리 되도록
        if (!isGrounded)
        {
            int MaxHeight = 50; //최대치 높이 제한
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
                    Debug.Log("고도 높이 낮아짐" + LimitHeight);
                    Vector3 _flyVec2 = Vector3.down + transform.position.normalized;
                    rb.velocity = _flyVec2 * DownSpeed;
                    Physics.gravity = new Vector3(0, -2f, 0);
                    Quaternion FlyingMove = Quaternion.Euler(0f, _flyVec2.y, 0f);
                }

            }
        
            // 추가: 캐릭터가 땅이 아닌 경우, 수직 속도를 빠르게 0으로 조절
            if (rb.velocity.y > 0)
            {
                RunaAnim.SetTrigger("highLanding");
                Physics.gravity = new Vector3(0, -22.0f, 0);//중력 제어
                //이 코드에 따라 무중력과 중력으로 바뀜.
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
}
