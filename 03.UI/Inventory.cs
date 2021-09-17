using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    //필요한 컴포넌트
    [SerializeField]
    private GameObject InventoryBase;
    [SerializeField]
    private GameObject SlotParent;
    private Slot[] slots;

    public GameObject playerInfoUI; //플래이어 인포 ui 관련 변수
    public int RifleAmmoNum; // 총알 
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
    //아이템 획득
    public void AcquireItem(Inventory_Item _item )
    {//슬롯의 갯수 만큼  포문돌림
     

        //아이템 타입이 장비 아이템이 아닌경우 숫자 누적
        if(Inventory_Item.ItemType.Equipment != _item.itemType)
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

    //뭔가 이상하지만 일단 사용하자
    public void CheckRifleAmmo()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //인벤토리
            if (slots[i].item == null || slots[i].item.itemName != "Inven_Rifle_Ammo")
            {
                //Debug.Log("인벤에 총알 없음");
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
            Debug.Log("인벤에 총알 있음");
        }
       else
        {
            Debug.Log("인벤에 총알 없음");
            RifleAmmoNum = 0;
        }


    }



    public void useRifleAmmo(int UseAmmoNum)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //인벤토리
            if (slots[i].item != null && slots[i].item.itemName == "Inven_Rifle_Ammo")
            {
                slots[i].SetSlotCount(-UseAmmoNum);
                return;
            }
        }
    }

}
