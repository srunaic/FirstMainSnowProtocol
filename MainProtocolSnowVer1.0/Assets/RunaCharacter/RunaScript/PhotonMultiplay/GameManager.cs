using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeonghyoGameManagerGroup //게임매니저 그룹 이용.
{
    public class GameManager : MonoBehaviour
    {
        [Header("게임 인스턴스화 씬이 넘어가더도 파괴 안됨.")]
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
            yield return new WaitUntil(() => isConnect);//연결 될때까지 대기 아니면 실행하지마

            spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();//스폰 그룹의 자식 트랜스폼 들고옴.
            Vector3 pos = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].position; //대기시간
            Quaternion rot = spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount].rotation;

            GameObject _player = PhotonNetwork.Instantiate("PlayerRuna", Vector3.zero, Quaternion.identity, 0);//플레이어 접속
        }

    }
}

