using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MulltiSeat : MonoBehaviourPunCallbacks
{
    public Transform sitPos;

    public void SetSeat(MultiPlayer player) //첫번째 캐릭터 선택시 
    {
        //의자에 앉을 때의 위치를 동기화
        if (player.OnSit == false)
        {
            player.pv.RPC("SetSit", RpcTarget.All);
        }
        else
        {
            player.SetSit();
        }

    }

    public void HwaYeonSeat(HwaYeonMove _player2)//두번째 캐릭터 선택시 
    {
        if (_player2.OnSit == false)
        {
            _player2.pv.RPC("SetSit", RpcTarget.All);
        }
        else
        {
            _player2.SetSit();
        }
    }

    public void OffSeat(MultiPlayer _player)
    {
    if (_player.OnSit == true)
    {
        _player.pv.RPC("OffSit", RpcTarget.All);
    }
    else
    {
        _player.OffSit();
    }
    }
    public void HwaYeonOffSeat(HwaYeonMove _player2)
    {
        if (_player2.OnSit == true)
        {
            _player2.pv.RPC("OffSit", RpcTarget.All);
        }
        else
        {
            _player2.OffSit();
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 의자의 위치와 회전 정보를 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 의자의 위치와 회전 정보를 수신
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
