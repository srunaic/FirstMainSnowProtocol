using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SeonghyoAudio;

public class BellTwingcle : MonoBehaviour
{
    [Header("Bell")]

    [SerializeField]
    private GameObject BellUI;

    public TextMeshProUGUI textMesh;

    [SerializeField]
    private Animator BellAnim;

    private void Start()
    {
        BellAnim = GetComponent<Animator>();
    }

    //벨 애니메이션 실행 텍스트 클릭시 발생.
    public void PlayOnBell()
    {
        BellAnim.SetBool("OnBell",true);
        textMesh.text = "종 치기";

        StartCoroutine(SoundTrack());

    }

    //플레이 접촉시
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            BellUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
         BellUI.SetActive(false);
         BellAnim.SetBool("OnBell", false);
        
    }
    IEnumerator SoundTrack() //사운드 나오는 타이밍
    {
        AudioGameManage.Instance.bellSound(); //인스턴스 오디오 매니저 호출.
        yield return new WaitForSeconds(2f);
    }

}
