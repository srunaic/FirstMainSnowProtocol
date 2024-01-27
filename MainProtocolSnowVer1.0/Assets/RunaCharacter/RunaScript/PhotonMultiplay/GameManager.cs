using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeonghyoGameManagerGroup //���ӸŴ��� �׷� �̿�.
{
    public class GameManager : MonoBehaviour
    {
        [Header("���� �ν��Ͻ�ȭ ���� �Ѿ���� �ı� �ȵ�.")]
        public static GameManager instance = null;
        public bool isConnect = false;
        public Transform[] spawnPoints;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (instance != null)
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            StartCoroutine(CreatePlayer());
        }

        public IEnumerator CreatePlayer()
        {
            yield return new WaitUntil(() => isConnect);//���� �ɶ����� ��� �ƴϸ� ����������

            spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();//���� �׷��� �ڽ� Ʈ������ ����.
            Vector3 pos = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].position; //���ð�
            Quaternion rot = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].rotation;

            GameObject _player = PhotonNetwork.Instantiate("PlayerRuna", Vector3.zero, Quaternion.identity, 0);//�÷��̾� ����
        }

    }
}

