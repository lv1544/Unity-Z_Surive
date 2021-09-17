using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GameObject InventoryBase;
    [SerializeField]
    private GameObject SlotParent;
    private Slot[] slots;

    public GameObject playerInfoUI; //�÷��̾� ���� ui ���� ����
    public int RifleAmmoNum; // �Ѿ� 
    public bool IsRifleAmmo;

    // Start is called before the first frame update
    void Start()
    {
        slots = SlotParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        ikeyOpennven();
      //  HaveInvenRifleAmmo();
        CheckRifleAmmo();
    }

    private void ikeyOpennven()
    {
        if (Input.GetKeyDown(KeyCode.I) && !Gamemanager.isGameover)
        {
            TryOpenInventory();
        }
    }

    public void lootingEvent()
    {
        TryOpenInventory();
    }

    public void InventoryOpen()
    {
        OpenInventory();
    }

    public void InventoryClose()
    {
        CloseInventory();
    }

    private void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;

        if (inventoryActivated)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        InventoryBase.SetActive(true);
        playerInfoUI.SetActive(true);
    }


    private void CloseInventory()
    {
        InventoryBase.SetActive(false);
        playerInfoUI.SetActive(false);
    }
    //������ ȹ��
    public void AcquireItem(Inventory_Item _item )
    {//������ ���� ��ŭ  ��������
     

        //������ Ÿ���� ��� �������� �ƴѰ�� ���� ����
        if(Inventory_Item.ItemType.Equipment != _item.itemType)
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

    //���� �̻������� �ϴ� �������
    public void CheckRifleAmmo()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //�κ��丮
            if (slots[i].item == null || slots[i].item.itemName != "Inven_Rifle_Ammo")
            {
                //Debug.Log("�κ��� �Ѿ� ����");
                RifleAmmoNum = 0;
                IsRifleAmmo = false;
            }
            else
            {
                if (slots[i].item.itemName == "Inven_Rifle_Ammo")
                {
                    RifleAmmoNum = slots[i].itemCount;
                    IsRifleAmmo = true;
                    return;
                }
            }
        }
    }

    public void HaveInvenRifleAmmo()
    {
       if(IsRifleAmmo)
        {
            Debug.Log("�κ��� �Ѿ� ����");
        }
       else
        {
            Debug.Log("�κ��� �Ѿ� ����");
            RifleAmmoNum = 0;
        }


    }



    public void useRifleAmmo(int UseAmmoNum)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //�κ��丮
            if (slots[i].item != null && slots[i].item.itemName == "Inven_Rifle_Ammo")
            {
                slots[i].SetSlotCount(-UseAmmoNum);
                return;
            }
        }
    }

}
