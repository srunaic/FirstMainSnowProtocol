using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunaCam : MonoBehaviour
{
    private bool CursorVisible = true;
    public float sensitivity = 2.0f; // ���콺 ���� ���� ����
    public bool invertY = false; // Y �� ȸ�� ���� ����

    private float rotationX = 0.0f; // X �� ȸ�� ����

    void Update()
    {
        Cursors();

        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Y �� ȸ�� ���� ���
        rotationX -= mouseY * sensitivity * (invertY ? -1 : 1);

        // Y �� ȸ�� ���� ���� (���ϴ� ������ ����)
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        // ī�޶�� ĳ���� ȸ�� ����
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        transform.parent.rotation *= Quaternion.Euler(0.0f, mouseX * sensitivity, 0.0f);
    }
    private void Cursors()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            CursorVisible = !CursorVisible; // Ŀ�� ���¸� ����մϴ�.

            // Ŀ�� ǥ�� ���� ����
            Cursor.visible = CursorVisible;

            // Ŀ�� ��� ���� ���� (Ŀ���� ��׸� ���콺�� ȭ�� �ٱ����� ������ �ʽ��ϴ�)
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
