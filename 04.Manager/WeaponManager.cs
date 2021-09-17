using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������ �ε��� ��ȣ 
//���� ����� -> 0
//���ݼ��� -> 1
//�ܰ� -> 2
//����ź -> 3 �̷�������... ���Ⱑ �׷��� ���� �ʴ�
//���� �Ŵ��� ���� � ���⸦ ��� �ִ��� 
public class WeaponManager : MonoBehaviour
{
    //���� �ߺ� ��ü ����
    public static bool isChangeEquipment;
    public static GameObject currentWeapon;  //���� ����
    public enum WeaponType { melee, range, throwing,None}
    public static WeaponType currentWeaponType;
    public bool CanRangeAttack;
    public bool HasWeapon = false;

    public PlayerRangeAttack playerRangeAttack;

    public Animator animator; // ���⿡ �´� �ִϸ��̼�
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private float changeEquipDelay;
    [SerializeField]
    private float changeEquipEndDelay;
    [SerializeField]
    private GameObject[] weapons;
    [SerializeField]
    private Slot[] quickslots;
    [SerializeField]
    private GameObject[] curWeaponImages;

    public Slot   InvenSlot0;
    public Slot[] Equipslots;

    public bool IsInvenItem;



    private void Start()
    {
        currentWeaponType = WeaponType.None;
        playerInput = GetComponent<PlayerInput>();
        playerRangeAttack = GetComponent<PlayerRangeAttack>();
    }

    private void Update()
    {
        UsingQuickSlot();
        ReleaseEquipSlot();

        //���� 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weaponRelease();
        }

       // IfQuickSlotHasItem();


    }

    void  UsingQuickSlot()
    {
        //if(PlayerInfo.playerState != PlayerInfo.PlayerState.Dead 
        //    && PlayerInfo.playerState != PlayerInfo.PlayerState.Sleeping 
        //    && PlayerInfo.playerState != PlayerInfo.PlayerState.Looting
        //    && PlayerInfo.playerState != PlayerInfo.PlayerState.Eating)
        //{
        //    if(currentWeapon == null) // ���� ���Ⱑ ������
        //    {
        //        IfQuickSlotHasItem();//�����Կ� �������� �ִٸ�
        //    }
        //    else // ���� ���Ⱑ ������  �����Կ��� �������� �ִٸ� 
        //    {
        //        WeaponSwap();
        //    }

           
        //}
    }

    private void ReleaseEquipSlot()
    {
        if(HasWeapon == true  && Equipslots[0].item == null)
        {
            weaponRelease();
            //InvenSlot0.AddItem
        }

    }


    public void EquipWeapon(string weaponName)
    {
        switch (weaponName)
        {
            case "Wood_Bat" :
                if(currentWeapon != weapons[0])
                {

                    Debug.Log("������Ʈ ���");
                    weaponRelease();
                    weapons[0].SetActive(true); //  ���� Ȱ��ȭ 
                    currentWeapon = weapons[0];  //���� ����
                    curWeaponImages[0].SetActive(true); //���� ���� ui
                    currentWeaponType = WeaponType.melee;
                    animator.SetBool("IsMeleeWeapon", true);
                    CanRangeAttack = false;
                    HasWeapon = true;

                }
                else
                    weaponRelease();

                break;
            case "Assault_Rifle":
                if (currentWeapon != weapons[1])
                {
                    
                    weaponRelease();
                    weapons[1].SetActive(true);
                    currentWeapon = weapons[1];
                    curWeaponImages[1].SetActive(true); //���� ���� ui
                    currentWeaponType = WeaponType.range;
                    animator.SetBool("IsRangeWeapon", true);
                    animator.SetBool("IsMeleeWeapon", false);

                    CanRangeAttack = true;
                    HasWeapon = true;
                }
                else
                    weaponRelease();

                break;


            default:
                break;
        }
    }

    public void weaponRelease()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
            animator.SetBool("IsRangeWeapon", false);
            animator.SetBool("IsMeleeWeapon", false);
        }

        for (int i = 0; i < curWeaponImages.Length; i++)
        {
            curWeaponImages[i].SetActive(false);
        }

        currentWeapon = null;
        currentWeaponType = WeaponManager.WeaponType.None;
        HasWeapon = false;

    }

    public void IfQuickSlotHasItem()
    {
        if(HasWeapon == false)
        {

            if (playerInput.quickSlot1 && quickslots[0].item != null)
            {
                EquipWeapon(quickslots[0].item.itemName);
            }
            if (playerInput.quickSlot2 && quickslots[1].item != null)
            {
                EquipWeapon(quickslots[1].item.itemName);

            }
            if (playerInput.quickSlot3 && quickslots[2].item != null)
            {
                EquipWeapon(quickslots[2].item.itemName);

            }
            if (playerInput.quickSlot4 && quickslots[3].item != null)
            {
                EquipWeapon(quickslots[3].item.itemName);
            }
            if (playerInput.quickSlot5 && quickslots[4].item != null)
            {
                EquipWeapon(quickslots[4].item.itemName);
            }
            if (playerInput.quickSlot6 && quickslots[5].item != null)
            {
                EquipWeapon(quickslots[5].item.itemName);
            }
            if (playerInput.quickSlot7 && quickslots[6].item != null)
            {
                EquipWeapon(quickslots[6].item.itemName);
            }
        }


    }

    public void WeaponSwap()
    {
        if (playerInput.quickSlot1 && quickslots[0].item != null)
        {
            weaponRelease();
            EquipWeapon(quickslots[0].item.itemName);
        }
        if (playerInput.quickSlot2 && quickslots[1].item != null)
        {
            weaponRelease();
            EquipWeapon(quickslots[1].item.itemName);

        }
        if (playerInput.quickSlot3 && quickslots[2].item != null)
        {
            weaponRelease();
            EquipWeapon(quickslots[2].item.itemName);

        }
        if (playerInput.quickSlot4 && quickslots[3].item != null)
        {
            weaponRelease();
            EquipWeapon(quickslots[3].item.itemName);
        }
        if (playerInput.quickSlot5 && quickslots[4].item != null)
        {
            weaponRelease();
            EquipWeapon(quickslots[4].item.itemName);
        }
        if (playerInput.quickSlot6 && quickslots[5].item != null)
        {
            weaponRelease();
            EquipWeapon(quickslots[5].item.itemName);
        }
        if (playerInput.quickSlot7 && quickslots[6].item != null)
        {
            weaponRelease();
            EquipWeapon(quickslots[6].item.itemName);
        }
    }


}
