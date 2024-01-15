using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamVersion : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera1;
    [SerializeField]
    private GameObject Camera2;
   
    private float angleY = 0;
    //카메라를 쳐다보면서 회전 조정.
    [SerializeField]
    private float angleZ = 0;
    private float rotateSpeedX = 3f;

    private void Start()
    {
        angleY = 0;
        angleZ = 0;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");  // Y 각도는 반전시킴

        angleY += mouseX * rotateSpeedX;
        angleZ += mouseY * rotateSpeedX;

        // Y 각도를 제한하여 오버헤드 및 땅을 바라보지 않도록 함
        angleZ = Mathf.Clamp(angleZ, -80f, 80f);

        Quaternion camRotY = Quaternion.Euler(0, angleY, 0);
        //Quaternion camRotZ = Quaternion.Euler(-angleZ, 0, 0);
        transform.rotation = camRotY; //* camRotZ;


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Camera2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera1.SetActive(false);

        }
    }

}
