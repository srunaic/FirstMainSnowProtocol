using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShootingInterAct : MonoBehaviourPunCallbacks
{
    public Transform ShotPos;
    public GameObject ShootCanvas;
    //public GameObject MainShotGame;
    //public GameObject MainShotCam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ShootCanvas.SetActive(false);
        }
    }

    public void SetShotGame(MultiPlayer _Shotplayer)
    {
        if (_Shotplayer._checkstate == CheckState.ShotGames)
        {
            _Shotplayer.onMoveable = false;
            ShootCanvas.SetActive(true);

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
