using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeSquare : MonoBehaviour
{
    private bool IsPause;
    private GameObject GameUIStarted;

     void Start()
    {
        IsPause = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPause == false)
            {
                Time.timeScale = 0;
                Debug.Log("일시정지 상태입니다.");
                IsPause = true;
                return;
            }


            if (IsPause == true)
            {
                Time.timeScale = 1;
                Debug.Log("일시정지 해제");
                IsPause = false;
                return;

            }
        }
    }
}
