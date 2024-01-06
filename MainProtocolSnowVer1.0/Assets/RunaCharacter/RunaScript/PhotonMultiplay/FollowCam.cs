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
    public float sensitivity = 2.0f; // ���콺 ���� ���� ����
   
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

    private void FixedUpdate()//������ �ֱ⸶�� ������Ʈ(�����ֱ� ,�ξ� �ʰ� �ݺ�.0.02)
    {
        if (followTarget != null)
        {//Ÿ���� �ִٸ�, ���󰡶�.
                Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

                Vector3 lookPos = followTarget.position + camRot * offset;
                // �÷��̾ �ٶ󺸵��� ȸ�� ����
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
            // �÷��̾��� ��ġ�� ���󰡵��� ī�޶��� ��ġ�� ������Ʈ
            transform.position = player.position + offset;

            // �÷��̾ �ٶ󺸵��� ī�޶��� ȸ���� ������Ʈ (���� ȸ���� ������ ���� ��� Quaternion.identity ���)
            transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);
        }
    }

    //�÷��̾�� �� ���� �Ѱ��ְ� �޾ƿ´�. 
    public void SetPlayer(Transform newPlayer,Transform Target)
    {
        player = newPlayer;
        followTarget = Target;

        transform.position = followTarget.position + followTarget.rotation * followOffset;

        transform.LookAt(followTarget.position);
    }
    public void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");//���콺�� �����϶�,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y�� �ޱ� �ٲ�.
        angleZ += mInputY * rotSpeedY;

        Quaternion rotX = Quaternion.Euler(0, angleY, angleZ);

        Vector3 followPos =
            player.position + rotX * offset; //�ޱ��� �ٲ������,

        transform.position =
            Vector3.Lerp(transform.position, followPos, Time.deltaTime * CamMoveSpeed); //���ư��� �Լ�.
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
