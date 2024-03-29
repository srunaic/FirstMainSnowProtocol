using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SeonghyoGameManagerGroup; //게임매니저

public class NetworkManager : MonoBehaviourPunCallbacks
{
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

    [Header("캐릭터 생성 UI")]
    [SerializeField]
    private GameObject CreatePlayerBtn; //캐릭터 생성 버튼 추가
    [SerializeField]
    private GameObject CreatePlaye2rBtn; //캐릭터 생성 버튼 추가
    [SerializeField]
    private GameObject CreatePlaye3rBtn; //캐릭터 생성 버튼 추가

    List<RoomInfo> myList = new List<RoomInfo>();//방 만들기 자료구조.
    int currentPage = 1, maxPage, multiple;

    //게임시작을 하면,캐릭터가 생성되고 네트워크 연결됨.
    public void LoadingScene()
    {
        CreatePlayerBtn.SetActive(true);
        CreatePlaye2rBtn.SetActive(true);
        CreatePlaye3rBtn.SetActive(true);
    }
    //루나 캐릭터 생성.
    public void CreatePlayer()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameManager.instance._choicePlayer = ChoicePlayer.RunaPlayer;
            GameManager.instance.isConnect = true;//GameManager에서 네트워크의 연결을 받으면 캐릭터 생성하도록.
            RoomPanel.SetActive(false);
            CreatePlaye2rBtn.SetActive(false);
            StatusText.enabled = false;
            CreatePlayerBtn.SetActive(false);
            CreatePlaye3rBtn.SetActive(false);
        }
        else
        {
            GameManager.instance._choicePlayer = ChoicePlayer.NonePlayer;
        }
    }
    //화연 캐릭터 생성.
    public void CreatePlayer2()
    {
        if (PhotonNetwork.IsConnected)
        {

            GameManager.instance._choicePlayer = ChoicePlayer.HwaYeonPlayer;
            GameManager.instance.isConnect = true;//GameManager에서 네트워크의 연결을 받으면 캐릭터 생성하도록.
            RoomPanel.SetActive(false);
            CreatePlaye2rBtn.SetActive(false);
            StatusText.enabled = false;
            CreatePlayerBtn.SetActive(false);
            CreatePlaye3rBtn.SetActive(false);
        }
        else
        {
            GameManager.instance._choicePlayer = ChoicePlayer.NonePlayer;
        }

    }

    public void CreatePlayer3()
    {
        if (PhotonNetwork.IsConnected)
        {

            GameManager.instance._choicePlayer = ChoicePlayer.Runaria;
            GameManager.instance.isConnect = true;//GameManager에서 네트워크의 연결을 받으면 캐릭터 생성하도록.
            RoomPanel.SetActive(false);
            CreatePlaye2rBtn.SetActive(false);
            StatusText.enabled = false;
            CreatePlayerBtn.SetActive(false);
            CreatePlaye3rBtn.SetActive(false);
        }
        else
        {
            GameManager.instance._choicePlayer = ChoicePlayer.NonePlayer;
        }

    }
    public void OnChatRoom()
    {
        if (onClick <= 0) //메시지
        {
              RoomPanel.SetActive(true);
              onClick = 1;
             
        }
        else if (onClick <= 1)
        {
           
              RoomPanel.SetActive(false);
              onClick = 0; //0으로 초기화.
        }
    }

    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num) //방 버튼 OnClick
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name); // 방 참가.
        MyListRenewal();

        Debug.Log("방생성" + name);
    }

    void MyListRenewal() //코드에서 버튼 제어.
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
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
        Screen.SetResolution(1920, 1080, false); //최초 해상도 설정.
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

        StatusText.text = PhotonNetwork.NetworkClientState.ToString(); //현재 접속 상태.
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();//접속하기

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() //방 생성.
    {
        DisConnectPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);

        //포톤 닉네임 입력된 값 받아오기.
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;//게임을 참여했을때, 닉네임을 들고옴.
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다"; //닉네임 입력했음.

        myList.Clear();
    }
    public void Disconnect()//접속종료시 발생.
    {
        StatusText.enabled = true;//현재 상태바 다시 볼수 있도록 세팅
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
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    void RoomRenewal()//방 재생성.
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = "방이름:" + PhotonNetwork.CurrentRoom.Name +"       "+PhotonNetwork.CurrentRoom.PlayerCount + "명" + "       "+
             PhotonNetwork.CurrentRoom.MaxPlayers + "최대인원";
    }

    public void Send() //채팅 메시지 보내기.
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text); //상대방 한테 보이게 하는 방법중 하나.
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
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
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }

}