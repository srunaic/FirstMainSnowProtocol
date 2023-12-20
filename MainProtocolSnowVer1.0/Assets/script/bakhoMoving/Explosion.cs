using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject User;
    public GameObject explosion;//explosion 게임 오브젝트 생성.
    public GameObject Fair; //패배

    private float Count = 0;

     void OnCollisionEnter(Collision col)//일반 충돌
    {
        if (col.gameObject.tag == "Fire" && Count == 0) //만약에 총알이 충돌 했을때,
        {
       
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject, 1f);//3.0 초 지나면 파괴

                User.GetComponent<bakhochanMove>().enabled = false; //캐릭터가 죽으면 스크립트도 꺼짐. 
                Fair.SetActive(true); //승리 텍스트 활성화.


        }
     }

    
}
