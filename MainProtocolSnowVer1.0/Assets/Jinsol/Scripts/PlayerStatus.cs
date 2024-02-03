using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jinsol.RunGame
{
    public class PlayerStatus : MonoBehaviour
    {
        [Header("기본정보")]

        // HP
        [SerializeField] private int hp;
        [SerializeField] protected int maxHP = 3;
        [SerializeField] private int minHP = 0;

        // 상태
        [SerializeField] private bool isInvincible;
        [SerializeField] private bool isDead;

        // 점수
        [SerializeField] private int score;

        // 프로퍼티

        public int HP { get { return hp; } set { hp = value; } }
        public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }
        public bool IsDead { get { return isDead; } set { isDead = value; } }
        public int Score { get { return score; } set { score = value; } }

        private Rigidbody playerRigidbody;
        public ScoreManager scoreManager;
        public event Action onDeath;
        public Animator playerAnimator;

        private void Awake()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            playerRigidbody = (Rigidbody)GetComponent("Rigidbody");
            playerRigidbody.freezeRotation = true;
            playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
            playerAnimator = (Animator)GetComponent("Animator");
            IsDead = false;
            IsInvincible = false;
            minHP = 0;
            maxHP = 3;
            HP = maxHP;
            Score = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsInvincible || IsDead)
                return;
            else
            {
                if (other.TryGetComponent<itemI>(out itemI item))
                    item.Use(this);
            }
        }

        public void AddScore(int num)
        {
            scoreManager.Score += num;
            scoreManager.scoreText.text = scoreManager.Score.ToString();
            scoreManager.scoreSlider.value = scoreManager.Score;
        }

        public virtual void OnDamage(int damage)
        {
            if (!IsInvincible && !isDead && scoreManager.Score > 0)
            {
                scoreManager.Score -= damage;
                scoreManager.scoreText.text = scoreManager.Score.ToString();
                scoreManager.scoreSlider.value = scoreManager.Score;
            }
            else if (scoreManager.Score <= 0)
            {
                scoreManager.Score = 0;
                scoreManager.scoreText.text = scoreManager.Score.ToString();
                scoreManager.scoreSlider.value = scoreManager.Score;
            }
        }
    }
}
