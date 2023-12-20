using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnOff : MonoBehaviour
{
    public Light renderer;

    void Start()
    {
        // 플래쉬를 끕니다.
        renderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // 플래쉬의 enabled 속성을 반전합니다.
            renderer.enabled = !renderer.enabled;
        }
    }
}
