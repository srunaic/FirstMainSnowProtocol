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

    //�� �ִϸ��̼� ���� �ؽ�Ʈ Ŭ���� �߻�.
    public void PlayOnBell()
    {
        BellAnim.SetBool("OnBell",true);
        textMesh.text = "�� ġ��";

        StartCoroutine(SoundTrack());

    }

    //�÷��� ���˽�
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
    IEnumerator SoundTrack() //���� ������ Ÿ�̹�
    {
        AudioGameManage.Instance.bellSound(); //�ν��Ͻ� ����� �Ŵ��� ȣ��.
        yield return new WaitForSeconds(2f);
    }

}
