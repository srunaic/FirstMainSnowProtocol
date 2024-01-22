using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;

    [SerializeField]
    private ObjectGrabbable objectGrabbable;
    public bool CheckObject = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) //Grab object is null;
        {
            OnRender();
        }
        else
        {
            objectGrabPointTransform = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ObjectGrabbable>(out ObjectGrabbable Item))
        {
            objectGrabbable = Item;

            if (objectGrabbable != null)
            {
                objectGrabbable.rb.useGravity = false;
                objectGrabbable.rb.isKinematic = true;
                objectGrabPointTransform = objectGrabbable.objectGrabPointTransform;
                objectGrabPointTransform = GetComponent<Transform>();
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (objectGrabbable != null)
        {
            objectGrabPointTransform = null;
            objectGrabbable.rb.useGravity = true;
            objectGrabbable.rb.isKinematic = false;
        }
        
    }

    private void OnRender()
    {
            //�� ������Ʈ Ʈ�������� ã����..
            if (objectGrabPointTransform != null)
            {
                objectGrabbable.objectGrabPointTransform.position = objectGrabPointTransform.position;
                Debug.Log("��ƴ�� ������Ʈ �߰���" + objectGrabPointTransform);
          
            }
   
    }
    
}
