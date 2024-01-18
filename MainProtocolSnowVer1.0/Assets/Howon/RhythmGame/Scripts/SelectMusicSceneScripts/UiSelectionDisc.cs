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

        private string _title = string.Empty;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;

                // Ÿ��Ʋ �̹��� �ҷ��ͼ� _discImage�Ҵ�

                _selectDisc.onClick.AddListener(() =>
                {
                    ShareDataManager.instance.Title = _title;
                    EventManager.instance.onReleaseAsset();
                    SceneManager.LoadScene("MainScene");
                });
            }
        }

        private void Awake()
        {
            _discImage = GetComponent<Image>();
            _selectDisc = transform.Find("SelectButton").GetComponent<Button>();
        }
    }
}
