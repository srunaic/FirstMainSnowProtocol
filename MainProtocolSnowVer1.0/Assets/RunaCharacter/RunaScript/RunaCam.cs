using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunaCam : MonoBehaviour
{
    private bool CursorVisible = true;
    public float sensitivity = 2.0f; // 마우스 감도 조절 변수
    public bool invertY = false; // Y 축 회전 반전 여부

    private float rotationX = 0.0f; // X 축 회전 각도

    void Update()
    {
        Cursors();

        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Y 축 회전 각도 계산
        rotationX -= mouseY * sensitivity * (invertY ? -1 : 1);

        // Y 축 회전 각도 제한 (원하는 각도로 조절)
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        // 카메라와 캐릭터 회전 적용
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        transform.parent.rotation *= Quaternion.Euler(0.0f, mouseX * sensitivity, 0.0f);
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
