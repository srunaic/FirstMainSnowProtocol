using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI 제어하기 위함
using UnityEngine.SceneManagement; //씬 이동을 위함
using ShootingManager;
using Photon.Pun;

public class LobbyManager : MonoBehaviour
{
    static public string PlayerName; //유저 이름
    public bool hasSaveData = false; //저장 데이터가 있는지

    public InputField inputField_start; // 시작 창에 있는 인풋 필드
    public InputField inputField_countinue; // 이어하기 창에 있는 인풋 필드

    public Button continueButton; //이어하기 버튼
    public Button resetButton; //데이터 초기화 버튼
    static public bool Continuing = false; //이어하기 준비가 되었는가?
   
    private bool CursorVisible = true;

    [SerializeField]
    private ShootingInterAct shotAct;

    [SerializeField]
    private GameObject StartPanel;

    public GameObject shootingGamesPrefab; // shootingGame 프리팹을 저장할 변수
    private GameObject shootingGamesInstance; // shootingGame 인스턴스를 저장할 변수

    private void Awake()
    {
        shotAct = FindObjectOfType<ShootingInterAct>();

        inputField_start
            = transform.Find("StartPanel/Box/InputField").GetComponent<InputField>();
        inputField_countinue
            = transform.Find("ContinuePanel/Box/InputField").GetComponent<InputField>();

        continueButton = transform.Find("MenuBox/Continue").GetComponent<Button>();
        resetButton = transform.Find("MenuBox/DeleteData").GetComponent<Button>();
      
        continueButton.onClick.AddListener(ContinueGame);
        resetButton.onClick.AddListener(ResetData);//리셋 버튼 누르면 데이터 삭제.
    }
    void Start()
    {
        LoadData();
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            DestroyShootingGames();
            StartPanel.SetActive(false);
        }
    }
    //슈팅 게임 처음부터 시작하는 함수
    public void StartNewGame(MultiPlayer _Shotplayer)
    {
        if (string.IsNullOrEmpty(inputField_start.text))
            Debug.Log("이름을 입력해주세요.");
        else
        {
            SaveData();
            UpdateUI();

            Debug.Log("게임을 시작합니다.");

            CreateShootingGames();
        }
    }

    private void CreateShootingGames()
    {
       
         shootingGamesInstance = Instantiate(shootingGamesPrefab, Vector3.zero, Quaternion.identity);
        
    }

    private void RecreateShootingGames()
    {
        // 기존 인스턴스가 있다면 제거
        if (shootingGamesInstance != null)
        {
            Destroy(shootingGamesInstance);
            Debug.Log("Delete shooting games" + shootingGamesInstance);
        }

        // 리소스에서 프리팹을 로드하고 GameObject에 할당하여 재생성
        shootingGamesPrefab = Resources.Load<GameObject>("Resources/ShootingGame/ShootingGame 1"); // "Path_To_shootingGame_Prefab"에 실제 프리팹 경로를 넣어주세요.
        CreateShootingGames();
    }

    public void DestroyShootingGames()
    {
        if (shootingGamesInstance != null)
        {
            Destroy(shootingGamesInstance);
            Debug.Log("Delete shooting games" + shootingGamesInstance);
        }
    }

    public void ResetData()
    {
        ClearData();
        UpdateUI();
    }

    void LoadData() //데이터 불러옴
    {
        if (PlayerPrefs.HasKey("LastPlayer"))
        {
            PlayerName = PlayerPrefs.GetString("LastPlayer");
            Time.timeScale = 1.0f;
            hasSaveData = true;//저장이 되었는가 예
        }
        else
        {
            hasSaveData = false;//아니오.
        }
    }

    public void ContinueGame()
    {
        if (hasSaveData)//데이터 저장이 완료 되었다면 
        {
            Continuing = true;

            if(Continuing)//이어하기 준비완료.
            {   
                LoadData(); // 저장된 데이터 불러오기
                Debug.Log("이어하기: " + PlayerName);
                SceneManager.LoadScene("Stage_1"); // 여기에 이어질 씬 이름을 넣으세요.

                Time.timeScale = 1.0f;//일시정지 해제.
            }
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
        }
    }

    void ClearData() //불러온 데이터 초기화(리셋)
    {
        PlayerPrefs.DeleteKey("LastPlayer");//버튼을 누르면 이름 저장정보도 지우겠다.
        PlayerName = "";

        hasSaveData = false;
    }
    void SaveData()
    {
        PlayerName = inputField_start.text;
        PlayerPrefs.SetString("LastPlayer", PlayerName);

        hasSaveData = true;
    }
    void UpdateUI() //불러온 데이터를 UI에 적용
    {
        if (hasSaveData)
        {
            inputField_start.text = PlayerName;//이름 입력하고 게임
            //시작시에 이름을 저장하고 확인을 눌렀다면,
            inputField_countinue.text = PlayerName;

            continueButton.interactable = true;//이어하기 버튼 활성화.
            resetButton.interactable = true;//리셋버튼 활성화.
        }
        else
        {
            inputField_start.text = "";
            inputField_countinue.text = "";

            continueButton.interactable = false;
            resetButton.interactable = false;
        }
    }
    public void ExitBtn()
    {
        //shotAct.MainShotGame.SetActive(false);
        //shotAct.MainShotCam.SetActive(false);
    }
}
