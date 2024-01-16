using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeonghyoGameManagerGroup; //게임매니저

public enum CheckState //포톤 상의 플레이어 상태값.
{
    None,
    ChatState,
    Sitting,
    DollGames,
    ShotGames
}
public class MultiPlayer : MonoBehaviour, IPunObservable
{
    [Header("플레이어의 현재 상태줄표시")]
    public CheckState _checkstate = CheckState.None;

    [Header("인터페이스 상호작용")]
    public int score = 0;//인터페이스 플레이어가 받을 점수.
    public Text ScoreText;
    public MulltiSeat nearSeat;
    public ShootingInterAct nearShooting;

    [Header("포톤 Text 정보 들고오기")]
    public Text PlayerTxt;
    public NetworkManager networkManger;

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
    public Animator RunaAnim;

    [Header("캐릭터 애니메이션 bool값 저장")]
    public bool isMove = false;
    public bool OnSit = false;

    private float RunSpeed = 4f;
    private float BaseSpeed = 2f;

    [Header("포톤에서 실행되는 캐릭터")]
    public PhotonView pv;
    private Transform _PlayerTr;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    public bool onMoveable = true;
    private int onClick = 0;

    void Start()
    {
        ScoreText.text = ""+ score;//점수 초기화.

        _checkstate = CheckState.None;

        gravity = -Physics.gravity.y;
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        _PlayerTr = GetComponent<Transform>();
        RunaAnim = GetComponent<Animator>();

        networkManger = FindObjectOfType<NetworkManager>();

        //게임 시작할때,플레이어 카메라 위치 정보를 동기화 해줌.
        if (pv.IsMine && GameManager.instance.isConnect == true)
        {
            PhotonNetwork.LocalPlayer.NickName = networkManger.NickNameInput.text; //포톤 UI에 입력한 정보 닉네임 받아오기.
            PlayerTxt.text = PhotonNetwork.LocalPlayer.NickName;

            Camera.main.GetComponent<FollowCam>().SetPlayer(transform);
            nearSeat = FindObjectOfType<MulltiSeat>();
        }
    }
    private void FixedUpdate()
    {
        if(rb.velocity.y < 0)
        {
            // 캐릭터가 땅에 닿아 있는지 검사
            isGrounded = Physics.Raycast(transform.position, Vector3.down, feetHeight + checkHeight);
        }
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            ProcessPlayerMovement();

        }
 
        if (Input.GetKeyDown(KeyCode.C))
        {
            _checkstate = CheckState.None;

            if (_checkstate == CheckState.None)
            {
                 onMoveable = true;
            }  
        }
    }

    void ProcessPlayerMovement()
    {
        if (onMoveable)
        {
            Move(); // 움직임 처리.   
        }
        else
        {
            rb.velocity = Vector3.zero + Vector3.up * VelocityY;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pv.RPC("TriggerDanceAnimation", RpcTarget.All);//Trigger 애니메이션을 포톤 값에 넘겨주는방법.
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            pv.RPC("TriggerHiAnimation", RpcTarget.All);
        }
        if (_checkstate == CheckState.Sitting)
        {
            if (nearSeat != null && Input.GetKeyDown(KeyCode.Z))//앉을때 동작.
            {

                nearSeat.SetSeat(this);
            }
        }
        else if (OnSit && Input.GetKeyDown(KeyCode.C))//일어날때 동작.
        {
            _checkstate = CheckState.None;

            if (_checkstate == CheckState.None)
            {
                nearSeat.OffSeat(this);
            }
        }
       else if (nearShooting != null && _checkstate == CheckState.ShotGames)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                nearShooting.SetShotGame(this);
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
    //포톤에서 유저의 정보를 이 함수로 전달해줌. 그래야 상대측도 보인다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //원격 전송방식.
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_PlayerTr.position);
            stream.SendNext(_PlayerTr.rotation);
            stream.SendNext(PhotonNetwork.LocalPlayer.NickName); // 닉네임을 전송
            stream.SendNext(RunaAnim.GetBool("isRun"));//bool값 애니메이션의 전송방법.
           // stream.SendNext(score);
        }
        /*else if(stream.IsReading)
        {
            score = (int)stream.ReceiveNext();//점수도 네트워크에 보내겠다.
            ScoreText.text = "" + score;
        }*/
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            PlayerTxt.text = (string)stream.ReceiveNext(); // 받은 닉네임을 텍스트로 업데이트
            RunaAnim.SetBool("isRun", (bool)stream.ReceiveNext());
        }
    }

    //인터페이스 ScoreItem 에 쓰일 함수.
    [PunRPC]
    public void AddScore(int num)
    {
        score += num;
        ScoreText.text = "" + score;
    }

    [PunRPC] //원격 전송 시스템.
    void TriggerDanceAnimation()
    {
        StartCoroutine(CheckPose());
    }

    [PunRPC]
    void TriggerJumpAnimation()
    {
        RunaAnim.SetTrigger("Jump");
    }
    [PunRPC]
    void TriggerHiAnimation()
    {
        RunaAnim.SetTrigger("Hi");
    }

    [PunRPC]
    public void SetSit()//앉기
    {
        if (nearSeat != null)
        {
            onMoveable = false;

            transform.rotation = nearSeat.sitPos.rotation;
            Debug.Log("의자 동기화" + nearSeat);
            RunaAnim.SetTrigger("Sit");

            OnSit = true;
        }
    }

    [PunRPC]
    public void OffSit()//앉는키 해제
    {
        if (OnSit)
        {
            RunaAnim.SetTrigger("Stend");

            onMoveable = true;
            OnSit = false;
            //애니메이션 추가.

        }
    }

    //인터페이스 적용 아이템 참조.
    public void OnTriggerEnter(Collider other)
    {
        //멀티플레이 중이고 마스터가 아니면 
        if (GameManager.instance.isConnect == true //게임 접속하기 상태라면,,
            && !PhotonNetwork.IsMasterClient)
            return; //아래코드 스킵

        if (other.TryGetComponent<iItem>(out iItem item))
        {
            item.Use(this);
            //Debug.Log("이 아이템 오브젝트 사라짐"+item);
            //멀티플레이 중이고 마스터가 아니면   
        }

        else if (other.TryGetComponent<MulltiSeat>(out MulltiSeat findSeat))
        {
            _checkstate = CheckState.Sitting;
            nearSeat = findSeat;
        }
        else if (other.TryGetComponent<ShootingInterAct>(out ShootingInterAct findShot))
        {
            nearShooting = findShot;
            if(nearShooting != null) 
            {
                _checkstate = CheckState.ShotGames;
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DollGaming")) //인형뽑기 기계
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _checkstate = CheckState.DollGames;
            }
            else if (_checkstate == CheckState.DollGames)
            {
                onMoveable = false;
            } 
        }
    }


    public IEnumerator CheckPose() //춤 스피드
    {
        yield return new WaitForSeconds(1f);
        RunaAnim.SetTrigger("isPose");

        yield return new WaitForSeconds(6.7f);
    }

    public void Move()
    {
        // 키보드 입력을 받아 이동 및 점프 제어
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        /*// 뒤로 가는 것을 방지
        if (verticalInput < 0)
        {
            verticalInput = 0;
        }*/

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

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Quaternion rotMove = Quaternion.LookRotation(moveDirection); // 이동 방향으로 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, rotMove, rotLookSpeed * Time.deltaTime);

        }
        // 캐릭터의 점프 처리
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            pv.RPC("TriggerJumpAnimation", RpcTarget.All);//점프 애니메이션 포톤에 넘겨주기.
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
                RunaAnim.SetTrigger("highLanding");
                Physics.gravity = new Vector3(0, VelocityY, 0);//물체 중력 제어 계산

                //이 코드에 따라 무중력과 중력으로 바뀜.
            }
        }

    }

}
