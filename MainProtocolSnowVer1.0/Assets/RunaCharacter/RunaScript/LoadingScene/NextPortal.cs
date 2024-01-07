using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadingScene.LoadScene("PingPongGame");//로딩씬의 매개변수 씬string값 안에 로딩이 될때마다 로딩화면 나오고 다음 씬 전환방식.
        }
    }
}
