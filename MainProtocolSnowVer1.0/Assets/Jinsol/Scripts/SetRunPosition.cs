using UnityEngine;
using UnityEngine.UI;

namespace Jinsol.RunGame
{
    public class SetRunPosition : MonoBehaviour
    {
        public Transform playerTransform; // �÷��̾�
        public Transform runPosition;


        private void Awake()
        {
            runPosition = (Transform)GetComponent("Transform");
        }


        public void ChangeLane()
        {
            playerTransform.position = runPosition.position;
        }
    }
}
