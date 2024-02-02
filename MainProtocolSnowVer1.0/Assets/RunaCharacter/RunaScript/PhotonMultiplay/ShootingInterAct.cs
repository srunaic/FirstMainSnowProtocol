using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShootingInterAct : MonoBehaviourPunCallbacks
{
    public Transform ShotPos;
    public GameObject ShootCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ShootCanvas.SetActive(false);
        }
    }

    public void SetShotGame(MultiPlayer _Shotplayer) //this�� ����.
    {
        if (_Shotplayer._checkstate == CheckState.ShotGames)
        {
            _Shotplayer.onMoveable = false;
            ShootCanvas.SetActive(true);

        }
    }
    public void SetShotGameHwaYeon(HwaYeonMove _Shotplayer2)
    {
        if (_Shotplayer2._checkstate == CheckHwaYeonState.ShotGames)
        {
            _Shotplayer2.onMoveable = false;
            ShootCanvas.SetActive(true);

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
