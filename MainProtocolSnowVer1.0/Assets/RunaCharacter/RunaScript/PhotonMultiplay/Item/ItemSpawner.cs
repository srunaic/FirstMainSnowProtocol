using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SeonghyoGameManagerGroup; //���ӸŴ���

public class ItemSpawner : MonoBehaviourPunCallbacks
{
    public float size = 10f;
    public GameObject itemPrefab;

    float spawnDist = 2f;
    float time = 0;

    void Update()
    {
        //��Ƽ�÷��� ���̰� �����Ͱ� �ƴϸ� 
        if (GameManager.instance.isConnect == true //���� �����ϱ� ��ư�� ��������,
            && !PhotonNetwork.IsMasterClient)
            return; //�Ʒ��ڵ� ��ŵ

        if (time >= spawnDist)
        {
            //�����ϴ� �ڵ�
            Vector3 spawnPos = transform.position;
            Vector3 addRange
                = new Vector3(Random.Range(0, size) - size * 0.5f,
                0, Random.Range(0, size) - size * 0.5f);

            photonView.RPC("SpawnItemAct", RpcTarget.All, spawnPos + addRange);

            Debug.Log("����");
            time = 0;
        }

        time += Time.deltaTime;
    }

    [PunRPC]
    public void SpawnItemAct(Vector3 _spawnPos)
    {
        if (GameManager.instance.isConnect == true)
        {
            //PhotonNetwork.Instantiate(itemPrefab.name, _spawnPos, Quaternion.identity, 0);
            //�⺻������ PhotonNetwork.Instantiate�� ������ ���
            //������ ������ ������� �����ڵ�
            PhotonNetwork.InstantiateRoomObject(itemPrefab.name, _spawnPos, Quaternion.identity, 0);
            //������ ������ ������� �ʴ� �����ڵ� ���� ������ ������ �ȴ�.
        }
        else
        {
            Instantiate(itemPrefab, _spawnPos, Quaternion.identity);
        }
    }

}
