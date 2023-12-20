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
                Debug.Log("�Ͻ����� �����Դϴ�.");
                IsPause = true;
                return;
            }


            if (IsPause == true)
            {
                Time.timeScale = 1;
                Debug.Log("�Ͻ����� ����");
                IsPause = false;
                return;

            }
        }
    }
}
