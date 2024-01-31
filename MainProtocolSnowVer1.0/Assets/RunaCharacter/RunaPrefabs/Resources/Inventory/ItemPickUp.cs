using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ItemSeonghyo;
using static UnityEditor.Progress;

public class ItemPickUp : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

    #region 인벤토리에 앞으로 들어올 카운트 갯수 
    public GameObject BlueberryCount;
    public GameObject ManduCount;   //과거 갯수

    public GameObject thisCount;  //현재 갯수

    [Header("item DB 참조")]
    Item Item;
    #endregion

    [Header("아이템을 들고올 조건 ex)이름 카운드 갯수등")]
    private string objName = null;
    private int objCount = 0;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        objName = this.gameObject.name.Replace("(Clone)","");
        thisCount.GetComponent<Text>().text = objCount.ToString();
        if (PhotonNetwork.IsConnected)
        {
            SearchThisCount();

        }
    }
    private void SearchThisCount() // 아이템 종류 검색 기능.
    {
        Debug.Log("Search" + objName);

        switch (objName)
        {
            case "Blueberry":
                thisCount = BlueberryCount;
                break;
            case "Mandu":
                thisCount = ManduCount;
                break;

            default:
                break;
        }

    }
    //멀티에서 이 아이템이 파괴될때 보이게 하기.

    #region 멀티에서 보여질 아이템들 제거 및 추가 관리.

    public void StayAlone() // 아이템이 파괴당할 조건.
    {
        PhotonNetwork.Destroy(gameObject);
    }
    private void AddItems(PhotonView playerViewID)
    {
        if (playerViewID.IsMine)
        {
            ItemManager.Instance.Add(Item);
        }
    }

    public void ItemNameCount(PhotonView playerViewID)
    {
        if(playerViewID.IsMine)
        {
            objCount++;
            thisCount.GetComponent<Text>().text = objCount.ToString();
        }
    }

    #endregion

}
