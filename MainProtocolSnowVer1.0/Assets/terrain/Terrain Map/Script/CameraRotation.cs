using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotation : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    //ī�޶� ����ٴϸ鼭 ��ġ�� ����.
    public Vector3 followOffset = Vector3.zero;
    private float CamMoveSpeed = 5f;
    public bool invertY = false; // Y �� ȸ�� ���� ����
    private float rotationX = 0.2f; // X �� ȸ�� ����

    [Header("LookAt")]
    public Transform lookTarget;
    public Vector3 lookOffset = Vector3.zero;//�ٶ� ��ġ�� �����ϴ� ��.

    //ī�޶� �Ĵٺ��鼭 ȸ�� ����.
    [SerializeField]
    private float angleY = 0;//ĳ���Ͱ� ���� �����϶��� ȸ�� �� ������ ��.
    [SerializeField]
    private float angleZ = 0;

    [Header("�ޱ� �ӵ�")]
    public float rotateSpeedX = 30f;
    public float rotSpeedY = 30f;

    [Header("ī�޶� ������ Ŀ����ġ")]
    private bool CursorVisible = true;
    public float sensitivity = 2.0f; // ���콺 ���� ���� ����


    private void Start()
    {
        transform.position = 
            followTarget.position + followTarget.rotation * followOffset;//ī�޶� y������ �󸶸�ŭ ȸ���߳�?
            
        angleY = 0;
        angleZ = 0;

    }
    private void LateUpdate()//���� ������Ʈ ��� ������Ʈ�� ������ �����.
    { 
    }
    private void FixedUpdate()//������ �ֱ⸶�� ������Ʈ(�����ֱ� ,�ξ� �ʰ� �ݺ�.0.02)
    {
        if (followTarget != null) //Ÿ���� �ִٸ�, ���󰡶�.
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
        float mInputX = Input.GetAxis("Mouse X");//���콺�� �����϶�,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y�� ���콺�� ��鶧���� �ޱ� �ٲ�.
        angleZ += mInputY * rotSpeedY;

        Quaternion rotX = Quaternion.Euler(0, angleY, angleZ);//�ޱ� ��.

        angleY = Mathf.Clamp(angleY,-180f,180f);//Y�� ī�޶� ��������.
       
        Vector3 followPos =
            followTarget.position + rotX * followOffset; //�ޱ��� �ٲ������,

        transform.position = 
            Vector3.Lerp(transform.position , followPos ,Time.deltaTime * CamMoveSpeed); //���ư��� �Լ�.

        rotationX -= mInputY * sensitivity * (invertY ? -1 : 1);

        // ī�޶�� ĳ���� ȸ���� ī�޶� ���� ����.
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        transform.rotation *= Quaternion.Euler(0.0f, mInputX * sensitivity, 0.0f);//������.
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

