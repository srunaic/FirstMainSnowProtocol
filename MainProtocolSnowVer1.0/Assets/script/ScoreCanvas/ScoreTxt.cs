using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTxt : MonoBehaviour
{
    public static int Score = 0;//static 변수 정수 Score 0으로 초기화.
    public GameObject scoreText;//점수 텍스트 오브젝트 생성

  
    void Update()
    {
        string scoreString = "4/"+ Score +"격추 되었음";//점수
        scoreText.GetComponent<Text>().text = scoreString;//텍스트 On
    }
}
