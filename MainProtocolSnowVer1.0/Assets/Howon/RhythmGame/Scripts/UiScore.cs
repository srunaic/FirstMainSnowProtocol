using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Howon.RhythmGame
{
    public class UiScore : MonoBehaviour
    {
        private Text _scoreText;

        private void Awake()
        {
            _scoreText = GetComponent<Text>();
            _scoreText.text = string.Empty;

        }

        private void Update()
        {
            _scoreText.text = ShareDataManager.instance.TotalScore.ToString();
        }
    }
}
