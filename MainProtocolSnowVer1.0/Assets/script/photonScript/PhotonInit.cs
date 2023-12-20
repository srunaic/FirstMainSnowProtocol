using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public string gameVersion = "3.30";
    public string nickName = "choi";


    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        OnLogin();
        CreateRoom();
    }


    void Update()
    {
        
    }

    void OnLogin() 
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = this.gameVersion;
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster() 
    {

        Debug.Log("connected!!!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        this.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Join Us");
        PhotonNetwork.Instantiate("backhostIdle",new Vector3(0,0,0),Quaternion.identity);
    }

    void CreateRoom() 
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
}
