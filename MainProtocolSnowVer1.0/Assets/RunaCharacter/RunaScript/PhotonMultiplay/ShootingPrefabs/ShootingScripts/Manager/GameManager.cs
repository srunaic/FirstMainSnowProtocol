using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ShootingManager
{
    public class GameManager : MonoBehaviour
    {
        [Header("점수는 매니저에 저장 및 플레이어 정보 로비에서 불러오기")]
        Text scoreText;

        static public int Score = 0;

        static public int myLastScore = 0; //마지막기록
        static public int myBestScore = 0; //최고점수

        private bool Clicked = false;

        [Header("점수 텍스트 결과값 및 패널관리")]
        public BossShooting bossDie;//껍데기 

        [Header("플레이어 정보를 실시간 저장.")]
        //static public int PlayerHpSave; //플레이어 체력 저장관리.
        private float saveInterval = 5f;//실시간 저장 변수.
        public Transform PlayerTr;

        //패널관리는 여기서 무조건 하고 bullet이나 다른곳에 참조해야됨.

        public GameObject UltimateWeaponTxt;
        public GameObject LosePanel;
        public GameObject GameWinPnl;
        public GameObject ResultPnl;
        public Text ResultTxt;

        void Start()
        {
            scoreText
                = transform.Find("PlayPanel/ScoreText").GetComponent<Text>();
            //밑에 자식으로 정해진 텍스트 불러오기,.

            Score = 0;
            CurrentScore();
            LoadPlayer();
        }

        void Update()
        {
            //실시간으로 플레이어 정보 저장.
            if (Time.time % saveInterval == 0)
            {
                SaveGame();
                Debug.Log("실시간 저장 실행" + saveInterval);
            }
            scoreText.text
                = "" + LobbyManager.PlayerName +
                "\n최고 기록:" + myBestScore +
                "\n최근 기록:" + myLastScore +
                "\n현재 기록:" + Score;
        }

        public void SaveGame()
        {
            //1.무엇을 저장해야될까요?
            //2.저장의 기준을 무엇으로 잡을것인가? ex)어떤 상황에
            //3.저장된것을 어떨때 불러올까요?

            //플레이어 위치를 저장.
            PlayerPrefs.SetFloat("LastPlayerLocateX", PlayerMove.InitialPosition.x);
            PlayerPrefs.SetFloat("LastPlayerLocateY", PlayerMove.InitialPosition.y);

            //플레이어 hp 저장.
            //PlayerPrefs.SetInt("LastPlayerHP" + PlayerMove.MaxHealth,
            //PlayerHpSave); //Hp 변수만 건들여라.

            myLastScore = Score;
            PlayerPrefs.SetInt("LastScore_" + LobbyManager.PlayerName,
                myLastScore);

            myBestScore
                = PlayerPrefs.GetInt("BestScore_" + LobbyManager.PlayerName);

            if (Score > myBestScore)//현재점수가 최고점수보다 크다면 최고점수임.
            {
                myBestScore = Score;//현재점수
                PlayerPrefs.SetInt("BestScore_" + LobbyManager.PlayerName,
                    myBestScore);
            }
        }

        public void LoadPlayer()//시작시 저장된 데이터 불러오기.
        {
            if (LobbyManager.Continuing)//이어하기 준비가 되었다면 로딩 실행.
            {
                // 플레이어 초기 위치 불러오기
                float initialPlayerPosX = PlayerPrefs.GetFloat("InitialPlayerLocateX");
                float initialPlayerPosY = PlayerPrefs.GetFloat("InitialPlayerLocateY");

                // 플레이어 hp 불러오기
                //PlayerHpSave = PlayerPrefs.GetInt("LastPlayerHP" + PlayerMove.MaxHealth);
                // 플레이어 초기 위치로 설정
                PlayerTr.position = new Vector2(initialPlayerPosX, initialPlayerPosY);

                Debug.Log("플레이어 현재 위치 불러오기" + PlayerTr.position);
            }
        }
        public void CurrentScore()
        {
            myLastScore
             = PlayerPrefs.GetInt("LastScore_" + LobbyManager.PlayerName);

            myBestScore
             = PlayerPrefs.GetInt("BestScore_" + LobbyManager.PlayerName);
        }

        public void LoadRanking()
        {
            // 이 랭킹은 플레이어가 죽었을때라 간단하게 bullet 스크립트에 참조 했다.
            //1.플레이어가 사망할때 랭킹.
            //2.플레이어의 현재 최고점수 기록을 들고와야한다.
            //3.현재 최고점수와 전에 있던 최고점수값 비교  
        }

        public void ReTryGame()
        {
            if (!Clicked) //함수를 넣어서 관리.
            {
                Clicked = true;
                SceneManager.LoadScene("Stage_1");//스테이지 1다시 플레이.
                Debug.Log("함수 실행.");
                Time.timeScale = 1.0f;//정지해제. TimeScale은 항상 초기화를 해줘야한다.
            }
            else
            {
                Clicked = false;
                Debug.Log("버튼 비활성화");
            }
        }

        public void GoToLobby()
        {
            if (!Clicked) //함수를 넣어서 관리.
            {
                Clicked = true;
                SaveGame();//로비로 데이터를 한번 이동하더라도 저장.
                SceneManager.LoadScene("Lobby");
                Debug.Log("함수 실행.");
            }
            else
            {
                Clicked = false;
                Debug.Log("버튼 비활성화");
            }
        }
    }
}
