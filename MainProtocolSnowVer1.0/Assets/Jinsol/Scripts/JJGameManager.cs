using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jinsol.RunGame
{
    public class JJGameManager : MonoBehaviour
    {
        ScoreManager scoreManager;
        public Animator playerAnimator;

        public void Awake()
        {
            scoreManager = (ScoreManager)GetComponent("ScoreManager");
        }

        public void Start()
        {
            StartCoroutine(GameEnd());
        }

        public IEnumerator GameEnd()
        {
            playerAnimator.SetBool("GameOver", false);
            yield return new WaitForSeconds(0.5f);
            playerAnimator.SetBool("IsRunning", true);
            yield return new WaitForSeconds(37f);
            playerAnimator.SetBool("IsRunning", false);
            playerAnimator.SetBool("GameOver", true);
            if (scoreManager.Score >= 7600)
            {
                playerAnimator.SetBool("Victory", true);
            }
            else
            {
                playerAnimator.SetBool("Failed", true);
            }
            yield return null;
        }
    }

}
