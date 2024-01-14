using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamVersion : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera1;
    [SerializeField]
    private GameObject Camera2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Camera2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera2.SetActive(false);
        }
    }
  
}
