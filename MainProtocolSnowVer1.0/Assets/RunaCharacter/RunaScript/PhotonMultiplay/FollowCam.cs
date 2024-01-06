using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowCam : MonoBehaviour
{
    private float CamMoveSpeed = 5f;
    private float angleY = 0;
    private float rotateSpeedX = 10f;

    private bool CursorVisible = true;
    public float sensitivity = 2.0f; // 마우스 감도 제어 변수
   
    public Vector3 followOffset;
   
    [Header("Follow")]
    private Transform followTarget;
   
    [Header("LookAt")]
    public Transform player;
    public Vector3 offset ;

    //카메라를 쳐다보면서 회전 조정.
    [SerializeField]
    private float angleZ = 0;
    private float rotSpeedY = 10f;

    private void FixedUpdate()//고정된 주기마다 업데이트(물리주기 ,훨씬 늦게 반복.0.02)
    {
        if (followTarget != null)
        {//타겟이 있다면, 따라가라.
                Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

                Vector3 lookPos = followTarget.position + camRot * offset;
                // 플레이어를 바라보도록 회전 설정
                transform.LookAt(lookPos);
        }
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);
        }

    }
    void Update()
    {
        Cursors();

        if (player != null)
        {
            // 플레이어의 위치를 따라가도록 카메라의 위치를 업데이트
            transform.position = player.position + offset;

            // 플레이어를 바라보도록 카메라의 회전을 업데이트 (수직 회전을 원하지 않을 경우 Quaternion.identity 사용)
            transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);
        }
    }

    //플레이어에게 이 값을 넘겨주고 받아온다. 
    public void SetPlayer(Transform newPlayer,Transform Target)
    {
        player = newPlayer;
        followTarget = Target;

        transform.position = followTarget.position + followTarget.rotation * followOffset;

        transform.LookAt(followTarget.position);
    }
    public void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");//마우스가 움직일때,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y축 앵글 바뀜.
        angleZ += mInputY * rotSpeedY;

        Quaternion rotX = Quaternion.Euler(0, angleY, angleZ);

        Vector3 followPos =
            player.position + rotX * offset; //앵글이 바뀌었을때,

        transform.position =
            Vector3.Lerp(transform.position, followPos, Time.deltaTime * CamMoveSpeed); //돌아가는 함수.
    }
    private void Cursors()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            CursorVisible = !CursorVisible;
            Cursor.visible = CursorVisible;

            if (CursorVisible)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

    }
}
