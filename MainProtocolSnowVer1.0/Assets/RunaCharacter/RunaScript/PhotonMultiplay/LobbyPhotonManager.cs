using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class LobbyPhotonManager : MonoBehaviourPunCallbacks
{
    [Header("포톤 접속 방식")]
    public readonly string gameversion = "v1.0"; // 게임버전   
    private string userId = "Choi";

    void Start()
    {    // 원하는 해상도 설정
        Screen.SetResolution(1920, 1080, true);
        // 설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤에 접속");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode,string message)
    {
        Debug.Log("랜덤 룸 접속 실패");
        //룸 생성
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 4;

        PhotonNetwork.CreateRoom("room_1",ro);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성완료");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        GameManager.instance.isConnect = true;
    }

}
