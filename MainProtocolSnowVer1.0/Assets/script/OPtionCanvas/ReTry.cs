using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReTry : MonoBehaviour
{
    public GameObject FairCamera;
    public GameObject CameraArm;
    public GameObject Enemy;
    public GameObject PressSign;
    public GameObject WinPos;
    public GameObject Fair;

    private float timer = 0;
  
    void LateUpdate()
    { 
        
        if (WinPos.activeSelf == true)
        {
            PressSign.SetActive(true);

        }
        else if (Input.GetKeyDown(KeyCode.Z) && PressSign.activeSelf == true) 
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            ScoreTxt.Score = 0; //점수도 다시 초기화 해줘라.
        }

        if (Fair.activeSelf == true)//실패 시 점수 초기화.
        {
            timer += Time.deltaTime;
            if (timer >= 0.9f)
            {
                PressSign.SetActive(true);
                Enemy.SetActive(false);
                CameraArm.SetActive(false);
                FairCamera.SetActive(true);


                timer = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z) && PressSign.activeSelf == true)
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            ScoreTxt.Score = 0; //점수도 다시 초기화 해줘라.
        }


    }

 }
