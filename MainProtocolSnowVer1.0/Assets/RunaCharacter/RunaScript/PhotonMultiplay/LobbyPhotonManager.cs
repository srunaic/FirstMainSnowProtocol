using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class LobbyPhotonManager : MonoBehaviourPunCallbacks
{
    [Header("���� ���� ���")]
    public readonly string gameversion = "v1.0"; // ���ӹ���   
    private string userId = "Choi";

    void Start()
    {    // ���ϴ� �ػ� ����
        Screen.SetResolution(1920, 1080, true);
        // ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���濡 ����");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode,string message)
    {
        Debug.Log("���� �� ���� ����");
        //�� ����
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 4;

        PhotonNetwork.CreateRoom("room_1",ro);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("�� �����Ϸ�");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� �Ϸ�");
        GameManager.instance.isConnect = true;
    }

}
