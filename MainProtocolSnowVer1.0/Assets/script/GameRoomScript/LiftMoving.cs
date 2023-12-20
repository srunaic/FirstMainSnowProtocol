using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GamePanel
{
   NoneGame,
   GameLiftChannel,//타입 1번 
   ShootingGame
}
public class LiftMoving : MonoBehaviour
{
    //1.게임 시작할 조건
    //2.게임 시작시 게임 모드 진입
    //3.게임 시작후 
    //움직이는 막대기와 통합 해줘야됨 근데 다른 그룹이고 따로 동작해야 함으로 통합불가.
    //일단 오른쪽 이동만 구현.

    [Header("게임 리프트를 담을 오브젝트")]
    public GameObject _LiftObj;//담을 게임기.
   
    //애니메이터
    [SerializeField]
    private Animator LiftAnim;
    private Rigidbody _rbLift;

    [Header("게임 패널 관리")]
    public GamePanel _LiftPanel;
    private GamePanel _gamePanel;

    [Header("움직임 스피드 관리.")]
    private float speed = 3f;
    public Vector3 LiftVec;
    private void Awake()
    {
        _gamePanel = GamePanel.NoneGame;
    }
    private void Start()
    {
        LiftVec = Vector3.zero;//리프트 벡터 초기화.
        _rbLift = GetComponent<Rigidbody>();
        LiftAnim = GetComponent<Animator>();
    }

    void Update()
    {
        MoveLiftCase();
    }
    private void MoveLiftCase()
    {
        //애니메이션 작동중이 아니라면 게임 시작
        //2.게임 진입 패널 필요.

        LiftVec = Vector3.left * speed;
        LiftVec = Vector3.right * speed;
        LiftVec = Vector3.forward * speed;
        LiftVec = Vector3.back * speed;
    }
}
