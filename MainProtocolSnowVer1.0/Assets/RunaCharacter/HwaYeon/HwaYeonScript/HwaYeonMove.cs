using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeonghyoGameManagerGroup;
using Photon.Pun;

public enum CheckHwaYeonState //포톤 상의 플레이어 상태값.
{
    None,
    Sitting,
    DollGames,
    ShotGames
}
public class HwaYeonMove : MonoBehaviour,IPunObservable
{
    [Header("플레이어의 현재 상태줄표시")]
    public CheckHwaYeonState _checkstate = CheckHwaYeonState.None;

    [Header("인터페이스 상호작용")]
    public MulltiSeat nearSeat;
    public ShootingInterAct nearShooting;

    [Header("포톤 Text 정보 들고오기")]
    public Text PlayerTxt; //포톤에 넘겨받을 플레이어 닉네임 정보.

    [Header("포톤에서 실행되는 캐릭터")]
    public PhotonView pv;
    private Transform _PlayerTr;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

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
    public float VelocityY = 22.0f;

    [Header("캐릭터 애니메이션 관리")]
    public Animator HwaAnim;

    [Header("캐릭터 애니메이션 bool값 저장")]
    public bool isMove = false;
    public bool OnSit = false;

    [Header("평상시 걷기 및 달리기")]
    private float RunSpeed = 4f;
    private float BaseSpeed = 2f;

    public bool onMoveable = true;
    private int onClick = 0;

    public NetworkManager netManager;

    void Start()
    {
        netManager = FindObjectOfType<NetworkManager>();
        gravity = -Physics.gravity.y;
        _checkstate = CheckHwaYeonState.None;
        rb = GetComponent<Rigidbody>();
        HwaAnim = GetComponent<Animator>();

        pv = GetComponent<PhotonView>();
        _PlayerTr = GetComponent <Transform>();

        if(pv.IsMine && GameManager.instance.isConnect == true)
        {
            PhotonNetwork.LocalPlayer.NickName = netManager.NickNameInput.text; //플레이어 2는 Instance화 된 매니저에서 직접 참조형.
            PlayerTxt.text = PhotonNetwork.LocalPlayer.NickName;//로컬 닉네임 으로 변환.

            Camera.main.GetComponent<FollowCam>().SetPlayer(transform);
            nearSeat = FindObjectOfType<MulltiSeat>();
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            // 캐릭터가 땅에 닿아 있는지 검사
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }

    }
    public void Update()
    {
        if (pv.IsMine)
        {
            ProcessPlayerMovement();


            if (!isGrounded)
            {
                if (rb.velocity.y > 0)
                {
                    Physics.gravity = new Vector3(0, -VelocityY * 10f, 0);
                }

            }

        }
      
        if (Input.GetKeyDown(KeyCode.C))
        {
            _checkstate = CheckHwaYeonState.None;

            if (_checkstate == CheckHwaYeonState.None)
            {
                onMoveable = true;
            }
        }
  
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_PlayerTr.position);
            stream.SendNext(_PlayerTr.rotation);
            stream.SendNext(PhotonNetwork.LocalPlayer.NickName); // 닉네임을 전송
            stream.SendNext(HwaAnim.GetBool("isRun"));//bool값 애니메이션의 전송방법.
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            PlayerTxt.text = (string)stream.ReceiveNext(); // 받은 닉네임을 텍스트로 업데이트
            HwaAnim.SetBool("isRun", (bool)stream.ReceiveNext());
        }
    }

    [PunRPC]
    void TriggerJumpAnimation()
    {
        HwaAnim.SetTrigger("Jump");
    }

    [PunRPC]
    public void SetSit()//앉기
    {
        if (nearSeat != null)
        {
            onMoveable = false;

            transform.rotation = nearSeat.sitPos.rotation;
            Debug.Log("의자 동기화" + nearSeat);
            HwaAnim.SetTrigger("Sit");

            OnSit = true;
        }
    }

    [PunRPC]
    public void OffSit()//앉는키 해제
    {
        if (OnSit)
        {
            HwaAnim.SetTrigger("Stend");

            onMoveable = true;
            OnSit = false;
            //애니메이션 추가.

        }
    }
    void ProcessPlayerMovement()
    {
        if (onMoveable)
        {
            Move(); // 움직임 처리.   
        }
        if (_checkstate == CheckHwaYeonState.Sitting)
        {
            if (nearSeat != null && Input.GetKeyDown(KeyCode.Z))//앉을때 동작.
            {
                nearSeat.HwaYeonSeat(this);
            }
        }
        else if (OnSit && Input.GetKeyDown(KeyCode.C))//일어날때 동작.
        {
            _checkstate = CheckHwaYeonState.None;

            if (_checkstate == CheckHwaYeonState.None)
            {
                nearSeat.HwaYeonOffSeat(this);
            }
        }
        else if (nearShooting != null && _checkstate == CheckHwaYeonState.ShotGames)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                nearShooting.SetShotGameHwaYeon(this);
            }
        }
        else if (Input.GetButtonDown("Cancel")) //네트워크 환경과 교류 접속 상태일때, 이 키일때 적용되는 값 채팅 연동.
        {
            if (onClick <= 0) //메시지
            {
                onClick = 1;
                onMoveable = false;

            }
            else if (onClick <= 1)
            {
                onMoveable = true;
                onClick = 0; //0으로 초기화.
            }
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        //멀티플레이 중이고 마스터가 아니면 
        if (GameManager.instance.isConnect == true//게임 접속하기 상태라면,,
            && !PhotonNetwork.IsMasterClient)
            return; //아래코드 스킵

        else if (other.TryGetComponent<MulltiSeat>(out MulltiSeat findSeat))
        {
            _checkstate = CheckHwaYeonState.Sitting;
            nearSeat = findSeat;
        }
        else if (other.TryGetComponent<ShootingInterAct>(out ShootingInterAct findShot))
        {
            nearShooting = findShot;
            if (nearShooting != null)
            {
                _checkstate = CheckHwaYeonState.ShotGames;
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DollGaming")) //인형뽑기 기계
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _checkstate = CheckHwaYeonState.DollGames;
            }
            else if (_checkstate == CheckHwaYeonState.DollGames)
            {
                onMoveable = false;
            }
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

        // 캐릭터의 점프 처리 달리기 상태일때도 이 키가 눌리면 작동 되도록.
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            pv.RPC("TriggerJumpAnimation", RpcTarget.All);//점프 애니메이션 포톤에 넘겨주기.
            float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

            rb.velocity = movement + Vector3.down * jumpSpeed * 5f;
        }
        else
            isGrounded = false;

    }

  
}
