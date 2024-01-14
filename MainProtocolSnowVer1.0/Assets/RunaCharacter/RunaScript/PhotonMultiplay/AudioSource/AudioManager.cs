using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AudioSeonghyo
{
    public class AudioManager : MonoBehaviour
    {
        [Header("��ü ����� ���� �ҽ�")]
        [Space(10f)]
        //ĳ���� ����� ȿ���� 
        public AudioSource audioGroup;

        [Tooltip("����� ���� ������")]

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
