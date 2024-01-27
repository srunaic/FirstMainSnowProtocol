using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MulltiSeat : MonoBehaviourPunCallbacks
{
    public Transform sitPos;

    public void SetSeat(MultiPlayer player) //ù��° ĳ���� ���ý� 
    {
        //���ڿ� ���� ���� ��ġ�� ����ȭ
        if (player.OnSit == false)
        {
            player.pv.RPC("SetSit", RpcTarget.All);
        }
        else
        {
            player.SetSit();
        }

    }

    public void HwaYeonSeat(HwaYeonMove _player2)//�ι�° ĳ���� ���ý� 
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
