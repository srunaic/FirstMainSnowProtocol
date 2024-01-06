using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInterActive : MonoBehaviour
{//1.문을 빼서 놓는다
 //2.충돌체 컨트롤
    public Animator BuildAnim;
    public BoxCollider col;

    private void Awake()
    {
       col = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        col.isTrigger = false;
        BuildAnim = GetComponent<Animator>();
    }

}
