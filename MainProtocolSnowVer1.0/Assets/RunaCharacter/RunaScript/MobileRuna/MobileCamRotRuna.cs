using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MobileCamRotRuna : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    //카메라 따라다니면서 위치를 맞춤.
    public Vector3 followOffset = Vector3.zero;
    private float CamMoveSpeed = 1f;
    public bool invertY = false; // Y 축 회전 반전 여부

    [Header("LookAt")]
    public Transform lookTarget;
    public Vector3 lookOffset = Vector3.zero;//바라볼 위치를 보정하는 값.

    //카메라를 쳐다보면서 회전 조정.
    [SerializeField]
    private float angleY = 0;//캐릭터가 수평 상태일때의 회전 축 임의의 값.
    private float rotateSpeedX = 0.2f;

    [Header("카메라 감도와 커서위치")]
    private bool CursorVisible = true;
 
    private void Start()
    {
        transform.position =
            followTarget.position + followTarget.rotation * followOffset;//카메라가 y축으로 얼마만큼 회전했나?

        angleY = 0;

    }
    private void FixedUpdate()//고정된 주기마다 업데이트(물리주기 ,훨씬 늦게 반복.0.02)
    {
        if (followTarget != null) //타겟이 있다면, 따라가라.
            CamFollow();
    }
    private void Update()
    {
        if (followTarget != null) // 플레이어가 존재한다면
        {
            Vector3 newPosition = followTarget.position; // 플레이어의 위치를 가져옴
            newPosition.y = transform.position.y; // 카메라의 높이를 유지하기 위해 Y 좌표만 변경

            transform.position = newPosition; // 카메라의 위치를 업데이트하여 플레이어를 따라가도록 함
        }
    }
    public void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");//마우스가 움직일때,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y축 앵글 바뀜.

        Quaternion rotX = Quaternion.Euler(0, angleY,0);

        Vector3 followPos =
            followTarget.position + rotX * followOffset; //앵글이 바뀌었을때,
       
        transform.position =
            Vector3.Lerp(transform.position, followPos, Time.deltaTime * CamMoveSpeed);
    }
 
}
