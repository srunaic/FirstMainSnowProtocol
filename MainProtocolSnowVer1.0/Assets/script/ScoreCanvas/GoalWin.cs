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
        if (collision.gameObject.tag == "Player" && ScoreTxt.Score == 4)//���� 10��) //�÷��̾�� ��Ҵ�
        {
            Win.SetActive(true); //�¸� �ؽ�Ʈ Ȱ��ȭ.
        }
    }

}
