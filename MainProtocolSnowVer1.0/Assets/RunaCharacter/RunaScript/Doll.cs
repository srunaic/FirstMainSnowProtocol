using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doll : MonoBehaviour
{
    [SerializeField]
    private LiftDollCatch _LiftDoll;

    private void Start()
    {
        _LiftDoll = FindObjectOfType<LiftDollCatch>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CheckDollCol"))
        {
            _LiftDoll.GetDollTxt.text = "Get Doll";
            _LiftDoll.GetDollTxt.enabled = true;
            StartCoroutine(_LiftDoll.TextShowDollCheck(2f));
        }
    }
}
