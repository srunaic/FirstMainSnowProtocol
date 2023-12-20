using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotation : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    //카메라 따라다니면서 위치를 맞춤.
    public Vector3 followOffset = Vector3.zero;
    private float CamMoveSpeed = 5f;
    public bool invertY = false; // Y 축 회전 반전 여부
    private float rotationX = 0.2f; // X 축 회전 각도

    [Header("LookAt")]
    public Transform lookTarget;
    public Vector3 lookOffset = Vector3.zero;//바라볼 위치를 보정하는 값.

    //카메라를 쳐다보면서 회전 조정.
    [SerializeField]
    private float angleY = 0;//캐릭터가 수평 상태일때의 회전 축 임의의 값.
    [SerializeField]
    private float angleZ = 0;

    [Header("앵글 속도")]
    public float rotateSpeedX = 30f;
    public float rotSpeedY = 30f;

    [Header("카메라 감도와 커서위치")]
    private bool CursorVisible = true;
    public float sensitivity = 2.0f; // 마우스 감도 조절 변수


    private void Start()
    {
        transform.position = 
            followTarget.position + followTarget.rotation * followOffset;//카메라가 y축으로 얼마만큼 회전했나?
            
        angleY = 0;
        angleZ = 0;

    }
    private void LateUpdate()//늦은 업데이트 모든 업데이트가 끝나고 실행됨.
    { 
    }
    private void FixedUpdate()//고정된 주기마다 업데이트(물리주기 ,훨씬 늦게 반복.0.02)
    {
        if (followTarget != null) //타겟이 있다면, 따라가라.
            CamFollow();
        if (lookTarget != null)
            CamLook();

    }
    private void Update()
    {
        Cursors();
    }
    public void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");//마우스가 움직일때,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y축 마우스를 흔들때마다 앵글 바뀜.
        angleZ += mInputY * rotSpeedY;

        Quaternion rotX = Quaternion.Euler(0, angleY, angleZ);//앵글 값.

        angleY = Mathf.Clamp(angleY,-180f,180f);//Y축 카메라 각도제한.
       
        Vector3 followPos =
            followTarget.position + rotX * followOffset; //앵글이 바뀌었을때,

        transform.position = 
            Vector3.Lerp(transform.position , followPos ,Time.deltaTime * CamMoveSpeed); //돌아가는 함수.

        rotationX -= mInputY * sensitivity * (invertY ? -1 : 1);

        // 카메라와 캐릭터 회전시 카메라 감도 조절.
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        transform.rotation *= Quaternion.Euler(0.0f, mInputX * sensitivity, 0.0f);//고정값.
        transform.rotation *= Quaternion.Euler(0f, mInputY * sensitivity, 0f);
      
    }
    public void CamLook()
    {
        Quaternion camRot = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0);
        
        Vector3 lookPos = lookTarget.position + camRot * lookOffset;
        
        transform.LookAt(lookPos);

    }

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

