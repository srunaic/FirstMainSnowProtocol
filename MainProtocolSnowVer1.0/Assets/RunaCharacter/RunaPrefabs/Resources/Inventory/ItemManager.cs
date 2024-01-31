using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace ItemSeonghyo
{
    public class ItemManager : MonoBehaviourPunCallbacks
    {
        [Header("��Ƽ���� �Ѱܹ��� �κ��丮 ���� �������� �ŷ� �ý���.")]
        public static ItemManager Instance;

        public List<Item> Items = new List<Item>(); //������ ���Ե� �ڷᱸ�� �����ͺ��̽� ������.

        [Header("�κ��丮 UI ����")]
        public Transform ItemContent; //������ �θ�UI �κ��丮 
        public GameObject InventoryItem;//�κ� ������ ����

        public InvenItemCtrl[] InventoryItems; //InvenItem �迭 ��ü ����

        private void Awake()
        {
            Instance = this;
        }

        #region ������ �κ��� �߰� �� ����

        public void Add(Item item)
        {
            Items.Add(item);
        }
        public void Remove(Item item)
        {
            Items.Remove(item);
        }

        //�������� �κ��丮�� �ҷ��Դٸ� �ؿ� ���� �� �������� ã�ƶ�.
        public void ListItem()
        {
            foreach (Transform item in ItemContent) //�κ��丮
            {
                Destroy(item.gameObject);//������ ����
            }

            foreach (var item in Items) //������ �߰� �� ����
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);

                //���Կ� ���Ե� ���� ���� �̸�
                var itemName = obj.transform.Find("itemName").GetComponent<Text>();
                var itemicon = obj.transform.Find("ItemIcon").GetComponent<Image>();
                var removeButton = obj.transform.Find("RemoveBtn").GetComponent<Button>();
                var aItemCount = obj.transform.Find("CountText").GetComponent<Text>();

                itemName.text = item.ItemName;//�̸�
                itemicon.sprite = item.icon;//�̹���
                aItemCount.text = item.count.ToString();// ������ ī��Ʈ 

            }
            SetInventoryItems();
        }
        public void SetInventoryItems()
        {
            InventoryItems = ItemContent.GetComponentsInChildren<InvenItemCtrl>();
            for (int i = 0; i < Items.Count; i++)
            {
                InventoryItems[i].AddItem(Items[i]);
            }
        }

    }
    #endregion
}