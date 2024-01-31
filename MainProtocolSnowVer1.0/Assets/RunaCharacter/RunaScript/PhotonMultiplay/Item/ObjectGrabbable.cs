using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviourPunCallbacks
{
    [Header("포톤에서 받아올 오브젝트")]
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
            objectRigidbody.MovePosition(newPosition);//이 리지드 바디 위치를 직접 이동 시키기 가능.

            //_GetItem.StayAlone();//이 아이템을 클릭하면 습득되도록 하기.
            //_GetItem.ItemNameCount(gameObject.GetPhotonView());//아이템 카운트 갯수 증가.

        }
    }
}
