using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotation : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    //ī�޶� ����ٴϸ鼭 ��ġ�� ����.
    public Vector3 followOffset = Vector3.zero;
    private float CamMoveSpeed = 2f;
    public bool invertY = false; // Y �� ȸ�� ���� ����

    //private float rotationX = 0.5f;

    [Header("LookAt")]
    public Transform lookTarget;
    public Vector3 lookOffset = Vector3.zero;//�ٶ� ��ġ�� �����ϴ� ��.

    //ī�޶� �Ĵٺ��鼭 ȸ�� ����.
    [SerializeField]
    private float angleY = 0;//ĳ���Ͱ� ���� �����϶��� ȸ�� �� ������ ��.
    [SerializeField]
    private float angleZ = 0;//ī�޶� ���� ���� ��

    private float rotateSpeedX = 10f;
    private float rotSpeedY = 10f;

    [Header("ī�޶� ������ Ŀ����ġ")]
    private bool CursorVisible = true;
    public float sensitivity = 2.0f; // ���콺 ���� ���� ����

    private void Start()
    {   
          followTarget = GameObject.Find("CamPivot").GetComponent<Transform>();
          lookTarget = GameObject.Find("CamPivot").GetComponent<Transform>();

          transform.position =
          followTarget.position + followTarget.rotation * followOffset;//ī�޶� y������ �󸶸�ŭ ȸ���߳�?

          angleY = 0;
          angleZ = 0;

    }

    private void FixedUpdate()//������ �ֱ⸶�� ������Ʈ(�����ֱ� ,�ξ� �ʰ� �ݺ�.0.02)
    {
        if (followTarget != null) //Ÿ���� �ִٸ�, ���󰡶�.
            CamFollow();
        if (lookTarget != null)
            CamLook();
        //�����ֱ⿡ ���缭 �ݺ�.
       // CamSensitivity();
    }
    private void Update()
    {
        Cursors();
    }
    public void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");//���콺�� �����϶�,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y�� �ޱ� �ٲ�.
        angleZ += mInputY * rotSpeedY;

        Quaternion rotX = Quaternion.Euler(-angleZ, angleY, 0); //�̰� �� �����ϼ�.

        Vector3 followPos =
            followTarget.position + rotX * followOffset; //�ޱ��� �ٲ������,

        transform.position = 
            Vector3.Lerp(transform.position , followPos ,Time.deltaTime * CamMoveSpeed); //ī�޶� ������ ����.
    }
    public void CamLook()
    {
        Quaternion camRot = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0);
        
        Vector3 lookPos = lookTarget.position + camRot * lookOffset;
        
        transform.LookAt(lookPos);
    }

     
     /*void CamSensitivity()//ī�޶� ����
     {
         // ���콺 �Է� �ޱ�
         float mouseX = Input.GetAxis("Mouse X");
         float mouseY = Input.GetAxis("Mouse Y");

         // Y �� ȸ�� ���� ���
         rotationX -= mouseY * sensitivity * (invertY ? -1 : 1);

         // Y �� ȸ�� ���� ���� (���ϴ� ������ ����)
         rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

         // ī�޶�� ĳ���� ȸ�� ����
         transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
         transform.rotation *= Quaternion.Euler(0.0f, mouseX * sensitivity, 0.0f);//������.
         transform.rotation *= Quaternion.Euler(0f, mouseY * sensitivity,0f);

     }*/

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

