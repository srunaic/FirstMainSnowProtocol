using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace ItemSeonghyo
{
    public class ItemManager : MonoBehaviourPunCallbacks
    {
        [Header("멀티에서 넘겨받을 인벤토리 서로 공유가능 거래 시스템.")]
        public static ItemManager Instance;

        public List<Item> Items = new List<Item>(); //아이템 슬롯들 자료구조 데이터베이스 관리형.

        [Header("인벤토리 UI 슬롯")]
        public Transform ItemContent; //슬롯의 부모UI 인벤토리 
        public GameObject InventoryItem;//인벤 아이템 슬롯

        public InvenItemCtrl[] InventoryItems; //InvenItem 배열 객체 저장

        private void Awake()
        {
            Instance = this;
        }

        #region 아이템 인벤에 추가 및 제거

        public void Add(Item item)
        {
            Items.Add(item);
        }
        public void Remove(Item item)
        {
            Items.Remove(item);
        }

        //아이템을 인벤토리에 불러왔다면 밑에 갱신 및 아이템을 찾아라.
        public void ListItem()
        {
            foreach (Transform item in ItemContent) //인벤토리
            {
                Destroy(item.gameObject);//아이템 제거
            }

            foreach (var item in Items) //아이템 추가 및 생성
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);

                //슬롯에 포함된 하위 폴더 이름
                var itemName = obj.transform.Find("itemName").GetComponent<Text>();
                var itemicon = obj.transform.Find("ItemIcon").GetComponent<Image>();
                var removeButton = obj.transform.Find("RemoveBtn").GetComponent<Button>();
                var aItemCount = obj.transform.Find("CountText").GetComponent<Text>();

                itemName.text = item.ItemName;//이름
                itemicon.sprite = item.icon;//이미지
                aItemCount.text = item.count.ToString();// 아이템 카운트 

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