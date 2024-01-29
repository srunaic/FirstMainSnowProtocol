using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeonghyoGameManagerGroup //���ӸŴ��� �׷� �̿�.
{
    public enum ChoicePlayer 
    {
       NonePlayer,
       RunaPlayer,
       HwaYeonPlayer
    }
    public class GameManager : MonoBehaviour
    {
        [Header("���� �ν��Ͻ�ȭ ���� �Ѿ���� �ı� �ȵ�.")]
        public static GameManager instance = null;
        public bool isConnect = false;
        public Transform[] spawnPoints;

        public ChoicePlayer _choicePlayer = ChoicePlayer.NonePlayer;

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

            if (_choicePlayer == ChoicePlayer.RunaPlayer) // 1�� �������� ������ 
            {
                GameObject _player2 = PhotonNetwork.Instantiate("PlayerRuna", Vector3.zero, Quaternion.identity, 0);//�÷��̾� ����
            }
            else if (_choicePlayer == ChoicePlayer.HwaYeonPlayer)
            {
                GameObject _player = PhotonNetwork.Instantiate("Multi Dwarf Idle", Vector3.zero, Quaternion.identity, 0);//�÷��̾� ����
            
            } 
        }

    }
}

