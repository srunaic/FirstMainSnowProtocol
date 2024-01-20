using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectGrabbable objectGrabbable;

    public float pickUpDistance = 100f;

    private void Update()
    {
      /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.position,
                    out RaycastHit raycastHit, pickUpDistance))
                {
                    Vector3 dir = Camera.main.transform.position;
                    Color color = Color.red;
                    Debug.DrawRay(Camera.main.transform.position, dir, color, pickUpDistance);

                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }
            else {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
      */


    }




}
