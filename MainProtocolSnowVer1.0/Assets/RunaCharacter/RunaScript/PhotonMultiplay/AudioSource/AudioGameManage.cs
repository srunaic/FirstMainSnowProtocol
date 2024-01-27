using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace SeonghyoAudio
{
    public class AudioGameManage : MonoBehaviour
    {
        [Header("전체 오디오 관리 소스")]
        [Space(10f)]

        public static AudioGameManage Instance;
         
        //캐릭터 오디오 효과음 
        public AudioSource audioGroup;

        [Tooltip("오디오 사운드 관리자")]

        public PlayableDirector MainBgm; //Timeline Director stage;
        public AudioClip DollGameSound;
        public AudioClip BellAudio;

        public LiftMultiDoll _DollGame;

        public bool isDollGameSoundPlaying = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

   
        private void Update()
        {
            PlayDollGameSound();
        }

        public void MainSound(PlayableDirector aDirector)
        {
            if (aDirector == MainBgm && isDollGameSoundPlaying)
            {
                audioGroup.clip = DollGameSound;
                audioGroup.Stop();
                MainBgm.Play();
            }
        }

        public void PlayDollGameSound()
        {
            if (_DollGame._gameKind == GameKinded.DollGame)
            {
                if (!isDollGameSoundPlaying)
                {
                    audioGroup.clip = DollGameSound;
                    audioGroup.Play();//인형 사운드.
                    MainBgm.Stop();
                    isDollGameSoundPlaying = true;
                }
            }
            else
            {
                isDollGameSoundPlaying = false;
            }
        }

        public void bellSound()
        {

            audioGroup.clip = BellAudio;
            audioGroup.Play();

        }
        public void OffbellSound()
        {

            audioGroup.clip = BellAudio;
            audioGroup.Stop();

        }



    }
}


