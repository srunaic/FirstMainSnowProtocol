using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviourPunCallbacks
{
    [Header("���濡�� �޾ƿ� ������Ʈ")]
    private Rigidbody objectRigidbody;
    public Transform objectGrabPointTransform; //this Transform null

    public Rigidbody rb;

    //public ItemPickUp _GetItem;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        objectGrabPointTransform = GetComponent<Transform>();
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //_GetItem = FindObjectOfType<ItemPickUp>();
    }

    private void FixedUpdate()
    {
        if(objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);//�� ������ �ٵ� ��ġ�� ���� �̵� ��Ű�� ����.

            //_GetItem.StayAlone();//�� �������� Ŭ���ϸ� ����ǵ��� �ϱ�.
            //_GetItem.ItemNameCount(gameObject.GetPhotonView());//������ ī��Ʈ ���� ����.

        }
    }
}
