using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInterActive : MonoBehaviour
{//1.���� ���� ���´�
 //2.�浹ü ��Ʈ��
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
