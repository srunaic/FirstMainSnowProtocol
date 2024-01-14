using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //캐릭터 오디오 효과음 
    public AudioSource audioSource;
    
    public AudioClip BossBgm;

    public void AudioListen()
    {
        audioSource.clip = BossBgm;
        audioSource.Play();
    }



}
