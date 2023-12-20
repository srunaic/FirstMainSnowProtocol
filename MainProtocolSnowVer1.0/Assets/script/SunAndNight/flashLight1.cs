using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashLight1 : MonoBehaviour
{
    public Light flash;
    private float Count = 0;

   void Start()
    {
        flash.GetComponent<Light>().enabled = false;
     
    }
    void Update()
    {

        Light();

    }

    void Light()
    {
   
            if (Count == 0 && Input.GetKeyDown(KeyCode.B))
            {
                flash.GetComponent<Light>().enabled = true;

                Count = 1;
            }

            else if(Count == 1 && Input.GetKeyDown(KeyCode.B))
            {
                flash.GetComponent<Light>().enabled = false;

                Count = 0;
        }
            

    }
}
