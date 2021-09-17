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

    //�ʿ��� ������Ʈ
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

        // �κ� ���� ���ÿ�����Ʈ ���ֱ�
        lootingObject.items = null;
         //
        InventoryBase.SetActive(true);
      
    }


    private void CloseInventory()
    {
        //���� �κ��丮 �ȿ� ������ ����
        checkItemNum();
        // ������ ������ //������ ���ڸ�ŭ ���ÿ�����Ʈ ������ �迭�� �Ҵ��ϱ�
        if (itemNum != 0)
        {
            //������ �����ϱ�
            SortItem();
            //������ �迭�����
            InputItem();

            //������ ���ڸ�ŭ �迭 �Ҵ��ϱ�
            lootingObject.items = looting_items;
        }
        else
        {
            itemNum = 0;
            if(lootingObject != null)
            lootingObject.items = null;
        }
        

        //���� �κ��丮 ���� ����
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = null;
            slots[i].itemCount = 0;
        }


        InventoryBase.SetActive(false);
        
    }
    //������ ȹ��
    public void AcquireItem(Inventory_Item _item)
    {//������ ���� ��ŭ  ��������


        //������ Ÿ���� ��� �������� �ƴѰ�� ���� ����
        if (Inventory_Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                //�̹� �κ��丮�� �ִ� �������� ���
                if (slots[i].item != null && slots[i].item.itemName == _item.itemName)
                {
                    slots[i].SetSlotCount(_item.itemNum);
                    return;
                }
            }

        }
        for (int i = 0; i < slots.Length; i++)
        {
            //�󽽷��� ���
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
            //�̹� �κ��丮�� �ִ� �������� ���
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
            //�̹� �κ��丮�� �ִ� �������� ���
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
                //�̹� �κ��丮�� �ִ� �������� ���
                if (slots[i].item != null)
                {
                    inventory.AcquireItem(slots[i].item);
                    slots[i].ClearSlot();
                }
            }
        }
    }

}
