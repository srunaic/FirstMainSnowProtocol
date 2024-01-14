using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShootingInterAct : MonoBehaviourPunCallbacks
{
    public Transform ShotPos;

    public void SetShotGame(MultiPlayer _Shotplayer)
    {
        if(_Shotplayer._checkstate == CheckState.ShotGames)
        {
            LoadingScene.LoadScene("ShootingLobby");
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ������ ��ġ�� ȸ�� ������ ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // ������ ��ġ�� ȸ�� ������ ����
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
