using UnityEngine;
using UnityEngine.SceneManagement;
using Jinsol.RunGame;

namespace Jinsol.RunGame
{
    public class RunGame : MonoBehaviour
    {
        public Transform StandPos;
        public GameObject StartScreen;
        private GameObject RunGameInstance;
        [SerializeField] private MultiPlayer _Runner; // 싱글플레이어

        #region
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && _Runner._checkstate == CheckState.RunGame)
            {
                NewRunGame();
                _Runner.onMoveable = false;
            }
        }

        public void NewRunGame()
        {
            RunGameInstance = Instantiate(RunGameInstance, Vector3.zero, Quaternion.identity);
        }

        public void EndRunGame()
        {
            if(RunGameInstance != null)
            {
                Destroy(RunGameInstance);
            }
        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _Runner._checkstate = CheckState.RunGame;
            }
        }

        public void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _Runner._checkstate = CheckState.None;
            }
        }
        #endregion

    }

}