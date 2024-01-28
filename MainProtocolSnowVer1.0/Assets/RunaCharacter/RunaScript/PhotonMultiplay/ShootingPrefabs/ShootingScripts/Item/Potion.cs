using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Potion : MonoBehaviour
{ 
    public int Healing = 10;

    public float endHeight = 0;
    public float spawnHeight = 0;
    public float flowSpeed = 1.0f;//이동속도
    public float rotateSpeed = 20f;//회전 속도

    private void Start()
    {
        StartCoroutine(PotionDestroy());
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject, 0.2f);
        }
    }

    IEnumerator PotionDestroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

}
