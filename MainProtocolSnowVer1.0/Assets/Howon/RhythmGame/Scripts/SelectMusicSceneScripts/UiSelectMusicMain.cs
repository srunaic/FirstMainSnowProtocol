using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class ItemInfo
    {
        public string title;
        public string artist;
        public float playTime;
    }

    public class UiSelectMusicMain : MonoBehaviour
    {
        [SerializeField] private SheetMusic[] _sheets;
        private UiMusicList _musicList;

        public Main mainscript = null;
        private void Awake()
        {
            _musicList = transform.Find("MainPanel/MusicList").GetComponent<UiMusicList>();
        }

        private void Start()
        {
            if (_sheets != null)
            {
                _musicList.Init(_sheets);
            }
        }
    }
}
