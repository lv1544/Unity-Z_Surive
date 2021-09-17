using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //��ġ�� ������ ���¸� �����ϴ� enum
    public enum SlotState { InvenSlot, QuickSlot , EquipSlot ,LootingSlot};
    public SlotState slotState;

    public Inventory_Item item; // ȹ���� ������
    public int itemCount; // ������ ����
    public Image itemImage;  //  �������� �̹���
    public Image EquipSlotImage;  // ��񽽷��� �̹���
    public Inventory_Item Rifle_Ammo;

    [SerializeField]
    private PlayerInfo playerInfo;
    [SerializeField]
    private Text Count_text;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private PlayerMouse playerMouse;

    public Inventory inventory; // �κ��丮�� �����Ҽ� �ֵ���
    private bool StartInven;
    public Transform DropPoint; //������ �������� ��ġ
    public GameObject[] DropItems; // ���������
    public bool IsItem;
    private bool IsChangeWeapon;
    int tap;// ����Ŭ���� 

    //���۽� �ʱ�ȭ
    void Start()
    {
       

    }




    private void Update()
    {
      UpdateEquipSlot();
    }

    //�̹��� ���� ����
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


    //������ ȹ��
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
    {    //�������� ���翩��
        if (item != null)
        {   // � ������ Ÿ���ΰ�
            switch (item.itemType)
            {
                case Inventory_Item.ItemType.Equipment: //����������
                    //���⸦ �ȵ�� ������
                    if (!weaponManager.HasWeapon)
                    {
                        //�κ� ���� �������� ���
                        if (slotState == SlotState.InvenSlot)
                        {
                            weaponManager.Equipslots[0].AddItem(item, itemCount); // ��񽽷Կ� ������ �ְ� 
                            weaponManager.EquipWeapon(item.itemName);//�����ϰ�
                            weaponManager.IsInvenItem = true;
                            IsChangeWeapon = true;
                            ClearSlot();
                        }
                        //�������� ���
                        else if (slotState == SlotState.QuickSlot)
                        {
                            weaponManager.Equipslots[0].AddItem(item, itemCount); // ��� ������ �ְ� 
                            weaponManager.EquipWeapon(item.itemName);//�����ϰ�
                            weaponManager.IsInvenItem = false;
                            IsChangeWeapon = false;
                        }
                    }
                    else //�̹� ���⸦ ��� ������
                    {
                        //����ȿ��� �б⸦ Ÿ����
                        if (slotState == SlotState.EquipSlot)//��� ���� �����ϸ� ���⸦ ������ϰ� 
                        {
                            //�������� �κ��丮�� �ٽ� ���� ��Ŵ
                            if (weaponManager.IsInvenItem == true)
                                inventory.AcquireItem(item);
                            ClearSlot();
                        }
                        if (slotState == SlotState.InvenSlot)//�κ����� ���
                        {
                            //���������� ���
                            if (item.itemType == Inventory_Item.ItemType.Equipment)
                                SwitchItem();
                        }
                        if (slotState == SlotState.QuickSlot) // ������ ���
                        {
                            if (item.itemType == Inventory_Item.ItemType.Equipment)
                            {
                                if(weaponManager.Equipslots[0].item != item)
                                {
                                    weaponManager.Equipslots[0].ClearSlot();//��񽽷�������
                                    weaponManager.Equipslots[0].AddItem(item);
                                    weaponManager.EquipWeapon(item.itemName);
                                }
                                if (weaponManager.Equipslots[0].item == item)
                                {
                                    weaponManager.Equipslots[0].ClearSlot();//��񽽷�������
                                }

                               
                            }
                        }
                    }
                    break;
                case Inventory_Item.ItemType.Used://�Ҹ� ������

                    //ü���� �ִ�ü�º��� �پ�� ������ ��� ����
                    if (playerInfo.health < playerInfo.maxHealth)
                    {
                        Debug.Log(item.itemName + "�� ����߽��ϴ�" + " " + playerInfo.health);
                        playerInfo.RestoreHealth(20f);
                        SetSlotCount(-1);
                    }
                    break;
                case Inventory_Item.ItemType.Ingredient://��� ������
                    break;
                case Inventory_Item.ItemType.AMMO:// �Ѿ� ������
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
        //��񽽷� �ϰ�� 
        if(slotState == SlotState.EquipSlot)
        {
            if (item != null && IsItem == false)
            {
                //������ ���� ���θ� ������ ��ȯ
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


    //������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Count_text.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();

    }
    //���� �ʱ�ȭ
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
        inventory.AcquireItem(weaponManager.Equipslots[0].item);//��񽽷��� �������� �κ��� �߰��Ѵ�
        weaponManager.Equipslots[0].ClearSlot();//��񽽷�������
        weaponManager.Equipslots[0].AddItem(item);
        weaponManager.EquipWeapon(item.itemName);
        ClearSlot();
    }

    IEnumerator SwitchItemCo()
    {        
        inventory.AcquireItem(weaponManager.Equipslots[0].item);//��񽽷��� �������� �κ��� �߰��Ѵ�
        weaponManager.Equipslots[0].ClearSlot();//��񽽷�������
        weaponManager.Equipslots[0].AddItem(item);
        yield return new WaitForSeconds(1f);
        weaponManager.EquipWeapon(item.itemName);
        ClearSlot();
    }


    //����Ŭ�� �̺�Ʈ ������ ����
    public void OnPointerClick(PointerEventData eventData)
    {
        tap = eventData.clickCount;

        if (tap == 2)
        {
            useItem();
        }
    }





    //�巡�װ� ���۵�����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
           // dragStartPos = eventData.position;// ������ġ �޾ƿ�
            //CheckDragStartPos(); // ������ġ�� ���� ��𼭵巡�� ���ۉ���� �˰Ե�
            DragSlot.instance.dragSlot = this; // �巡�� ������ ����ִ� ���� ä���
            DragSlot.instance.DragSetImage(itemImage); //���Կ� ������ �̹����� �޴´�
            DragSlot.instance.transform.position = eventData.position; //�̹����� ����´�


            if (slotState == SlotState.InvenSlot)
            {
                DragSlot.instance.startSlotInfo = DragSlot.startSlotState.InvenSlot;
                Debug.Log("�κ����� ����");
            }
            else if (slotState == SlotState.QuickSlot)
            {
                DragSlot.instance.startSlotInfo = DragSlot.startSlotState.QuickSlot;
                Debug.Log("�����Կ��� ����");
            }
            else if (slotState == SlotState.EquipSlot)
            {
                DragSlot.instance.startSlotInfo = DragSlot.startSlotState.EquipSlot;
                Debug.Log("��񽽷Կ��� ����");
            }
        }
    }

    //�巡�� ���϶�
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;

        playerMouse.IsItemDrag =true;

        Debug.Log("�巡�� ���϶�");
    }
    //�巡�װ� ��������
    public void OnEndDrag(PointerEventData eventData)
    {    //������ ����� ���
        playerMouse.IsItemDrag = false ;

        if (playerMouse.nowMouseOn == PlayerMouse.MouseCheck.NULL)
        {
            //�κ� ������ ������
            if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot)
            {
                //�������� �����ϸ� ����
                if (WeaponManager.currentWeapon != null)
                {
                    weaponManager.weaponRelease();

                }
                //���Ӿ����� ������
                GameObject instantDropItem = Instantiate(DropItems[0], DropPoint.position, DropPoint.rotation);


                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;

            }
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot)
            {//�����Կ��ִ� �Ŵ� �����Կ��� ����
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }//��񽽷��� �������� �κ��� �߰��Ѵ�
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.EquipSlot)
            {
                inventory.AcquireItem(weaponManager.Equipslots[0].item);
  
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }

        }
        else// ui���� ����Ѱ�� 
        {
            //�巡�װ� ������ ���Կ��� �������
            //DragSlot.instance.dragSlot.ClearSlot();
    
            if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot)
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot)
            {//�����Կ��ִ� �Ŵ� ��
                DragSlot.instance.dragSlot.ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance.dragSlot = null;
            }
            else if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.EquipSlot)
            {
                inventory.AcquireItem(weaponManager.Equipslots[0].item);//��񽽷��� �������� �κ��� �߰��Ѵ�
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
        //    DropPos = eventData.position;// �����ġ ������Ʈ�Ǵ� ��
                                         //Debug.Log("Mouse Position : " + eventData.position);
                                         //CheckDropEndPos(); // �����ġ�� ���ؼ� ����Ǵ� ���� �κ����� ���������� �˰Ե�

       
     
            ChangeSlot();
        }
    }

    //����ؼ� ������ �ű��  ���� ������ �ִ�..
    private void ChangeSlot()
    {
        Inventory_Item _tempItem = item;
        int _tempCount = itemCount;

        //Ȯ�ο�
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot)
        {
            Debug.Log("�κ����Կ��� ����");
        }
         else
        {
            Debug.Log("�����Կ��� ����");
        }

        //�巡�� ������ġ�� �κ��̰� ����� �κ��϶�
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot && slotState == SlotState.InvenSlot)
        {
            // Debug.Log("�巡�� ������ġ�� �κ��̰� ����� �κ��϶�");
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

        //�巡�� ������ġ�� �������̰� ����� �������϶�
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot && slotState == SlotState.QuickSlot)
        {
            Debug.Log("�巡�� ������ġ�� �������̰� ����� �������϶�");
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
        //������ �ι��̰� ����� �������϶�
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot && slotState == SlotState.QuickSlot)
        {
            // Debug.Log("������ �ι��̰� ����� �������϶�");
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

        ////�巡�� ������ġ�� �������̰� ���콺�� ������ ������ 
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.QuickSlot && playerMouse.nowMouseOn == PlayerMouse.MouseCheck.NULL)
        {
            Debug.Log("�巡�� ������ġ�� �������̰� �������� ���� �Ҷ� ");
            DragSlot.instance.dragSlot.ClearSlot();
        }

        //������ �ι��̰� ����� ��� �����϶� 
        if (DragSlot.instance.startSlotInfo == DragSlot.startSlotState.InvenSlot && slotState == SlotState.EquipSlot)
        {
            Debug.Log("�巡�� ������ġ�� �κ��̰� ��񽽷Կ� ������ ");
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
