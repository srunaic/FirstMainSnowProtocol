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

    #region �κ��丮�� ������ ���� ī��Ʈ ���� 
    public GameObject BlueberryCount;
    public GameObject ManduCount;   //���� ����

    public GameObject thisCount;  //���� ����

    [Header("item DB ����")]
    Item Item;
    #endregion

    [Header("�������� ���� ���� ex)�̸� ī��� ������")]
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
    private void SearchThisCount() // ������ ���� �˻� ���.
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
    //��Ƽ���� �� �������� �ı��ɶ� ���̰� �ϱ�.

    #region ��Ƽ���� ������ �����۵� ���� �� �߰� ����.

    public void StayAlone() // �������� �ı����� ����.
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
