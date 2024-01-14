using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
public class FollowCam : MonoBehaviour, IPunObservable
{
    private float CamMoveSpeed = 1f;
    private float angleY = 0;
    private float rotateSpeedX = 3f;

    private bool CursorVisible = true;

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

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    void Start()
    {
        angleY = 0;
        angleZ = 0;
       
    }
    //플레이어에게 이 값을 넘겨주고 받아온다. 
    public void SetPlayer(Transform Target)
    {
        followTarget = Target;

        transform.position = followTarget.position + followOffset; // 카메라가 y축으로 얼마만큼 회전했나?
    }

    void Update()
    {
 
        Cursors();

  
        if (followTarget != null)
        {
            CamLook();

            // 플레이어의 위치를 따라가도록 카메라의 위치를 업데이트
            transform.position = followTarget.position + followOffset;
            // 마우스의 X 및 Y 각도를 사용하여 카메라 회전
           
            float mouseX = Input.GetAxis("Mouse X") ;
            float mouseY = Input.GetAxis("Mouse Y") ;  // Y 각도는 반전시킴

            angleY += mouseX * rotateSpeedX;
            angleZ += mouseY * rotateSpeedX;

            // Y 각도를 제한하여 오버헤드 및 땅을 바라보지 않도록 함
            angleZ = Mathf.Clamp(angleZ, -80f, 80f);

            Quaternion camRotY = Quaternion.Euler(0, angleY, 0);
            Quaternion camRotZ = Quaternion.Euler(-angleZ, 0, 0);
            transform.rotation = camRotY * camRotZ;

            // 플레이어를 바라보도록 카메라의 회전을 업데이트 (수직 회전을 원하지 않을 경우 Quaternion.identity 사용)
            //Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, camRot, Time.deltaTime * rotateSpeedX);
        }
    }
   
    public void CamLook()
    {
        Quaternion camRot = Quaternion.Euler(0, followTarget.transform.eulerAngles.y, 0);

        Vector3 lookPos = followTarget.position + camRot * followOffset;
        // 플레이어를 바라보도록 회전 설정
        transform.LookAt(lookPos);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Camera.main.transform.position);
            stream.SendNext(Camera.main.transform.rotation);

        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
     
        }
    }
}
