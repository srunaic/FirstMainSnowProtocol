using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FollowCam : MonoBehaviour
{
    private float CamMoveSpeed = 1f;
    private float angleY = 0;
    private float rotateSpeedX = 3f;

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

    //�÷��̾�� �� ���� �Ѱ��ְ� �޾ƿ´�. 
    public void SetPlayer(Transform newPlayer, Transform Target)
    {
        player = newPlayer;
        followTarget = Target;

        transform.position =
        followTarget.position + followTarget.rotation * followOffset;//ī�޶� y������ �󸶸�ŭ ȸ���߳�?

        angleY = 0;
        angleZ = 0;
    }

    private void FixedUpdate()//������ �ֱ⸶�� ������Ʈ(�����ֱ� ,�ξ� �ʰ� �ݺ�.0.02)
    {
        if (followTarget != null)    //Ÿ���� �ִٸ�, ���󰡶�.
        {
            CamLook();
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
   
    public void CamLook()
    {
        Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        Vector3 lookPos = followTarget.position + camRot * offset;
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
}
