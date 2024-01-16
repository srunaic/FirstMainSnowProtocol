using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI �����ϱ� ����
using UnityEngine.SceneManagement; //�� �̵��� ����
using ShootingManager;

public class LobbyManager : MonoBehaviour
{
    static public string PlayerName; //���� �̸�
    public bool hasSaveData = false; //���� �����Ͱ� �ִ���

    public InputField inputField_start; // ���� â�� �ִ� ��ǲ �ʵ�
    public InputField inputField_countinue; // �̾��ϱ� â�� �ִ� ��ǲ �ʵ�

    public Button continueButton; //�̾��ϱ� ��ư
    public Button resetButton; //������ �ʱ�ȭ ��ư
    static public bool Continuing = false; //�̾��ϱ� �غ� �Ǿ��°�?
   
    private bool CursorVisible = true;

    [SerializeField]
    private ShootingInterAct shotAct;

   
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
        resetButton.onClick.AddListener(ResetData);//���� ��ư ������ ������ ����.
    }
    void Start()
    {
        LoadData();
        UpdateUI();
    }

    private void Update()
    {
        Cursors();
    }

    private void Cursors()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            CursorVisible = !CursorVisible;
            Cursor.visible = CursorVisible;

            if (CursorVisible)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

    }

    //ó������ �����ϴ� �Լ�
    public void StartNewGame()
    {
        if (string.IsNullOrEmpty(inputField_start.text))
            Debug.Log("�̸��� �Է����ּ���.");
        else
        {
            SaveData();
            UpdateUI();

            Debug.Log("������ �����մϴ�.");
            shotAct.MainShotGame.SetActive(true);
            shotAct.MainShotCam.SetActive(true);
        }
    }
    public void ResetData()
    {
        ClearData();
        UpdateUI();
    }

    void LoadData() //������ �ҷ���
    {
        if (PlayerPrefs.HasKey("LastPlayer"))
        {
            PlayerName = PlayerPrefs.GetString("LastPlayer");
            Time.timeScale = 1.0f;
            hasSaveData = true;//������ �Ǿ��°� ��
        }
        else
        {
            hasSaveData = false;//�ƴϿ�.
        }
    }

    public void ContinueGame()
    {
        if (hasSaveData)//������ ������ �Ϸ� �Ǿ��ٸ� 
        {
            Continuing = true;

            if(Continuing)//�̾��ϱ� �غ�Ϸ�.
            {   
                LoadData(); // ����� ������ �ҷ�����
                Debug.Log("�̾��ϱ�: " + PlayerName);
                SceneManager.LoadScene("Stage_1"); // ���⿡ �̾��� �� �̸��� ��������.

                Time.timeScale = 1.0f;//�Ͻ����� ����.
            }
        }
        else
        {
            Debug.Log("����� �����Ͱ� �����ϴ�.");
        }
    }

    void ClearData() //�ҷ��� ������ �ʱ�ȭ(����)
    {
        PlayerPrefs.DeleteKey("LastPlayer");//��ư�� ������ �̸� ���������� ����ڴ�.
        PlayerName = "";

        hasSaveData = false;
    }
    void SaveData()
    {
        PlayerName = inputField_start.text;
        PlayerPrefs.SetString("LastPlayer", PlayerName);

        hasSaveData = true;
    }

    void UpdateUI() //�ҷ��� �����͸� UI�� ����
    {
        if (hasSaveData)
        {
            inputField_start.text = PlayerName;//�̸� �Է��ϰ� ����
            //���۽ÿ� �̸��� �����ϰ� Ȯ���� �����ٸ�,
            inputField_countinue.text = PlayerName;

            continueButton.interactable = true;//�̾��ϱ� ��ư Ȱ��ȭ.
            resetButton.interactable = true;//���¹�ư Ȱ��ȭ.
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
        shotAct.MainShotGame.SetActive(false);
        shotAct.MainShotCam.SetActive(false);
    }
}
