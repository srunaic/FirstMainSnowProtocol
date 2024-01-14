using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ShootingManager
{
    public class GameManager : MonoBehaviour
    {
        [Header("������ �Ŵ����� ���� �� �÷��̾� ���� �κ񿡼� �ҷ�����")]
        Text scoreText;

        static public int Score = 0;

        static public int myLastScore = 0; //���������
        static public int myBestScore = 0; //�ְ�����

        private bool Clicked = false;

        [Header("���� �ؽ�Ʈ ����� �� �гΰ���")]
        public BossShooting bossDie;//������ 

        [Header("�÷��̾� ������ �ǽð� ����.")]
        //static public int PlayerHpSave; //�÷��̾� ü�� �������.
        private float saveInterval = 5f;//�ǽð� ���� ����.
        public Transform PlayerTr;

        //�гΰ����� ���⼭ ������ �ϰ� bullet�̳� �ٸ����� �����ؾߵ�.

        public GameObject UltimateWeaponTxt;
        public GameObject LosePanel;
        public GameObject GameWinPnl;
        public GameObject ResultPnl;
        public Text ResultTxt;

        void Start()
        {
            scoreText
                = transform.Find("PlayPanel/ScoreText").GetComponent<Text>();
            //�ؿ� �ڽ����� ������ �ؽ�Ʈ �ҷ�����,.

            Score = 0;
            CurrentScore();
            LoadPlayer();
        }

        void Update()
        {
            //�ǽð����� �÷��̾� ���� ����.
            if (Time.time % saveInterval == 0)
            {
                SaveGame();
                Debug.Log("�ǽð� ���� ����" + saveInterval);
            }
            scoreText.text
                = "" + LobbyManager.PlayerName +
                "\n�ְ� ���:" + myBestScore +
                "\n�ֱ� ���:" + myLastScore +
                "\n���� ���:" + Score;
        }

        public void SaveGame()
        {
            //1.������ �����ؾߵɱ��?
            //2.������ ������ �������� �������ΰ�? ex)� ��Ȳ��
            //3.����Ȱ��� ��� �ҷ��ñ��?

            //�÷��̾� ��ġ�� ����.
            PlayerPrefs.SetFloat("LastPlayerLocateX", PlayerMove.InitialPosition.x);
            PlayerPrefs.SetFloat("LastPlayerLocateY", PlayerMove.InitialPosition.y);

            //�÷��̾� hp ����.
            //PlayerPrefs.SetInt("LastPlayerHP" + PlayerMove.MaxHealth,
            //PlayerHpSave); //Hp ������ �ǵ鿩��.

            myLastScore = Score;
            PlayerPrefs.SetInt("LastScore_" + LobbyManager.PlayerName,
                myLastScore);

            myBestScore
                = PlayerPrefs.GetInt("BestScore_" + LobbyManager.PlayerName);

            if (Score > myBestScore)//���������� �ְ��������� ũ�ٸ� �ְ�������.
            {
                myBestScore = Score;//��������
                PlayerPrefs.SetInt("BestScore_" + LobbyManager.PlayerName,
                    myBestScore);
            }
        }

        public void LoadPlayer()//���۽� ����� ������ �ҷ�����.
        {
            if (LobbyManager.Continuing)//�̾��ϱ� �غ� �Ǿ��ٸ� �ε� ����.
            {
                // �÷��̾� �ʱ� ��ġ �ҷ�����
                float initialPlayerPosX = PlayerPrefs.GetFloat("InitialPlayerLocateX");
                float initialPlayerPosY = PlayerPrefs.GetFloat("InitialPlayerLocateY");

                // �÷��̾� hp �ҷ�����
                //PlayerHpSave = PlayerPrefs.GetInt("LastPlayerHP" + PlayerMove.MaxHealth);
                // �÷��̾� �ʱ� ��ġ�� ����
                PlayerTr.position = new Vector2(initialPlayerPosX, initialPlayerPosY);

                Debug.Log("�÷��̾� ���� ��ġ �ҷ�����" + PlayerTr.position);
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
            // �� ��ŷ�� �÷��̾ �׾������� �����ϰ� bullet ��ũ��Ʈ�� ���� �ߴ�.
            //1.�÷��̾ ����Ҷ� ��ŷ.
            //2.�÷��̾��� ���� �ְ����� ����� ���;��Ѵ�.
            //3.���� �ְ������� ���� �ִ� �ְ������� ��  
        }

        public void ReTryGame()
        {
            if (!Clicked) //�Լ��� �־ ����.
            {
                Clicked = true;
                SceneManager.LoadScene("Stage_1");//�������� 1�ٽ� �÷���.
                Debug.Log("�Լ� ����.");
                Time.timeScale = 1.0f;//��������. TimeScale�� �׻� �ʱ�ȭ�� ������Ѵ�.
            }
            else
            {
                Clicked = false;
                Debug.Log("��ư ��Ȱ��ȭ");
            }
        }

        public void GoToLobby()
        {
            if (!Clicked) //�Լ��� �־ ����.
            {
                Clicked = true;
                SaveGame();//�κ�� �����͸� �ѹ� �̵��ϴ��� ����.
                SceneManager.LoadScene("Lobby");
                Debug.Log("�Լ� ����.");
            }
            else
            {
                Clicked = false;
                Debug.Log("��ư ��Ȱ��ȭ");
            }
        }
    }
}
