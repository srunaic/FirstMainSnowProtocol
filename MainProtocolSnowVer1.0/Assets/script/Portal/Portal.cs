using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Portal : MonoBehaviour
{
    public Transform ExitPos;

    [SerializeField]
    private RunaMoving thePlayer;

    private void Start()
    {
        thePlayer = FindObjectOfType<RunaMoving>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            thePlayer.transform.position = ExitPos.position;
            Debug.Log("플레이어" +thePlayer);
        }
    }
}
