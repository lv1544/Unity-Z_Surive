using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //배치된 슬롯의 상태를 구분하는 enum
    public enum SlotState { InvenSlot, QuickSlot , EquipSlot ,LootingSlot};
    public SlotState slotState;

    public Inventory_Item item; // 획득한 아이템
    public int itemCount; // 아이템 갯수
    public Image itemImage;  //  아이템의 이미지
    public Image EquipSlotImage;  // 장비슬롯의 이미지
    public Inventory_Item Rifle_Ammo;

    [SerializeField]
    private PlayerInfo playerInfo;
    [SerializeField]
    private Text Count_text;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private PlayerMouse playerMouse;

    public Inventory inventory; // 인벤토리에 접근할수 있도록
    private bool StartInven;
    public Transform DropPoint; //아이템 버려지는 위치
    public GameObject[] DropItems; // 드랍아이템
    public bool IsItem;
    private bool IsChangeWeapon;
    int tap;// 더블클릭용 

    //시작시 초기화
    void Start()
    {
       

    }




    private void Update()
    {
      UpdateEquipSlot();
    }

    //이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    private void SetEquipImage(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        EquipSlotImage.color = color;
    }


    //아이템 획득
    public void AddItem(Inventory_Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;


        if (item.itemType != Inventory_Item.ItemType.Equipment)
        {
            Count_text.text = itemCount.ToString();
            SetColor(1);
        }
        else
        {
            Count_text.text = "";
            SetColor(1);
        }

    }

    public void useItem()
    {    //아이템의 존재여부
        if (item != null)
        {   // 어떤 아이템 타입인가
            switch (item.itemType)
            {
                case Inventory_Item.ItemType.Equipment: //장착아이템
                    //무기를 안들고 있을때
                    if (!weaponManager.HasWeapon)
                    {
                        //인벤 슬롯 아이템일 경우
                        if (slotState == SlotState.InvenSlot)
                        {
                            weaponManager.Equipslots[0].AddItem(item, itemCount); // 장비슬롯에 아이콘 넣고 
                            weaponManager.EquipWeapon(item.itemName);//장착하고
                            weaponManager.IsInvenItem = true;
                            IsChangeWeapon = true;
                            ClearSlot();
                        }
                        //퀵슬롯일 경우
                        else if (slotState == SlotState.QuickSlot)
                        {
                            weaponManager.Equipslots[0].AddItem(item, itemCount); // 장비에 아이콘 넣고 
                            weaponManager.EquipWeapon(item.itemName);//장착하고
                            weaponManager.IsInvenItem = false;
                            IsChangeWeapon = false;
                        }
                    }
                    else //이미 무기를 들고 있을때
                    {
                        //여기안에서 분기를 타야함
                        if (slotState == SlotState.EquipSlot)//장비 슬롯 선택하면 무기를 벗어야하고 
                        {
                            //아이템을 인벤토리에 다시 복귀 시킴
                            if (weaponManager.IsInvenItem == true)
                                inventory.AcquireItem(item);
                            ClearSlot();
                        }
                        if (slotState == SlotState.InvenSlot)//인벤슬롯 사용
                        {
                            //장비아이템일 경우
                            if (item.itemType == Inventory_Item.ItemType.Equipment)
                                SwitchItem();
                        }
                        if (slotState == SlotState.QuickSlot) // 퀵슬롯 사용
                        {
                            if (item.itemType == Inventory_Item.ItemType.Equipment)
                            {
                                if(weaponManager.Equipslots[0].item != item)
                                {
                                    weaponManager.Equipslots[0].ClearSlot();//장비슬롯을비운다
                                    weaponManager.Equipslots[0].AddItem(item);
                                    weaponManager.EquipWeapon(item.itemName);
                                }
                                if (weaponManager.Equipslots[0].item == item)
                                {
                                    weaponManager.Equipslots[0].ClearSlot();//장비슬롯을비운다
                                }

                               
                            }
                        }
                    }
                    break;
                case Inventory_Item.ItemType.Used://소모 아이템

                    //체력이 최대체력보다 줄어야 아이템 사용 가능
                    if (playerInfo.health < playerInfo.maxHealth)
                    {
                        Debug.Log(item.itemName + "을 사용했습니다" + " " + playerInfo.health);
                        playerInfo.RestoreHealth(20f);
                        SetSlotCount(-1);
                    }
                    break;
                case Inventory_Item.ItemType.Ingredient://재료 아이템
                    break;
                case Inventory_Item.ItemType.AMMO:// 총알 아이템
                    break;
            }
        }
    }




    void AddEqiupSlot(Inventory_Item _item, int _count = 1)
    {
        weaponManager.Equipslots[0].AddItem(_item, _count);
    }


    void UpdateEquipSlot()
    {
        //장비슬롯 일경우 
        if(slotState == SlotState.EquipSlot)
        {
            if (item != null && IsItem == false)
            {
                //아이템 보유 여부를 참으로 전환
                IsItem = true;
                if (EquipSlotImage != null)
                {
                    SetEquipImage(0);
                }


            }
            else if (item == null && IsItem == true)
            {
                IsItem = false;
                SetEquipImage(0.2f);
            }
        }
    }


    //아이템 갯수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Count_text.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();

    }
    //슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        Count_text.text = "";
    }

    void SwitchItem()
    {
        inventory.AcquireItem(weaponManager.Equipslots[0].item);//장비슬롯의 아이템을 인벤에 추가한다
        weaponManager.Equipslots[0].ClearSlot();//장비슬롯을비운다
        weaponManager.Equipslots[0].AddItem(item);
        weaponManager.EquipWeapon(item.itemName);
        ClearSlot();
    }

    IEnumerator SwitchItemCo()
    {        
        inventory.AcquireItem(weaponManager.Equipslots[0].item);//장비슬롯의 아이템을 인벤에 추가한다
        weaponManager.Equipslots[0].ClearSlot();//장비슬롯을비운다
        weaponManager.Equipslots[0].AddItem(item);
        yield return new WaitForSeconds(1f);
        weaponManager.EquipWeapon(item.itemName);
        ClearSlot();
    }


    //더블클릭 이벤트 아이템 장착
    public void OnPointerClick(PointerEventData eventData)
    {
        tap = eventData.clickCount;

        if (tap == 2)
        {
            useItem();
        }
    }





    //드래그가 시작됐을때
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
           // dragStartPos = eventData.position;// 시작위치 받아옴
            //CheckDragStartPos(); // 시작위치를 통해 어디서드래그 시작됬는지 알게됨
            DragSlot.instance.dragSlot = this; // 드래그 슬롯의 비어있는 것을 채운다
            DragSlot.instance.DragSetImage(itemImage); //슬롯에 아이템 이미지를 받는다
            DragSlot.instance.transform.position = eventData.position; //이미지가 따라온다


            if (slotState == SlotState.InvenSlot)
            {
                DragSlot.instance.startSlotInfo = DragSlot.startSlotState.InvenSlot;
                Debug.Log("인벤에서 시작");
            }
            else if (slotState == SlotState.QuickSlot)
            {
                DragSlot.instance.startSlotInfo = DragSlot.startSlotState.QuickSlot;
                Debug.Log("퀵슬롯에서 시작");
            }
            else if (slotState == SlotState.EquipSlot)
            {
                DragSlot.instance.startSlotInfo = DragSlot.startSlotState.EquipSlot;
                Debug.Log("장비슬롯에서 시작");
            }
        }
    }

    //드래그 중일때
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;

        playerMouse.IsItemDrag =true;

        Debug.Log("드래그 중일때");
    }
    //드래그가 끝났을때
    public void OnEndDrag(PointerEventData eventData)
    {    //땅에다 드랍한 경우
        playerMouse.IsItemDrag = false ;

        if (playerMouse.nowMouseOn == PlayerMouse.MouseCheck.NULL)
        {
            //인벤 아이템 버리기
            if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot)
            {
                //아이템을 장착하면 벗고
                if (WeaponManager.currentWeapon != null)
                {
                    weaponManager.weaponRelease();

                }
                //게임아이템 버리기
                GameObject instantDropItem = Instantiate(DropItems[0], DropPoint.position, DropPoint.rotation);


                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;

            }
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot)
            {//퀵슬롯에있던 거는 퀵슬롯에서 제거
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }//장비슬롯의 아이템을 인벤에 추가한다
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.EquipSlot)
            {
                inventory.AcquireItem(weaponManager.Equipslots[0].item);
  
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }

        }
        else// ui에다 드랍한경우 
        {
            //드래그가 끝나면 슬롯에서 사라진다
            //DragSlot.instance.dragSlot.ClearSlot();
    
            if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot)
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot)
            {//퀵슬롯에있던 거는 아
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.EquipSlot)
            {
                inventory.AcquireItem(weaponManager.Equipslots[0].item);//장비슬롯의 아이템을 인벤에 추가한다
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }

        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
        //    DropPos = eventData.position;// 드랍위치 업데이트되는 곳
                                         //Debug.Log("Mouse Position : " + eventData.position);
                                         //CheckDropEndPos(); // 드랍위치를 통해서 드랍되는 곳이 인벤인지 퀵슬롯인지 알게됨

       
     
            ChangeSlot();
        }
    }

    //드랍해서 아이템 옮기기  조금 문제가 있다..
    private void ChangeSlot()
    {
        Inventory_Item _tempItem = item;
        int _tempCount = itemCount;

        //확인용
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot)
        {
            Debug.Log("인벤슬롯에서 시작");
        }
         else
        {
            Debug.Log("퀵슬롯에서 시작");
        }

        //드래그 시작위치가 인벤이고 드랍도 인벤일때
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot && slotState == SlotState.InvenSlot)
        {
            // Debug.Log("드래그 시작위치가 인벤이고 드랍도 인벤일때");
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            if (_tempItem != null)
            {
                DragSlot.instance.dragSlot.AddItem(_tempItem, _tempCount);
            }
            else
            {
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }

        //드래그 시작위치가 퀵슬롯이고 드랍도 퀵슬롯일때
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot && slotState == SlotState.QuickSlot)
        {
            Debug.Log("드래그 시작위치가 퀵슬롯이고 드랍도 퀵슬롯일때");
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            if (_tempItem != null)
            {
                DragSlot.instance.dragSlot.AddItem(_tempItem, _tempCount);
            }
            else
            {
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
        //시작이 인밴이고 드랍이 퀵슬롯일때
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot && slotState == SlotState.QuickSlot)
        {
            // Debug.Log("시작이 인밴이고 드랍이 퀵슬롯일때");
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            if (_tempItem != null)
            {
                DragSlot.instance.dragSlot.AddItem(_tempItem, _tempCount);
            }
            //else
            //{
            //    DragSlot.instance.dragSlot.ClearSlot();
            //}
        }

        ////드래그 시작위치가 퀵슬롯이고 마우스가 땅위에 있을때 
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot && playerMouse.nowMouseOn == PlayerMouse.MouseCheck.NULL)
        {
            Debug.Log("드래그 시작위치가 퀵슬롯이고 퀵슬롯을 제거 할때 ");
            DragSlot.instance.dragSlot.ClearSlot();
        }

        //시작이 인밴이고 드랍이 장비 슬롯일때 
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot && slotState == SlotState.EquipSlot)
        {
            Debug.Log("드래그 시작위치가 인벤이고 장비슬롯에 넣을때 ");
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            //weaponManager.Equipslots[0].AddItem(item, itemCount);
            weaponManager.EquipWeapon(item.itemName);
            if (EquipSlotImage != null)
            {
                SetEquipImage(0);
            }

            if (_tempItem != null)
            {
                DragSlot.instance.dragSlot.AddItem(_tempItem, _tempCount);
            }
            else
            {
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }



    }

}
