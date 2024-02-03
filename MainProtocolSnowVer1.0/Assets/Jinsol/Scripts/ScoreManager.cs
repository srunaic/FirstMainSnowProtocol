using UnityEngine;
using UnityEngine.UI;

namespace Jinsol.RunGame
{
    public class ScoreManager : MonoBehaviour
    {
        public Slider scoreSlider;
        public Text scoreText;

        [SerializeField]
        private int score = 0;
        public int Score { get { return score; } set { score = value; } }

        private void Awake()
        {
            Score = 0;
            scoreText.text = "";
            scoreSlider.value = 0;
        }

        private void Update()
        {
            UpdateScore();
        }

        public void Hit()
        {
            score += 214;
        }

        private void UpdateScore()
        {

        }
    }

}
