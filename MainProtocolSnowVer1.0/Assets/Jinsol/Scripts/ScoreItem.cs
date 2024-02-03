using UnityEngine;

namespace Jinsol.RunGame
{
    public class ScoreItem : MonoBehaviour, itemI
    {
        public ScoreManager scoreManager;

        public void Use(PlayerStatus player)
        {
            player.AddScore(419);
            Destroy(gameObject);
        }

        void Start()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}

