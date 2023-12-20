using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    public GameObject Ex;//explosion 게임 오브젝트 생성.
  
    private float Count = 0; // 총알이 얼마나 부딪혔나

    void OnCollisionEnter(Collision col)//일반 충돌
    {
        if (col.gameObject.tag == "Missile" && Count == 0) //만약에 총알이 충돌 했을때,
        {
            Instantiate(Ex, transform.position, transform.rotation);
            Destroy(gameObject, 0.5f);

            ScoreTxt.Score += 1;//점수 1점
        }
     
    }
}
