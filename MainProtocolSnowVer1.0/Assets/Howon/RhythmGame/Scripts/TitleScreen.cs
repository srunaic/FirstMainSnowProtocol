using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Howon.RhythmGame
{
    public class TitleScreen : MonoBehaviour
    {
        private Button _btnStart;
        private Text _txtStart;

        [SerializeField] private float _fadeDuration = 2.0f;

        private Color _targetColor;
        private bool _isFadingIn = true;

        private void Awake()
        {
            _btnStart = GetComponent<Button>();
            _txtStart = transform.Find("TxtStart").GetComponent<Text>();

            _btnStart.onClick.AddListener(() => SceneManager.LoadScene("SelectMusicScene"));
        }

        void Start()
        {
            _targetColor = _txtStart.color;
            _targetColor.a = 0.0f;
            _txtStart.color = _targetColor;
        }

        private void Update()
        {
            Fade();
        }

        private void Fade()
        {
            float alphaDelta = Time.deltaTime / _fadeDuration;

            if (_isFadingIn)
            {
                // ���̵� �� ���̸� ���� ���� �������� ��Ÿ���� ��
                _targetColor.a += alphaDelta;
                if (_targetColor.a >= 1.0f)
                {
                    _targetColor.a = 1.0f;
                    _isFadingIn = false;
                }
            }
            else
            {
                // ���̵� �ƿ� ���̸� ���� ���� ���ҽ��� ������� ��
                _targetColor.a -= alphaDelta;
                if (_targetColor.a <= 0.0f)
                {
                    _targetColor.a = 0.0f;
                    _isFadingIn = true;
                }
            }

            // ���� ���� ���� ���� �ؽ�Ʈ ������Ʈ�� ����
            _txtStart.color = _targetColor;
        }
    }
}
