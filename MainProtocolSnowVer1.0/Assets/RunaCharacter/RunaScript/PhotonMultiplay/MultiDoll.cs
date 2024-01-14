using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDoll : MonoBehaviour
{
    [SerializeField]
    private MultiLiftCatch _LiftDoll;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckDollCol"))
        {
            _LiftDoll.GetDollTxt.text = "Get Doll";
            _LiftDoll.GetDollTxt.enabled = true;
            StartCoroutine(_LiftDoll.TextShowDollCheck(2f));
        }
    }*/
}
