using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public enum KindBuilding
{
  NoneBuild,
  GameBuild
}
public enum GameJoy 
{
  None,
  DollGame,
  SongGame,
  ShootingGame
}
public class InterManager : MonoBehaviour
{
    [Header("게임기 종류")]
    public KindBuilding _ContactBuild;
    public GameJoy _JoyGame;

    public FbsControler player;
    public Seat nearSeat;//가까이에 있는 의자를 접근.
    public LiftMoving DollGameObj;//인형게임기에 접근을 했다면,
    public BuildingInterActive _building;

    [Header("페이드 인 아웃")]
    [SerializeField]
    private FaidInOut FadeInOut;
   
    [Header("인형뽑기 UI 시간초 관리")]
    public TextMeshProUGUI _TimeTxt;
    [SerializeField]
    private float TimeGames;

    void Start()
    {
          TimeGames = 0;
        _TimeTxt.text = "0:20";//시간 초

        _ContactBuild = KindBuilding.NoneBuild;
        _JoyGame = GameJoy.None;//쓰는 게임기 종류 없음

        player = FindObjectOfType<FbsControler>();
        FadeInOut = FindObjectOfType<FaidInOut>();
        _building = FindObjectOfType<BuildingInterActive>();
    }

    void FixedUpdate()
    {   //인형뽑기 취소 캔슬
        if (_JoyGame != GameJoy.None && Input.GetKeyDown(KeyCode.C))
        {
            _JoyGame = GameJoy.None;
            player.onMoveable = true;
            DollGameObj.DollGameCam.SetActive(false);//인형뽑기 캠 비활성화.
        }

        DollGameTimes();

    }
    void Update()
    {
        if (nearSeat != null && Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(SetSit());
        }
        else if (player.OnSit && Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(OffSit());
        }
        else if (_JoyGame == GameJoy.None && DollGameObj !=null
            && Input.GetKeyDown(KeyCode.Z))
        {
            _JoyGame = GameJoy.DollGame;//사용중인 게임기 변환.
           StartCoroutine(DollGame());
        }
    }

    IEnumerator SetSit()//앉기
    {
        player.onMoveable = false;

        transform.rotation = nearSeat.sitPos.rotation;
        player.RunaAnim.SetTrigger("Sit");

        yield return new WaitForSeconds(0.5f);
        player.OnSit = true;

    }
    IEnumerator OffSit()//앉는키 해제
    {
        player.RunaAnim.ResetTrigger("Sit");
        player.RunaAnim.SetTrigger("Stend");

        yield return new WaitForSeconds(1f);
        player.onMoveable = true;
        player.OnSit = false;
        //애니메이션 추가.
    }
    //게임 연출
    IEnumerator DollGame()
    {
        player.onMoveable = false;
        transform.position = DollGameObj.LiftPos.position;

        if(_JoyGame == GameJoy.DollGame)//인형뽑기 기계 활성화.
        {
            yield return new WaitForSeconds(1f);
            FadeInOut.Fade();
            DollGameObj.DollGameCam.SetActive(true);//인형뽑기 캠 활성화.
        }
    }
    //게임 시간 관리
    void DollGameTimes()
    {
        if (_JoyGame == GameJoy.DollGame)//인형뽑기 기계 활성화.
        {
            float timeIncrement = 1.0f; //시간에 대입
            TimeGames += timeIncrement * Time.deltaTime;

            int roundedTime = Mathf.RoundToInt(TimeGames);//1초 분의 1 만큼 시간 흐름.
            _TimeTxt.text = roundedTime + ":20";
        }
        if (TimeGames >= 20)
        {
            TimeGames = 0;
            StartCoroutine(NoneGame(2f));
            DollGameObj.DollGameCam.SetActive(false);
            player.onMoveable = true;
        }
 
    }

    //상호작용 객체 감지.
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Seat>(out Seat findSeat))
        {
            nearSeat = findSeat;
        }
        else if (other.TryGetComponent<LiftMoving>(out LiftMoving findDollGame))
        {
            DollGameObj = findDollGame;
            Debug.Log("게임기랑 접촉" + findDollGame);
        }
        else if (other.TryGetComponent<BuildingInterActive>(out BuildingInterActive _buildActive))
        {
            _building = _buildActive;
            Debug.Log("빌딩이랑 접촉" + _buildActive);

            _ContactBuild = KindBuilding.GameBuild;
            if(_ContactBuild == KindBuilding.GameBuild) 
            {
                _building.BuildAnim.SetTrigger("RightDoor");
                _building.BuildAnim.SetTrigger("LeftDoor");
                _building.col.isTrigger = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Seat>(out Seat findSeat))
        {
            if (nearSeat == findSeat)
            {
               nearSeat = null;
            }
        }
        else if (other.TryGetComponent<LiftMoving>(out LiftMoving findDollGame))
        {
            if (DollGameObj = findDollGame)
            {
                DollGameObj = null;
            }
        }
        //빌딩이랑 상호작용.
        else if (other.TryGetComponent<BuildingInterActive>(out BuildingInterActive _buildActive))
        {
            StartCoroutine(DoorAnim(2f)); //DistanceDoor 닫히는 타이밍과 속도값.
            _ContactBuild = KindBuilding.NoneBuild;
        }
    }
    IEnumerator NoneGame(float RateGameTime)//게임초기화.
    {
        _JoyGame = GameJoy.None;
        yield return new WaitForSeconds(RateGameTime);
    
    }
    //문이 열리는 타이밍과 충돌체의 타이밍 리스트
    IEnumerator DoorAnim(float DisatanceDoor)
    {
         if (_ContactBuild == KindBuilding.NoneBuild)
         {
                yield return new WaitForSeconds(DisatanceDoor);
                _building.BuildAnim.SetTrigger("CloseRightDoor");
                _building.BuildAnim.SetTrigger("CloseLeftDoor");
                _building.col.isTrigger = false;

         }
        
    }
}
