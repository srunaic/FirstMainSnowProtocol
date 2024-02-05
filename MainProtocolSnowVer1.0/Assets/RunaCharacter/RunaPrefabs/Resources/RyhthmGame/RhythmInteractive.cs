using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Howon.RhythmGame;

    public class RhythmInteractive : MonoBehaviour
    {
        [Header("������� ���� �÷��̾� ����")]
        [SerializeField]
        private MultiPlayer _player1;
        [SerializeField]
        private HwaYeonMove _player2;

    private void Start()
    { 
        _player1.onMoveable = true;
    }

    GameObject gameInstance;
         #region ������� ���۽� �÷��̾� ���°���.
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
            Debug.LogError("�������� ã�� �� �����ϴ�. ��θ� Ȯ�����ּ���: " + prefabPath);
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
                _player1._checkstate = CheckState.None; //�÷��̾� 1
                _player2._checkstate = CheckHwaYeonState.None;  //�÷��̾� 2
            }
        }
        #endregion

    }
