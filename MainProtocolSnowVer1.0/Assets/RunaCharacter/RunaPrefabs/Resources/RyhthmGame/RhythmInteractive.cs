using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Howon.RhythmGame;

    public class RhythmInteractive : MonoBehaviour
    {
        [Header("리듬게임 접속 플레이어 관리")]
        [SerializeField]
        private MultiPlayer _player1;
        [SerializeField]
        private HwaYeonMove _player2;

    private void Start()
    { 
        _player1.onMoveable = true;
    }

    GameObject gameInstance;
         #region 리듬게임 시작시 플레이어 상태관리.
         private void Update()
         {
            if(Input.GetKeyDown(KeyCode.Z) && _player1._checkstate == CheckState.RyhthmGame
                && _player2._checkstate == CheckHwaYeonState.RyhthmGame)
            {
                CreateRhythmGame();
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                _player1.onMoveable = true;
                _player2.onMoveable = true;
            }
            if(Input.GetKeyDown(KeyCode.V))
            {
                DestroyRhythmGame();
            }
        }
        public void CreateRhythmGame()
        {
            string prefabPath = "RhythmGame";
            GameObject prefab = Resources.Load(prefabPath) as GameObject;


            if (prefab != null)
        {
            gameInstance = Instantiate(prefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("프리팹을 찾을 수 없습니다. 경로를 확인해주세요: " + prefabPath);
        }

        }
       public void DestroyRhythmGame()
       {
            EventManager.instance.onGameTerminate = () =>
            {
            Destroy(gameInstance);
            };
       }
        public void OnTriggerEnter(Collider col)
        {
            if(col.CompareTag("Player"))
            {
                _player1._checkstate = CheckState.RyhthmGame;
                _player2._checkstate = CheckHwaYeonState.RyhthmGame;
            }
        }
        public void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _player1._checkstate = CheckState.None; //플레이어 1
                _player2._checkstate = CheckHwaYeonState.None;  //플레이어 2
            }
        }
        #endregion

    }
