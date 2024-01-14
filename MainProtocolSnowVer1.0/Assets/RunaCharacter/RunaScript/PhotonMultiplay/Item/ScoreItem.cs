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
            Destroy(gameObject);
            Debug.Log("점수추가 및 아이템 삭제"+ player);
        }
        else//로컬일때도 적용되라.
        {
            if(player != null)
            {
                Destroy(gameObject);
            }
        }
    }

}
