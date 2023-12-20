using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancleOption : MonoBehaviour
{
    public int Count = 0;
    public GameObject VolSlide;
    public GameObject Sound;
    public GameObject Map;
    
    void Update()
    {
        vol();
        MiniMap();
    }

    private void vol() 
    {

        if (Count == 0 && Input.GetKeyDown(KeyCode.C))
        {

            VolSlide.SetActive(false);
            Sound.SetActive(false);

            Count = 1;
        }

        else if (Count == 1 && Input.GetKeyDown(KeyCode.C))
        {

            VolSlide.SetActive(true);
            Sound.SetActive(true);
            Count = 0;
        }

    }

    private void MiniMap() 
    {

        if (Count == 0 && Input.GetKeyDown(KeyCode.M))
        {


            Map.SetActive(false);

            Count = 1;
        }

        else if (Count == 1 && Input.GetKeyDown(KeyCode.M))
        {

  
            Map.SetActive(true);
            Count = 0;
        }

    }
}
