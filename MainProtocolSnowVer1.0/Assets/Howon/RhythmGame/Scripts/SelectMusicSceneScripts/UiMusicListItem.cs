using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Howon.RhythmGame
{
    public class UiMusicListItem : MonoBehaviour
    {
        public Button _buttonList;
        private Text _txtTitle;
        private Text _txtArtist;
        private Text _txtPlayTime;

        private void Awake()
        {
            _buttonList = GetComponent<Button>();
            _txtTitle = transform.Find("Txt_TitleName").GetComponent<Text>();
            _txtArtist = transform.Find("Txt_Artist").GetComponent<Text>();
            _txtPlayTime = transform.Find("Txt_PlayTime").GetComponent<Text>();
        }

        public void Init(ItemInfo data)
        {
            _txtTitle.text = data.title;
            _txtArtist.text = data.artist;
            _txtPlayTime.text = data.playTime.ToString();
        }
    }
}
