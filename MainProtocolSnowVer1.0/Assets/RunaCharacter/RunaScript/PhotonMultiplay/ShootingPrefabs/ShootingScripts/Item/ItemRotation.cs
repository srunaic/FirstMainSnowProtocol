using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    public float RotationSpeed = 30.0f;
    void Update()
    {
        RuteItem();
    }
    void RuteItem()
    {
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }
}
