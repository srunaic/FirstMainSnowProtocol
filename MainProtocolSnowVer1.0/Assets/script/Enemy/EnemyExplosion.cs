using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject Ex;//explosion ���� ������Ʈ ����.
  
    private float Count = 0; // �Ѿ��� �󸶳� �ε�����

    void OnCollisionEnter(Collision col)//�Ϲ� �浹
    {
        if (col.gameObject.tag == "Missile" && Count == 0) //���࿡ �Ѿ��� �浹 ������,
        {
            Instantiate(Ex, transform.position, transform.rotation);
            Destroy(gameObject, 0.5f);

            ScoreTxt.Score += 1;//���� 1��
        }
     
    }
}
