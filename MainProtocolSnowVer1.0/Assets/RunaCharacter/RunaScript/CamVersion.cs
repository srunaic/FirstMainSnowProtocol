using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamVersion : MonoBehaviour
{
    [SerializeField]
    private GameObject Camera1;
    [SerializeField]
    private GameObject Camera2;

    public Transform follow; //photon µû¶ó´Ù´Ò °´Ã¼
    [SerializeField] float m_Speed;
    [SerializeField] float m_MaxRayDist = 1;
    [SerializeField] float m_Zoom = 3f;
    RaycastHit m_Hit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Camera2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Camera2.SetActive(false);
        }

        Zoom();

    }
    void Zoom() //Ä³¸¯ÅÍ ¾À ÁÜ.
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
            //Debug.Log("hit point : " + m_Hit.point + ", distance : " + m_Hit.distance + ", name : " + m_Hit.collider.name);
            Debug.DrawRay(cam.position, transform.forward * m_Hit.distance, Color.red);
            cam.position += new Vector3(m_Hit.point.x, 0, m_Hit.point.z);
            return false;
        }

        return true;
    }

}
