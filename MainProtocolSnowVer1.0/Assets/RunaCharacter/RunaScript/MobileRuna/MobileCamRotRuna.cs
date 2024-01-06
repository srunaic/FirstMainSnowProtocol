using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MobileCamRotRuna : MonoBehaviour
{
    [Header("Follow")]
    public Transform followTarget;
    //ī�޶� ����ٴϸ鼭 ��ġ�� ����.
    public Vector3 followOffset = Vector3.zero;
    private float CamMoveSpeed = 1f;
    public bool invertY = false; // Y �� ȸ�� ���� ����

    [Header("LookAt")]
    public Transform lookTarget;
    public Vector3 lookOffset = Vector3.zero;//�ٶ� ��ġ�� �����ϴ� ��.

    //ī�޶� �Ĵٺ��鼭 ȸ�� ����.
    [SerializeField]
    private float angleY = 0;//ĳ���Ͱ� ���� �����϶��� ȸ�� �� ������ ��.
    private float rotateSpeedX = 0.2f;

    [Header("ī�޶� ������ Ŀ����ġ")]
    private bool CursorVisible = true;
 
    private void Start()
    {
        transform.position =
            followTarget.position + followTarget.rotation * followOffset;//ī�޶� y������ �󸶸�ŭ ȸ���߳�?

        angleY = 0;

    }
    private void FixedUpdate()//������ �ֱ⸶�� ������Ʈ(�����ֱ� ,�ξ� �ʰ� �ݺ�.0.02)
    {
        if (followTarget != null) //Ÿ���� �ִٸ�, ���󰡶�.
            CamFollow();
    }
    private void Update()
    {
        if (followTarget != null) // �÷��̾ �����Ѵٸ�
        {
            Vector3 newPosition = followTarget.position; // �÷��̾��� ��ġ�� ������
            newPosition.y = transform.position.y; // ī�޶��� ���̸� �����ϱ� ���� Y ��ǥ�� ����

            transform.position = newPosition; // ī�޶��� ��ġ�� ������Ʈ�Ͽ� �÷��̾ ���󰡵��� ��
        }
    }
    public void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");//���콺�� �����϶�,
        float mInputY = Input.GetAxis("Mouse Y");

        angleY += mInputX * rotateSpeedX; //y�� �ޱ� �ٲ�.

        Quaternion rotX = Quaternion.Euler(0, angleY,0);

        Vector3 followPos =
            followTarget.position + rotX * followOffset; //�ޱ��� �ٲ������,
       
        transform.position =
            Vector3.Lerp(transform.position, followPos, Time.deltaTime * CamMoveSpeed);
    }
 
}
