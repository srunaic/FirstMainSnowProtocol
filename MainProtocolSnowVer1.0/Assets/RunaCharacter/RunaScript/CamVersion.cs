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
            Camera1.SetActive(true);
            Camera2.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera1.SetActive(false);
            Camera2.SetActive(true);
        }
    }

}
