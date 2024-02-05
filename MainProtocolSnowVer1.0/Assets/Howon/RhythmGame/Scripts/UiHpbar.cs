using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Howon.RhythmGame
{
    public class UiHpbar : MonoBehaviour
    {
        private Slider slider;
        private Image _imgFill;

        public Color belowThresholdColor = Color.red;
        private Color normalColor;
        public float colorChangeThreshold = 300f;


        private void Awake()
        {
            slider = GetComponent<Slider>();
            _imgFill = transform.Find("FillArea/Fill").GetComponent<Image>();
            normalColor = _imgFill.color;
            slider.onValueChanged.AddListener(CheckSliderValue);
            CheckSliderValue(slider.value);
        }

        void Start()
        {
            EventManager.instance.onDamage = TakeDamage;
            EventManager.instance.onRecover = Recover;
        }

        void TakeDamage(int damage)
        {
            if (slider.value > 0)
            {
                slider.value -= damage;
            }
            else
            {
                EventManager.instance.onPreProcessGameOver();
            }
        }

        void Recover(int recover)
        {
            if (slider.value < slider.maxValue)
            {
                slider.value += recover;
            }
        }

        void CheckSliderValue(float value)
        {
            // 슬라이더의 값이 특정 임계값 이하일 때 색상 변경
            if (value <= colorChangeThreshold)
            {
                ChangeSliderColor(belowThresholdColor);
            }
            else
            {
                // 임계값을 초과할 경우 원래 색상으로 변경
                ChangeSliderColor(normalColor);
            }
        }

        void ChangeSliderColor(Color newColor)
        {
            _imgFill.color = newColor;
        }
    }
}
