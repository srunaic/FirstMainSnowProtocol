using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField]
    public GameObject boss;
    public GameObject Hpbar;

    public AudioManager audiomanager;
    //보스가 나올 시간
    static public int BossCount = 0; //씬 하나마다 적용해줄 보스
    public int SpawnCount = 10;

    private bool SpawnEnd = false;

    private void Start()
    {
        BossCount = 0;
    }

    private void Awake()
    {
        audiomanager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (!SpawnEnd && BossCount > SpawnCount)
        {
            audiomanager.AudioListen();
            Hpbar.SetActive(true);
            boss.SetActive(true);

            SpawnEnd = true;
        }

    }
}
