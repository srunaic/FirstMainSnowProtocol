using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SeonghyoGameManagerGroup; //���ӸŴ���

public class ScoreItem : MonoBehaviourPunCallbacks, iItem
{    
    public void Use(MultiPlayer player) //�ּ� �Ѹ� �̻��� ������ �����ߴٸ�,
    {
        if(GameManager.instance.isConnect == true)//��Ƽ�϶�,����.
        {
            player.pv.RPC("AddScore", RpcTarget.All, 1);
            Destroy(gameObject);
        }
        else
        {//�����϶�,
            player.AddScore(1);
            Destroy(gameObject);
        }
    }

}

