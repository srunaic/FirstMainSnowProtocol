using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AudioSeonghyo
{
    public class AudioManager : MonoBehaviour
    {
        [Header("전체 오디오 관리 소스")]
        [Space(10f)]
        //캐릭터 오디오 효과음 
        public AudioSource audioGroup;

        [Tooltip("오디오 사운드 관리자")]

        public AudioClip MainBgm;
        public AudioClip DollGameSound;
        public LiftMultiDoll _DollGame;

        public bool isDollGameSoundPlaying = false;

        private void Start()
        {
            _DollGame = FindObjectOfType<LiftMultiDoll>();
        }

        private void Update()
        {
            PlayDollGameSound();
            MainSound();
        }
        public void MainSound()
        {
            if (!audioGroup.isPlaying && !isDollGameSoundPlaying )
            {
                audioGroup.clip = MainBgm;
                audioGroup.Play();
            }
    
        }

        public void PlayDollGameSound()
        {
            if (_DollGame._gameKind == GameKinded.DollGame)
            {
                if (!isDollGameSoundPlaying)
                {
                    audioGroup.clip = DollGameSound;
                    audioGroup.Play();
                    isDollGameSoundPlaying = true;
                }
            }
            else
            {
                isDollGameSoundPlaying = false;
            }
        }

    }

}
