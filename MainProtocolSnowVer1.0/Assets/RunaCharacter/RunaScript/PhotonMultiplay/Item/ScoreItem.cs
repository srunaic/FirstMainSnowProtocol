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
            Destroy(gameObject);
            Debug.Log("�����߰� �� ������ ����"+ player);
        }
        else//�����϶��� ����Ƕ�.
        {
            if(player != null)
            {
                Destroy(gameObject);
            }
        }
    }

}
