using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SeonghyoGameManagerGroup; //게임매니저

public class ScoreItem : MonoBehaviourPunCallbacks, iItem
{    
    public void Use(MultiPlayer player) //최소 한명 이상의 유저와 접촉했다면,
    {
        if(GameManager.instance.isConnect == true)//멀티일때,연결.
        {
            player.pv.RPC("AddScore", RpcTarget.All, 1);
            Destroy(gameObject);
        }
        else
        {//로컬일때,
            player.AddScore(1);
            Destroy(gameObject);
        }
    }

}

