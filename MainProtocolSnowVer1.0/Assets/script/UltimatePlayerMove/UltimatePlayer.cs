using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePlayer : MonoBehaviour
{
    [Header("움직임 속도")]
    public float movespeed = 1f;
    public Rigidbody rb;

    public Transform camTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        float XInput = Input.GetAxisRaw("Horizontal");
        float ZInput = Input.GetAxisRaw("Vertical");
      
        Vector3 PlayerDir = new Vector3(XInput, 0f, ZInput).normalized;
       
        rb.velocity = PlayerDir * movespeed;

        //입력방향을 실제 이동방향으로 가공
        Vector3 moveVec = PlayerDir;
        //카메라 회전을 기준으로
        Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y축 Quaternion 카메라 코드에 계산해서 맞춰준다.
        moveVec = camRoty * moveVec;

        //중력이 누적되도록 가공.
        Vector3 fVel = moveVec * movespeed;
        fVel += new Vector3(0, rb.velocity.y, 0);
        rb.velocity = fVel;

    }


}
