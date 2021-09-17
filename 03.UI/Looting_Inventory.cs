using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looting_Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    public Inventory inventory;
    public LootingObject lootingObject;
    public Text lootingObjectName;
    public int itemNum = 0;

    public Inventory_Item[] looting_items;

    //필요한 컴포넌트
    [SerializeField]
    private GameObject InventoryBase;
    [SerializeField]
    private GameObject SlotParent;
    private Slot[] slots;


    // Start is called before the first frame update
    void Start()
    {
       
        slots = SlotParent.GetComponentsInChildren<Slot>();
       


    }

    


    // Update is called once per frame
    void Update()
    {
        quitLootInventory();
        showObjectName();
        MoveAllitem();
    }

    void showObjectName()
    {
        if(lootingObject != null)
        lootingObjectName.text = lootingObject.gameObject.name;
    }

    public void quitLootInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) && !Gamemanager.isGameover)
        {
            CloseInventory();
        }
    }


    public void TryOpenInventory()
    {
        // OpenInventory();
        inventoryActivated = !inventoryActivated;

        if (inventoryActivated)
            OpenInventory();
        else
            CloseInventory();


    }

    public void TryClose()
    {
        InventoryBase.SetActive(false);
    }

    private void OpenInventory()
    {
        lootingObject.IsBeforeOpen = true;

        if (lootingObject.items != null)
        itemNum = lootingObject.items.Length;

        if(itemNum != 0)
        {
            for (int i = 0; i < itemNum; i++)
            {
                AcquireItem(lootingObject.items[i]);
            }
      
        }

        // 인벤 열때 루팅오브젝트 없애기
        lootingObject.items = null;
         //
        InventoryBase.SetActive(true);
      
    }


    private void CloseInventory()
    {
        //루팅 인벤토리 안에 아이템 세기
        checkItemNum();
        // 아이템 있으면 //아이템 숫자만큼 루팅오브젝트 아이템 배열에 할당하기
        if (itemNum != 0)
        {
            //아이템 정렬하기
            SortItem();
            //아이템 배열만들기
            InputItem();

            //아이템 숫자만큼 배열 할당하기
            lootingObject.items = looting_items;
        }
        else
        {
            itemNum = 0;
            if(lootingObject != null)
            lootingObject.items = null;
        }
        

        //루팅 인벤토리 슬롯 비우기
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = null;
            slots[i].itemCount = 0;
        }


        InventoryBase.SetActive(false);
        
    }
    //아이템 획득
    public void AcquireItem(Inventory_Item _item)
    {//슬롯의 갯수 만큼  포문돌림


        //아이템 타입이 장비 아이템이 아닌경우 숫자 누적
        if (Inventory_Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                //이미 인벤토리에 있는 아이템인 경우
                if (slots[i].item != null && slots[i].item.itemName == _item.itemName)
                {
                    slots[i].SetSlotCount(_item.itemNum);
                    return;
                }
            }

        }
        for (int i = 0; i < slots.Length; i++)
        {
            //빈슬롯인 경우
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _item.itemNum);
                return;
            }
        }

    }

    void checkItemNum()
    {
        itemNum = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            //이미 인벤토리에 있는 아이템인 경우
            if (slots[i].item != null)
            {
                itemNum += 1;
            }
        }
    }

    void InputItem()
    {

       looting_items = new Inventory_Item[itemNum];


        for (int i = 0; i < slots.Length; i++)
        {
            //이미 인벤토리에 있는 아이템인 경우
            if (slots[i].item != null && i <= itemNum)
            {
                looting_items[i] = slots[i].item;
            }
        }
    }

    void SortItem()
    {
        for (int i = 0; i < slots.Length-1; i++)
        {
            //
            if (slots[i].item == null && slots[i+1].item !=null)
            {
                slots[i].item = slots[i + 1].item;
                slots[i + 1].ClearSlot();
            }
        }
    }

    void MoveAllitem()
    {
        if (Input.GetKeyDown(KeyCode.A) && !Gamemanager.isGameover)
        {

            for (int i = 0; i < slots.Length; i++)
            {
                //이미 인벤토리에 있는 아이템인 경우
                if (slots[i].item != null)
                {
                    inventory.AcquireItem(slots[i].item);
                    slots[i].ClearSlot();
                }
            }
        }
    }

}
