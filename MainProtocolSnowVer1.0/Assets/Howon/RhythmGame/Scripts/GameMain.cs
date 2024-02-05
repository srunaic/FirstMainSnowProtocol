using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class GameMain : MonoBehaviour
    {
        private GameObject _main;
        private GameObject _mainCanvas;

        private void Awake()
        {
            _main = transform.Find("Main").gameObject;
            _mainCanvas = transform.Find("MainCanvas").gameObject;
        }

        private void Start()
        {
            EventManager.instance.onStartGameMain = StartGameMain;
            EventManager.instance.onCloseGameMain = CloseGameMain;
        }

        private void StartGameMain()
        {
            _main.SetActive(true);
            _mainCanvas.SetActive(true);
        }

        private void CloseGameMain()
        {
            _main.SetActive(false);
            _mainCanvas.SetActive(false);

            EventManager.instance.onStartGameOver();
        }
    }
}
