using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Howon.RhythmGame
{
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource _musicSource;
        private float _initVolume = 1f;

        private AsyncOperationHandle _asyncAudioHandle;

        private void Awake()
        {
            _musicSource = GetComponent<AudioSource>();
            _musicSource.volume = _initVolume;
        }

        public void Play(string name)
        {
            _asyncAudioHandle = ResourceManager.instance.LoadAudioClip<AudioClip>(name);
            AudioClip audioClip = (AudioClip)_asyncAudioHandle.Result;

            if (audioClip != null)
            {
                _musicSource.clip = audioClip;

                _musicSource.Play();
            }
        }
        
        public void Stop()
        {
            _musicSource.Stop();
        }

        public void Pause()
        {
            _musicSource?.Pause();
        }

        public float GetVolume()
        {
            return _musicSource.volume;
        }

        public void SetVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        public void ResetVolume()
        {
            _musicSource.volume = _initVolume;
        }

        public float GetLength()
        {
            return _musicSource.clip.length;
        }

        public void ReleaseMusic()
        {
            ResourceManager.instance.ReleaseAsset(_asyncAudioHandle);
        }
    }
}
