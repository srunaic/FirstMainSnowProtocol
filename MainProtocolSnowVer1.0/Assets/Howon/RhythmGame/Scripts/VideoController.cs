using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;

namespace Howon.RhythmGame
{
    public class VideoController : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;
        private AsyncOperationHandle _asyncVideoHandle;
        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        public void Play(string name)
        {
            _asyncVideoHandle = ResourceManager.instance.LoadVideoClip<VideoClip>(name);
            VideoClip videoClip = (VideoClip)_asyncVideoHandle.Result;

            if (videoClip != null)
            {
                _videoPlayer.clip = videoClip;
                _videoPlayer.Play();
            }
        }

        public void ReleaseVideo()
        {
            ResourceManager.instance.ReleaseAsset(_asyncVideoHandle);
        }

    }
}
