using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWin : MonoBehaviour
{
    public GameObject Win;
    public GameObject sign;
    private float timer = 0f;

    void LateUpdate() 
    {
        if (ScoreTxt.Score == 4)
        {
            sign.SetActive(true);
            timer += Time.deltaTime;
            if (timer >= 2)
            {

                sign.SetActive(false);

            }
            
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && ScoreTxt.Score == 4)//점수 10점) //플레이어와 닿았다
        {
            Win.SetActive(true); //승리 텍스트 활성화.
        }
    }

}
