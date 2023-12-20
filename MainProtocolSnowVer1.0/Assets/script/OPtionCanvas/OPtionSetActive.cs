using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPtionSetActive : MonoBehaviour
{
    public GameObject Option;
    private int Count = 0;


    void Update()
    {
        OPtionInput(); 
    }
    void OPtionInput()
    {
        if (Count == 0 && Input.GetKeyDown(KeyCode.T))
        {
            Option.gameObject.SetActive(true);
            Count = 1;
        }
        else if(Count == 1 && Input.GetKeyDown(KeyCode.T))
        {
            Option.gameObject.SetActive(false);
            Count = 0;
        }
     
    }
}
