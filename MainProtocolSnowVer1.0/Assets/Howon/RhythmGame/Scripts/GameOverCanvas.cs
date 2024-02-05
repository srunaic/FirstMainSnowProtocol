using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class GameOverCanvas : MonoBehaviour
    {
        private GameObject _gameOverPanel;

        private void Awake()
        {
            _gameOverPanel = transform.Find("GameOverPanel").gameObject;
        }

        void Start()
        {
            EventManager.instance.onStartGameOver = StartGameOver;
            EventManager.instance.onCloseGameOver = CloseGameOver;
        }

        private void StartGameOver()
        {
            _gameOverPanel.SetActive(true);
        }

        private void CloseGameOver()
        {
            _gameOverPanel.SetActive(false);

            EventManager.instance.onStartMusicList();
        }
    }
}

