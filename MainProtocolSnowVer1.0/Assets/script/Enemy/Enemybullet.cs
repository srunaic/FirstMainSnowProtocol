using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemybullet : MonoBehaviour
{

    public GameObject Ex;
    private Rigidbody rigid;

    public GameObject target;
    private bool homing;
    public Vector3 FireVec;
    private float timer;

    void Update()
    {

        if (Input.GetButtonDown("Fire1")) 
        {
            timer += Time.deltaTime;

            if (homing)
            {
                FireVec = (target.transform.position - transform.position).normalized;
            }

            transform.position += FireVec * Time.deltaTime * 20f;
            transform.forward = FireVec;
        }


        timer += Time.deltaTime;

        if (timer >= 3)
        {

            Destroy(gameObject);

        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            GameObject ins = Instantiate(Ex, transform.position, transform.rotation) as GameObject;
            Destroy(gameObject, 1f);//ÆÄ±«ÇÏ¶ó

        }
 
    }
}
