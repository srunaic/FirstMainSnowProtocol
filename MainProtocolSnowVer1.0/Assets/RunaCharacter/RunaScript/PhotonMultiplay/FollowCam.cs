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

    //ī�޶� �Ĵٺ��鼭 ȸ�� ����.
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
    //�÷��̾�� �� ���� �Ѱ��ְ� �޾ƿ´�. 
    public void SetPlayer(Transform Target)
    {
        followTarget = Target;

        transform.position = followTarget.position + followOffset; // ī�޶� y������ �󸶸�ŭ ȸ���߳�?
    }

    void Update()
    {
 
        Cursors();

  
        if (followTarget != null)
        {
            CamLook();

            // �÷��̾��� ��ġ�� ���󰡵��� ī�޶��� ��ġ�� ������Ʈ
            transform.position = followTarget.position + followOffset;
            // ���콺�� X �� Y ������ ����Ͽ� ī�޶� ȸ��
           
            float mouseX = Input.GetAxis("Mouse X") ;
            float mouseY = Input.GetAxis("Mouse Y") ;  // Y ������ ������Ŵ

            angleY += mouseX * rotateSpeedX;
            angleZ += mouseY * rotateSpeedX;

            // Y ������ �����Ͽ� ������� �� ���� �ٶ��� �ʵ��� ��
            angleZ = Mathf.Clamp(angleZ, -80f, 80f);

            Quaternion camRotY = Quaternion.Euler(0, angleY, 0);
            Quaternion camRotZ = Quaternion.Euler(-angleZ, 0, 0);
            transform.rotation = camRotY * camRotZ;

            // �÷��̾ �ٶ󺸵��� ī�޶��� ȸ���� ������Ʈ (���� ȸ���� ������ ���� ��� Quaternion.identity ���)
            //Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, camRot, Time.deltaTime * rotateSpeedX);
        }
    }
   
    public void CamLook()
    {
        Quaternion camRot = Quaternion.Euler(0, followTarget.transform.eulerAngles.y, 0);

        Vector3 lookPos = followTarget.position + camRot * followOffset;
        // �÷��̾ �ٶ󺸵��� ȸ�� ����
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
