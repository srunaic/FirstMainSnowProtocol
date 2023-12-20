using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(0);
            
        }        
    }

}
