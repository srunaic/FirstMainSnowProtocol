using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SongRoom : MonoBehaviour
{
    private Transform _songRoomPos;

    public Animator _Songanim;
    public BoxCollider col;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        col.isTrigger = false;
        _Songanim = GetComponent<Animator>();
    }
}
