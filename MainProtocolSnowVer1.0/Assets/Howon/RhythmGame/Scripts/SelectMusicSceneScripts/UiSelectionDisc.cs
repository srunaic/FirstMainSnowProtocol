using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Howon.RhythmGame
{
    public class UiSelectionDisc : MonoBehaviour
    {
        private Image _discImage;
        private Button _selectDisc;
        private Button _noteSpeedTimes;

        private string _title = string.Empty;
        private readonly int _numSpeedTimes = 3;
        private int _curNumSpeedTimes = 0;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;

                // 타이틀 이미지 불러와서 _discImage할당

                _selectDisc.onClick.AddListener(() =>
                {
                    ShareDataManager.instance.Title = _title;
                    EventManager.instance.onCloseMusicList();
                });
            }
        }

        private void Awake()
        {
            _discImage = GetComponent<Image>();
            _selectDisc = transform.Find("SelectButton").GetComponent<Button>();
            _noteSpeedTimes = transform.Find("NoteSpeedTimes").GetComponent<Button>();


            _noteSpeedTimes.onClick.AddListener(() =>
            {
                _curNumSpeedTimes++;
                _curNumSpeedTimes = _curNumSpeedTimes % _numSpeedTimes;
                Text txtSpeedTimes = _noteSpeedTimes.transform.Find("Text").GetComponent<Text>();
                if (_curNumSpeedTimes == 0)
                {
                    txtSpeedTimes.text = "X1";
                }
                else if (_curNumSpeedTimes == 1)
                {
                    txtSpeedTimes.text = "X2";
                }
                else if (_curNumSpeedTimes == 2)
                {
                    txtSpeedTimes.text = "X3";
                }

                ShareDataManager.instance.NoteSpeedTimes = _curNumSpeedTimes + 1;
            });
        }


    }
}
