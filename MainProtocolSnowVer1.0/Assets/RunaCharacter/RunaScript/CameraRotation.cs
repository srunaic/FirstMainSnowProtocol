using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotation : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    //카메라 따라다니면서 위치를 맞춤.
    public Vector3 followOffset = Vector3.zero;
    private float CamMoveSpeed = 2f;
    public bool invertY = false; // Y 축 회전 반전 여부

    //private float rotationX = 0.5f;

    [Header("LookAt")]
    public Transform lookTarget;
    public Vector3 lookOffset = Vector3.zero;//바라볼 위치를 보정하는 값.

    //카메라를 쳐다보면서 회전 조정.
    [SerializeField]
    private float angleY = 0;//캐릭터가 수평 상태일때의 회전 축 임의의 값.
    [SerializeField]
    private float angleZ = 0;//카메라 축의 방향 위

    private float rotateSpeedX = 10f;
    private float rotSpeedY = 10f;

    [Header("카메라 감도와 커서위치")]
    private bool CursorVisible = true;
    public float sensitivity = 2.0f; // 마우스 감도 조절 변수

    private void Start()
    {   
          followTarget = GameObject.Find("CamPivot").GetComponent<Transform>();
          lookTarget = GameObject.Find("CamPivot").GetComponent<Transform>();

          transform.position =
          followTarget.position + followTarget.rotation * followOffset;//카메라가 y축으로 얼마만큼 회전했나?

          angleY = 0;
          angleZ = 0;

    }

    private void FixedUpdate()//고정된 주기마다 업데이트(물리주기 ,훨씬 늦게 반복.0.02)
    {
        if (followTarget != null) //타겟이 있다면, 따라가라.
            CamFollow();
        if (lookTarget != null)
            CamLook();
        //물리주기에 맞춰서 반복.
       // CamSensitivity();
    }
    private void Update()
    {
        Cursors();
    }
    public void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");//마우스가 움직일때,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y축 앵글 바뀜.
        angleZ += mInputY * rotSpeedY;

        Quaternion rotX = Quaternion.Euler(-angleZ, angleY, 0); //이거 잘 조정하셈.

        Vector3 followPos =
            followTarget.position + rotX * followOffset; //앵글이 바뀌었을때,

        transform.position = 
            Vector3.Lerp(transform.position , followPos ,Time.deltaTime * CamMoveSpeed); //카메라가 떨리는 이유.
    }
    public void CamLook()
    {
        Quaternion camRot = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0);
        
        Vector3 lookPos = lookTarget.position + camRot * lookOffset;
        
        transform.LookAt(lookPos);
    }

     
     /*void CamSensitivity()//카메라 감도
     {
         // 마우스 입력 받기
         float mouseX = Input.GetAxis("Mouse X");
         float mouseY = Input.GetAxis("Mouse Y");

         // Y 축 회전 각도 계산
         rotationX -= mouseY * sensitivity * (invertY ? -1 : 1);

         // Y 축 회전 각도 제한 (원하는 각도로 조절)
         rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

         // 카메라와 캐릭터 회전 적용
         transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
         transform.rotation *= Quaternion.Euler(0.0f, mouseX * sensitivity, 0.0f);//고정값.
         transform.rotation *= Quaternion.Euler(0f, mouseY * sensitivity,0f);

     }*/

    private void Cursors()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CursorVisible = !CursorVisible; // 커서 상태를 토글합니다.

            // 커서 표시 여부 설정
            Cursor.visible = CursorVisible;

            // 커서 잠금 상태 설정 (커서를 잠그면 마우스가 화면 바깥으로 나가지 않습니다)
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

