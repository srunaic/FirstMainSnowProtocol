using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTxt : MonoBehaviour
{
    public static int Score = 0;//static ���� ���� Score 0���� �ʱ�ȭ.
    public GameObject scoreText;//���� �ؽ�Ʈ ������Ʈ ����

  
    void Update()
    {
        string scoreString = "4/"+ Score +"���� �Ǿ���";//����
        scoreText.GetComponent<Text>().text = scoreString;//�ؽ�Ʈ On
    }
}
