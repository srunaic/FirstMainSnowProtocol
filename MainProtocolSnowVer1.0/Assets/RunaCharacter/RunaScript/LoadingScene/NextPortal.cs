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
            LoadingScene.LoadScene("PingPongGame");//�ε����� �Ű����� ��string�� �ȿ� �ε��� �ɶ����� �ε�ȭ�� ������ ���� �� ��ȯ���.
        }
    }
}
