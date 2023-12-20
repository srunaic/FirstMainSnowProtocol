using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePlayer : MonoBehaviour
{
    [Header("������ �ӵ�")]
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

        //�Է¹����� ���� �̵��������� ����
        Vector3 moveVec = PlayerDir;
        //ī�޶� ȸ���� ��������
        Quaternion camRoty = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);//y�� Quaternion ī�޶� �ڵ忡 ����ؼ� �����ش�.
        moveVec = camRoty * moveVec;

        //�߷��� �����ǵ��� ����.
        Vector3 fVel = moveVec * movespeed;
        fVel += new Vector3(0, rb.velocity.y, 0);
        rb.velocity = fVel;

    }


}
