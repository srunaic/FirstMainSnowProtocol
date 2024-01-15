using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SeonghyoGameManagerGroup; //게임매니저

public class ItemSpawner : MonoBehaviourPunCallbacks
{
    public float size = 10f;
    public GameObject itemPrefab;

    float spawnDist = 2f;
    float time = 0;

    void Update()
    {
        //멀티플레이 중이고 마스터가 아니면 
        if (GameManager.instance.isConnect == true //게임 접속하기 버튼이 눌렸을때,
            && !PhotonNetwork.IsMasterClient)
            return; //아래코드 스킵

        if (time >= spawnDist)
        {
            //생성하는 코드
            Vector3 spawnPos = transform.position;
            Vector3 addRange
                = new Vector3(Random.Range(0, size) - size * 0.5f,
                0, Random.Range(0, size) - size * 0.5f);

            photonView.RPC("SpawnItemAct", RpcTarget.All, spawnPos + addRange);

            Debug.Log("생성");
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
            //기본적으로 PhotonNetwork.Instantiate로 생성한 경우
            //주인이 나가면 사라지는 생성코드
            PhotonNetwork.InstantiateRoomObject(itemPrefab.name, _spawnPos, Quaternion.identity, 0);
            //주인이 나가도 사라지지 않는 생성코드 다음 방장이 주인이 된다.
        }
        else
        {
            Instantiate(itemPrefab, _spawnPos, Quaternion.identity);
        }
    }

}
