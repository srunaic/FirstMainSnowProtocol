using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace SeonghyoAudio
{
    public class AudioGameManage : MonoBehaviour
    {
        [Header("��ü ����� ���� �ҽ�")]
        [Space(10f)]

        public static AudioGameManage Instance;
         
        //ĳ���� ����� ȿ���� 
        public AudioSource audioGroup;

        [Tooltip("����� ���� ������")]

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
                    audioGroup.Play();//���� ����.
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


