using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject User;
    public GameObject explosion;//explosion ���� ������Ʈ ����.
    public GameObject Fair; //�й�

    private float Count = 0;

     void OnCollisionEnter(Collision col)//�Ϲ� �浹
    {
        if (col.gameObject.tag == "Fire" && Count == 0) //���࿡ �Ѿ��� �浹 ������,
        {
       
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject, 1f);//3.0 �� ������ �ı�

                User.GetComponent<bakhochanMove>().enabled = false; //ĳ���Ͱ� ������ ��ũ��Ʈ�� ����. 
                Fair.SetActive(true); //�¸� �ؽ�Ʈ Ȱ��ȭ.


        }
     }

    
}
