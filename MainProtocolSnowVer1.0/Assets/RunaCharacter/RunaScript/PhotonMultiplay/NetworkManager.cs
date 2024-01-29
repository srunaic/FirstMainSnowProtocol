using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SeonghyoGameManagerGroup; //���ӸŴ���

public class NetworkManager : MonoBehaviourPunCallbacks
{
    /*[Header("������ ������")]
    public GameObject ItemSpawn;*/

    public static NetworkManager Instance;

    [Header("DisconnectPanel")]
    public GameObject MainUI;
    public GameObject DisConnectPanel;
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;
    public PhotonView PV;

    [SerializeField]
    private GameObject ExitBtn;
    private int onClick = 0;

    [Header("ĳ���� ���� UI")]
    [SerializeField]
    private GameObject CreatePlayerBtn; //ĳ���� ���� ��ư �߰�
    [SerializeField]
    private GameObject CreatePlaye2rBtn; //ĳ���� ���� ��ư �߰�

    List<RoomInfo> myList = new List<RoomInfo>();//�ڷᱸ��.
    int currentPage = 1, maxPage, multiple;

    //���ӽ����� �ϸ�,ĳ���Ͱ� �����ǰ� ��Ʈ��ũ �����.
    public void LoadingScene()
    {
        CreatePlayerBtn.SetActive(true);
        CreatePlaye2rBtn.SetActive(true);
    }
    //�糪 ĳ���� ����.
    public void CreatePlayer()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameManager.instance._choicePlayer = ChoicePlayer.RunaPlayer;
            GameManager.instance.isConnect = true;//GameManager���� ��Ʈ��ũ�� ������ ������ ĳ���� �����ϵ���.
            RoomPanel.SetActive(false);
            CreatePlaye2rBtn.SetActive(false);
            StatusText.enabled = false;
            CreatePlayerBtn.SetActive(false);
        }
        else
        {
            GameManager.instance._choicePlayer = ChoicePlayer.NonePlayer;
        }
    }
    //ȭ�� ĳ���� ����.
    public void CreatePlayer2()
    {
        if (PhotonNetwork.IsConnected)
        {

            GameManager.instance._choicePlayer = ChoicePlayer.HwaYeonPlayer;
            GameManager.instance.isConnect = true;//GameManager���� ��Ʈ��ũ�� ������ ������ ĳ���� �����ϵ���.
            RoomPanel.SetActive(false);
            CreatePlaye2rBtn.SetActive(false);
            StatusText.enabled = false;
            CreatePlayerBtn.SetActive(false);
        }
        else
        {
            GameManager.instance._choicePlayer = ChoicePlayer.NonePlayer;
        }


    }
    public void OnChatRoom()
    {
        if (onClick <= 0) //�޽���
        {
              RoomPanel.SetActive(true);
              onClick = 1;
             
        }
        else if (onClick <= 1)
        {
           
              RoomPanel.SetActive(false);
              onClick = 0; //0���� �ʱ�ȭ.
        }
    }

    // ����ư -2 , ����ư -1 , �� ����
    public void MyListClick(int num) //�� ��ư OnClick
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal() //�ڵ忡�� ��ư ����.
    {
        // �ִ�������
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // ����, ������ư
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }


        Screen.SetResolution(1920, 1080, false); //���� �ػ� ����.
    }
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Send();
        }
        if (Input.GetButtonDown("Cancel")) 
        {
            OnChatRoom();
        }

        StatusText.text = PhotonNetwork.NetworkClientState.ToString(); //���� ���� ����.
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();//�����ϱ�

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() //�� ����.
    {
        DisConnectPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);

        //���� �г��� �Էµ� �� �޾ƿ���.
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;//������ ����������, �г����� ����.
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�"; //�г��� �Է�����.

        myList.Clear();
    }
    public void Disconnect()//��������� �߻�.
    {
        StatusText.enabled = true;//���� ���¹� �ٽ� ���� �ֵ��� ����
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        DisConnectPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
    }
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 4});

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        RoomPanel.SetActive(true);
        LobbyPanel.SetActive(false);
        RoomRenewal();
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    void RoomRenewal()//�� �����.
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = "���̸�:" + PhotonNetwork.CurrentRoom.Name +"       "+PhotonNetwork.CurrentRoom.PlayerCount + "��" + "       "+
             PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ��ο�";
    }

    public void Send() //ä�� �޽��� ������.
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text); //���� ���� ���̰� �ϴ� ����� �ϳ�.
        ChatInput.text = "";
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // ������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }

}