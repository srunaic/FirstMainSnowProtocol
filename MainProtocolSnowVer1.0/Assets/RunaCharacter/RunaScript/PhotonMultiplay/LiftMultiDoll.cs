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

    [Header("인형뽑기 UI 시간초 관리")]
    public TextMeshProUGUI _TimeTxt;
    public float TimeGames;

    //오락기기 움직임 관리
    [Header("인형 뽑기 플레이어 캠위치.")]
    public GameObject DollGameCam;//게임 캠 위치.

    [Header("집게 포지션 위치")]
    public Transform _LegDollPos;//이 객체와 자식들을 한번에 움직이기.
    public Transform _LegDollPos2;
    [Header("인형 위치")]
    public Transform _Doll;
    public Transform ZilePos; //인형이 올라갈 위치

    [Header("움직임 스피드 관리.")]
    public float speed = 0.10f;
    public float Downspeed = 0.08f;

    public Vector3 LiftVec;

    [Header("페이드 인 아웃")]
    [SerializeField]
    private FaidInOut FadeInOut;//본인만보이면됨.

    private PhotonView pv;

    public AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        pv = GetComponent<PhotonView>();
         TimeGames = 0;//인형뽑기 시간 초기화.
        _TimeTxt.text = "0:20";//시간 초
    }

    private void Update()
    {
        //인형뽑기 취소 캔슬
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

    //게임 시간 관리
    void DollGameTimes()
    {
        if (_gameKind == GameKinded.DollGame)//인형뽑기 기계 활성화.
        {
            float timeIncrement = 1.0f; //시간에 대입
            TimeGames += timeIncrement * Time.deltaTime;

            int roundedTime = Mathf.RoundToInt(TimeGames);//1초 분의 1 만큼 시간 흐름.
            _TimeTxt.text = roundedTime + ":20";
        }
        if (TimeGames >= 20)
        {
            TimeGames = 0;
            StartCoroutine(NoneGame(1f));
        }
    }
    public void _DollGame() //플레이어가 접근 했을때,
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
        if (_playerpos.CompareTag("Player"))//플레이어와 접근했다면,
        {
            if (Input.GetKey(KeyCode.Z))
            {
                StartCoroutine(_DollGameFadeTiming());
                _gameKind = GameKinded.DollGame;   //계속해서 true가 
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
        DollGameCam.SetActive(true);//인형뽑기 캠 활성화.
    }
    //인형뽑기 시간초
    IEnumerator NoneGame(float RateGameTime)//게임초기화.
    {
        _gameKind = GameKinded.None; //게임 정지 상태.
        yield return new WaitForSeconds(RateGameTime);
    }

}
