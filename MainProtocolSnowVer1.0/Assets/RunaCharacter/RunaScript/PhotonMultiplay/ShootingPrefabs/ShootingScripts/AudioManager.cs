using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //ĳ���� ����� ȿ���� 
    public AudioSource audioSource;
    
    public AudioClip BossBgm;

    public void AudioListen()
    {
        audioSource.clip = BossBgm;
        audioSource.Play();
    }



}
