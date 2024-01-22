using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    public Transform objectGrabPointTransform; //this Transform null

    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        objectGrabPointTransform = GetComponent<Transform>();
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);//�� ������ �ٵ� ��ġ�� ���� �̵� ��Ű�� ����.
        }
    }
}
