using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSource : MonoBehaviour
{
    GameObject BackgroundMusic;
    AudioSource backmusic;
    internal AudioClip clip;

    void Start()
    {
        backmusic = GetComponent<AudioSource>();
    }

    internal void Play()
    {
        throw new NotImplementedException();
    }
    /*  void Awake()
 {
     BackgroundMusic = GameObject.Find("BackgroundMusic");
     backmusic = BackgroundMusic.GetComponent<AudioSource>(); //������� �����ص�
     if (backmusic.isPlaying)
     {
         return; //��������� ����ǰ� �ִٸ� �н�
     }
     else
     {
         backmusic.Play();
         DontDestroyOnLoad(BackgroundMusic); //������� ��� ����ϰ�(���� ��ư�Ŵ������� ����)
     }
 }*/


}
