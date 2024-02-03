using UnityEngine;

namespace Jinsol.RunGame
{
    public class ItemBomb : MonoBehaviour, itemI
    {
        public PlayerStatus playerStatus;

        void Awake()
        {
            playerStatus = FindObjectOfType<PlayerStatus>();
        }

        public void Use(PlayerStatus player)
        {
            player.OnDamage(214);
            Destroy(gameObject);
        }
    }
}
