using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeonghyoGameManagerGroup;
using Photon.Pun;

public enum CheckHwaYeonState //���� ���� �÷��̾� ���°�.
{
    None,
    Sitting,
    DollGames,
    ShotGames
}
public class HwaYeonMove : MonoBehaviour,IPunObservable
{
    [Header("�÷��̾��� ���� ������ǥ��")]
    public CheckHwaYeonState _checkstate = CheckHwaYeonState.None;

    [Header("�������̽� ��ȣ�ۿ�")]
    public MulltiSeat nearSeat;
    public ShootingInterAct nearShooting;

    [Header("���� Text ���� ������")]
    public Text PlayerTxt; //���濡 �Ѱܹ��� �÷��̾� �г��� ����.

    [Header("���濡�� ����Ǵ� ĳ����")]
    public PhotonView pv;
    private Transform _PlayerTr;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

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
    public float VelocityY = 22.0f;

    [Header("ĳ���� �ִϸ��̼� ����")]
    public Animator HwaAnim;

    [Header("ĳ���� �ִϸ��̼� bool�� ����")]
    public bool isMove = false;
    public bool OnSit = false;

    [Header("���� �ȱ� �� �޸���")]
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
            PhotonNetwork.LocalPlayer.NickName = netManager.NickNameInput.text; //�÷��̾� 2�� Instanceȭ �� �Ŵ������� ���� ������.
            PlayerTxt.text = PhotonNetwork.LocalPlayer.NickName;//���� �г��� ���� ��ȯ.

            Camera.main.GetComponent<FollowCam>().SetPlayer(transform);
            nearSeat = FindObjectOfType<MulltiSeat>();
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            // ĳ���Ͱ� ���� ��� �ִ��� �˻�
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
            stream.SendNext(PhotonNetwork.LocalPlayer.NickName); // �г����� ����
            stream.SendNext(HwaAnim.GetBool("isRun"));//bool�� �ִϸ��̼��� ���۹��.
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            PlayerTxt.text = (string)stream.ReceiveNext(); // ���� �г����� �ؽ�Ʈ�� ������Ʈ
            HwaAnim.SetBool("isRun", (bool)stream.ReceiveNext());
        }
    }

    [PunRPC]
    void TriggerJumpAnimation()
    {
        HwaAnim.SetTrigger("Jump");
    }

    [PunRPC]
    public void SetSit()//�ɱ�
    {
        if (nearSeat != null)
        {
            onMoveable = false;

            transform.rotation = nearSeat.sitPos.rotation;
            Debug.Log("���� ����ȭ" + nearSeat);
            HwaAnim.SetTrigger("Sit");

            OnSit = true;
        }
    }

    [PunRPC]
    public void OffSit()//�ɴ�Ű ����
    {
        if (OnSit)
        {
            HwaAnim.SetTrigger("Stend");

            onMoveable = true;
            OnSit = false;
            //�ִϸ��̼� �߰�.

        }
    }
    void ProcessPlayerMovement()
    {
        if (onMoveable)
        {
            Move(); // ������ ó��.   
        }
        if (_checkstate == CheckHwaYeonState.Sitting)
        {
            if (nearSeat != null && Input.GetKeyDown(KeyCode.Z))//������ ����.
            {
                nearSeat.HwaYeonSeat(this);
            }
        }
        else if (OnSit && Input.GetKeyDown(KeyCode.C))//�Ͼ�� ����.
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
        else if (Input.GetButtonDown("Cancel")) //��Ʈ��ũ ȯ��� ���� ���� �����϶�, �� Ű�϶� ����Ǵ� �� ä�� ����.
        {
            if (onClick <= 0) //�޽���
            {
                onClick = 1;
                onMoveable = false;

            }
            else if (onClick <= 1)
            {
                onMoveable = true;
                onClick = 0; //0���� �ʱ�ȭ.
            }
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        //��Ƽ�÷��� ���̰� �����Ͱ� �ƴϸ� 
        if (GameManager.instance.isConnect == true//���� �����ϱ� ���¶��,,
            && !PhotonNetwork.IsMasterClient)
            return; //�Ʒ��ڵ� ��ŵ

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
        if (other.CompareTag("DollGaming")) //�����̱� ���
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

        // ĳ������ ���� ó�� �޸��� �����϶��� �� Ű�� ������ �۵� �ǵ���.
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            pv.RPC("TriggerJumpAnimation", RpcTarget.All);//���� �ִϸ��̼� ���濡 �Ѱ��ֱ�.
            float jumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);

            rb.velocity = movement + Vector3.down * jumpSpeed * 5f;
        }
        else
            isGrounded = false;

    }

  
}
