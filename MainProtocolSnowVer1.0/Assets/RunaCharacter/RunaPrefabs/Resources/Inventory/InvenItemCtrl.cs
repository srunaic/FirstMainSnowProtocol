using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSeonghyo;

public class InvenItemCtrl : MonoBehaviour
{
    [Header("������ ������ ���̽� ����")]

    [TextArea(1,10)]

    Item item;

    private Button RmmoveButton;//������ ���� ��ư

    #region ������ ���� �� �߰� ���. �κ��丮�� ���� ������.

    public void RemoveItem()
    {
        ItemManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem; // �߰��� �����ۿ� �� �Լ� ����.
    }

    #endregion
}
