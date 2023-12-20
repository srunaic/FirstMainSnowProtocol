using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GamePanel
{
   NoneGame,
   GameLiftChannel,//Ÿ�� 1�� 
   ShootingGame
}
public class LiftMoving : MonoBehaviour
{
    //1.���� ������ ����
    //2.���� ���۽� ���� ��� ����
    //3.���� ������ 
    //�����̴� ������ ���� ����ߵ� �ٵ� �ٸ� �׷��̰� ���� �����ؾ� ������ ���պҰ�.
    //�ϴ� ������ �̵��� ����.

    [Header("���� ����Ʈ�� ���� ������Ʈ")]
    public GameObject _LiftObj;//���� ���ӱ�.
   
    //�ִϸ�����
    [SerializeField]
    private Animator LiftAnim;
    private Rigidbody _rbLift;

    [Header("���� �г� ����")]
    public GamePanel _LiftPanel;
    private GamePanel _gamePanel;

    [Header("������ ���ǵ� ����.")]
    private float speed = 3f;
    public Vector3 LiftVec;
    private void Awake()
    {
        _gamePanel = GamePanel.NoneGame;
    }
    private void Start()
    {
        LiftVec = Vector3.zero;//����Ʈ ���� �ʱ�ȭ.
        _rbLift = GetComponent<Rigidbody>();
        LiftAnim = GetComponent<Animator>();
    }

    void Update()
    {
        MoveLiftCase();
    }
    private void MoveLiftCase()
    {
        //�ִϸ��̼� �۵����� �ƴ϶�� ���� ����
        //2.���� ���� �г� �ʿ�.

        LiftVec = Vector3.left * speed;
        LiftVec = Vector3.right * speed;
        LiftVec = Vector3.forward * speed;
        LiftVec = Vector3.back * speed;
    }
}
