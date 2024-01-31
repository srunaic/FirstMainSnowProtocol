using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSeonghyo;

public class InvenItemCtrl : MonoBehaviour
{
    [Header("아이템 데이터 베이스 참조")]

    [TextArea(1,10)]

    Item item;

    private Button RmmoveButton;//아이템 제거 버튼

    #region 아이템 제거 및 추가 기능. 인벤토리에 직접 참조형.

    public void RemoveItem()
    {
        ItemManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem; // 추가될 아이템에 이 함수 실행.
    }

    #endregion
}
