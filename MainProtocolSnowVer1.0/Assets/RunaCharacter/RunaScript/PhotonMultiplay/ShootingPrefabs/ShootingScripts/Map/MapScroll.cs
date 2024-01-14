using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScroll : MonoBehaviour
{
   Renderer spriteRend;

    private float offset;
    public float speed;

    private void Start()
    {
        spriteRend = GetComponent<Renderer>();
        offset = 0;
        speed = 0.2f;
    }

    void Update()
    {
        offset = Time.time * speed;
        spriteRend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
