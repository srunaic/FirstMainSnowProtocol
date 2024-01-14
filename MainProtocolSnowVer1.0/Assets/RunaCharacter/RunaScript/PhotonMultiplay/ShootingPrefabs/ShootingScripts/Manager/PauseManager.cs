using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField]
    private GameObject PausePanel;

    public void GamePause()
    {
          if (isPaused)
          {
                ResumeGame();
               PausePanel.SetActive(false);
          }
          else
          {
                PauseGame();
                PausePanel.SetActive(true);
          }
       
    }
    //돌아가기 버튼
    public void Continue()
    {
        if (isPaused)
        {
            ResumeGame();
            PausePanel.SetActive(false);
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // 게임을 일시 정지
        isPaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // 게임을 재개
        isPaused = false;
    }
}
