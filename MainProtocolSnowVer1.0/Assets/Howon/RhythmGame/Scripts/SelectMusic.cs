using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class SelectMusic : MonoBehaviour
    {
        private GameObject _selectMusicCanvas;

        private void Awake()
        {
            _selectMusicCanvas = transform.Find("SelectMusicCanvas").gameObject;
        }

        void Start()
        {
            EventManager.instance.onStartMusicList = StartMusicList;
            EventManager.instance.onCloseMusicList = CloseMusicList;
        }

        private void StartMusicList()
        {
            _selectMusicCanvas.SetActive(true);
        }

        private void CloseMusicList()
        {
            _selectMusicCanvas.SetActive(false);

            EventManager.instance.onStartGameMain();
        }
    }
}
