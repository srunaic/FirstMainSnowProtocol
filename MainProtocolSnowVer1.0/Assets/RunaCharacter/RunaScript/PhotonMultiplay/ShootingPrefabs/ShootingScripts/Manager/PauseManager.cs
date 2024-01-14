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
    //���ư��� ��ư
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
        Time.timeScale = 0f; // ������ �Ͻ� ����
        isPaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // ������ �簳
        isPaused = false;
    }
}
