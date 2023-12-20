using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject Target;            // 카메라가 따라다닐 타겟

    public float offsetX = -4f;          // 카메라의 x좌표
    public float offsetY = 2f;           // 카메라의 y좌표
    public float offsetZ = 0.0f;         // 카메라의 z좌표

    public float CameraSpeed = 10.0f;    // 카메라의 속도
    Vector3 TargetPos;                   // 타겟의 위치

    //------------------------------------------------
    private float turnSpeed = 4.0f; // 마우스 회전 속도    
    private float xRotate = 0.0f; // 내부 사용할 X축 회전량은 별도 정의 ( 카메라 위 아래 방향 )
    private float moveSpeed = 4.0f; // 이동 속도
    private float rotateSpeed = 0.2f;
    private float movement;
    private int Count = 0;

    Rigidbody body; // Rigidbody를 가져올 변수

    public Transform follow; //photon 따라다닐 객체
    [SerializeField] float m_Speed;
    [SerializeField] float m_MaxRayDist = 1;
    [SerializeField] float m_Zoom = 3f;
    RaycastHit m_Hit;

    //------------------------------------------
    //카메라 방향 이동

    void Start()
    {
        body = GetComponent<Rigidbody>();             // Rigidbody를 가져온다.
    }
    void FixedUpdate()
    {
        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);


        Move();
        Zoom();
        SmoothTurn();

    }
    void Update()
    {
        Cursori();
    }

    void Zoom() //캐릭터 씬 줌.
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
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -25, 80);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        //키보드에 따른 이동량 측정
            Vector3 move =
            transform.forward * Input.GetAxis("Vertical") +
            transform.right * Input.GetAxis("Horizontal");

        // 이동량을 좌표에 반영
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

