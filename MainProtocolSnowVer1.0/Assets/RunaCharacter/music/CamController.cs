using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject Target;            // ī�޶� ����ٴ� Ÿ��

    public float offsetX = -4f;          // ī�޶��� x��ǥ
    public float offsetY = 2f;           // ī�޶��� y��ǥ
    public float offsetZ = 0.0f;         // ī�޶��� z��ǥ

    public float CameraSpeed = 10.0f;    // ī�޶��� �ӵ�
    Vector3 TargetPos;                   // Ÿ���� ��ġ

    //------------------------------------------------
    private float turnSpeed = 4.0f; // ���콺 ȸ�� �ӵ�    
    private float xRotate = 0.0f; // ���� ����� X�� ȸ������ ���� ���� ( ī�޶� �� �Ʒ� ���� )
    private float moveSpeed = 4.0f; // �̵� �ӵ�
    private float rotateSpeed = 0.2f;
    private float movement;
    private int Count = 0;

    Rigidbody body; // Rigidbody�� ������ ����

    public Transform follow; //photon ����ٴ� ��ü
    [SerializeField] float m_Speed;
    [SerializeField] float m_MaxRayDist = 1;
    [SerializeField] float m_Zoom = 3f;
    RaycastHit m_Hit;

    //------------------------------------------
    //ī�޶� ���� �̵�

    void Start()
    {
        body = GetComponent<Rigidbody>();             // Rigidbody�� �����´�.
    }
    void FixedUpdate()
    {
        // Ÿ���� x, y, z ��ǥ�� ī�޶��� ��ǥ�� ���Ͽ� ī�޶��� ��ġ�� ����
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        // ī�޶��� �������� �ε巴�� �ϴ� �Լ�(Lerp)
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);


        Move();
        Zoom();
        SmoothTurn();

    }
    void Update()
    {
        Cursori();
    }

    void Zoom() //ĳ���� �� ��.
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Transform cam = Camera.main.transform;
            if (CheckRay(cam, scroll))
            {
                Vector3 targetDist = cam.transform.position - follow.transform.position;
                targetDist = Vector3.Normalize(targetDist);
                Camera.main.transform.position -= (targetDist * scroll * m_Zoom);
            }
        }

        Camera.main.transform.LookAt(follow.transform);
    }
    bool CheckRay(Transform cam, float scroll)
    {
        if (Physics.Raycast(cam.position, transform.forward, out m_Hit, m_MaxRayDist))
        {
            Debug.Log("hit point : " + m_Hit.point + ", distance : " + m_Hit.distance + ", name : " + m_Hit.collider.name);
            Debug.DrawRay(cam.position, transform.forward * m_Hit.distance, Color.red);
            cam.position += new Vector3(m_Hit.point.x, 0, m_Hit.point.z);
            return false;
        }

        return true;
    }
    void Move()
    {
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        // Clamp �� ���� ������ �����ϴ� �Լ�
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -25, 80);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        //Ű���忡 ���� �̵��� ����
            Vector3 move =
            transform.forward * Input.GetAxis("Vertical") +
            transform.right * Input.GetAxis("Horizontal");

        // �̵����� ��ǥ�� �ݿ�
        transform.position += move * moveSpeed * Time.deltaTime;
        transform.RotateAround(Vector3.zero, Vector3.back, movement * Time.fixedDeltaTime * moveSpeed);
    }

    void SmoothTurn()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, v, 0.0f);

        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, angle), rotateSpeed * Time.deltaTime);
        }
    }
    void Cursori()
    {

        if (Count == 0 && Input.GetKeyDown(KeyCode.X))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;


            Count = 1;
        }

        else if (Count == 1 && Input.GetKeyDown(KeyCode.X))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Count = 0;
        }

    }
}

