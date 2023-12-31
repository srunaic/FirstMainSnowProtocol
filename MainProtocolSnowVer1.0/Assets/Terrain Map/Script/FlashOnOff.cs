using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnOff : MonoBehaviour
{
    public Light renderer;

    void Start()
    {
        // �÷����� ���ϴ�.
        renderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // �÷����� enabled �Ӽ��� �����մϴ�.
            renderer.enabled = !renderer.enabled;
        }
    }
}
