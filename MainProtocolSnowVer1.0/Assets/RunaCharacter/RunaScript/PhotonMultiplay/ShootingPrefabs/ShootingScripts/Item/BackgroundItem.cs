using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundItem : MonoBehaviour
{
    public float endHeight = 0;
    public float spawnHeight = 0;

    public float flowSpeed = 1.0f;//이동속도
    public float rotateSpeed = 20f;//회전 속도

    void Update()
    {
        transform.position += Vector3.down * flowSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        if (transform.position.y < endHeight) 
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = spawnHeight;
            transform.position = spawnPos;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerMove>().SetShotLevel(1);
            Destroy(gameObject);
        }
    }

}
