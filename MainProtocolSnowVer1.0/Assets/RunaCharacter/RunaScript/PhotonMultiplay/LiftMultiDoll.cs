using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AudioSeonghyo;
public enum GameKinded
{
    None,
    DollGame
}
public class LiftMultiDoll : MonoBehaviourPunCallbacks
{
    public GameKinded _gameKind = GameKinded.None;

    [Header("�����̱� UI �ð��� ����")]
    public TextMeshProUGUI _TimeTxt;
    public float TimeGames;

    //������� ������ ����
    [Header("���� �̱� �÷��̾� ķ��ġ.")]
    public GameObject DollGameCam;//���� ķ ��ġ.

    [Header("���� ������ ��ġ")]
    public Transform _LegDollPos;//�� ��ü�� �ڽĵ��� �ѹ��� �����̱�.
    public Transform _LegDollPos2;
    [Header("���� ��ġ")]
    public Transform _Doll;
    public Transform ZilePos; //������ �ö� ��ġ

    [Header("������ ���ǵ� ����.")]
    public float speed = 0.10f;
    public float Downspeed = 0.08f;

    public Vector3 LiftVec;

    [Header("���̵� �� �ƿ�")]
    [SerializeField]
    private FaidInOut FadeInOut;//���θ����̸��.

    private PhotonView pv;

    public AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        pv = GetComponent<PhotonView>();
         TimeGames = 0;//�����̱� �ð� �ʱ�ȭ.
        _TimeTxt.text = "0:20";//�ð� ��
    }

    private void Update()
    {
        //�����̱� ��� ĵ��
        if (Input.GetKeyDown(KeyCode.C))
        {
            _gameKind = GameKinded.None;
            _audioManager.MainSound();
            _audioManager.audioGroup.Stop();
            DollGameCam.SetActive(false);
        }
        else if (_gameKind == GameKinded.DollGame)
        {
            DollGameTimes();
       }
    }

    //���� �ð� ����
    void DollGameTimes()
    {
        if (_gameKind == GameKinded.DollGame)//�����̱� ��� Ȱ��ȭ.
        {
            float timeIncrement = 1.0f; //�ð��� ����
            TimeGames += timeIncrement * Time.deltaTime;

            int roundedTime = Mathf.RoundToInt(TimeGames);//1�� ���� 1 ��ŭ �ð� �帧.
            _TimeTxt.text = roundedTime + ":20";
        }
        if (TimeGames >= 20)
        {
            TimeGames = 0;
            StartCoroutine(NoneGame(1f));
        }
    }
    public void _DollGame() //�÷��̾ ���� ������,
    {
            if (Input.GetKey(KeyCode.R))
            {
                _LegDollPos2.Translate(Vector3.forward * Downspeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.T))
            {
                _LegDollPos2.Translate(Vector3.back * Downspeed * Time.deltaTime);
            }

            else if (Input.GetKey(KeyCode.D))
            {
                _LegDollPos.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _LegDollPos.Translate(Vector3.right * speed * Time.deltaTime);
            }

            else if (Input.GetKey(KeyCode.W))
            {
                _LegDollPos2.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _LegDollPos2.Translate(Vector3.right * speed * Time.deltaTime);
            }

    }

    private void OnTriggerStay(Collider _playerpos)
    {
        if (_playerpos.CompareTag("Player"))//�÷��̾�� �����ߴٸ�,
        {
            if (Input.GetKey(KeyCode.Z))
            {
                StartCoroutine(_DollGameFadeTiming());
                _gameKind = GameKinded.DollGame;   //����ؼ� true�� 
            }
            else if (_gameKind == GameKinded.DollGame)
            {
                _DollGame();
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameKind = GameKinded.None;
        }
    }
    IEnumerator _DollGameFadeTiming()
    {
        yield return new WaitForSeconds(1f);
        FadeInOut.Fade();
        DollGameCam.SetActive(true);//�����̱� ķ Ȱ��ȭ.
    }
    //�����̱� �ð���
    IEnumerator NoneGame(float RateGameTime)//�����ʱ�ȭ.
    {
        _gameKind = GameKinded.None; //���� ���� ����.
        yield return new WaitForSeconds(RateGameTime);
    }

}
