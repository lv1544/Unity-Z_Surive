using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//아이템 인덱스 번호 
//나무 방망이 -> 0
//돌격소총 -> 1
//단검 -> 2
//수류탄 -> 3 이런식으로... 무기가 그렇게 많지 않다
//웨폰 매니저 현재 어떤 무기를 들고 있는지 
public class WeaponManager : MonoBehaviour
{
    //무기 중복 교체 방지
    public static bool isChangeEquipment;
    public static GameObject currentWeapon;  //현재 무기
    public enum WeaponType { melee, range, throwing,None}
    public static WeaponType currentWeaponType;
    public bool CanRangeAttack;
    public bool HasWeapon = false;

    public PlayerRangeAttack playerRangeAttack;

    public Animator animator; // 무기에 맞는 애니매이션
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

        //무기 
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
        //    if(currentWeapon == null) // 현재 무기가 없을때
        //    {
        //        IfQuickSlotHasItem();//퀵슬롯에 아이템이 있다면
        //    }
        //    else // 현재 무기가 있을때  퀵슬롯에도 아이템이 있다면 
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

                    Debug.Log("나무배트 사용");
                    weaponRelease();
                    weapons[0].SetActive(true); //  무기 활성화 
                    currentWeapon = weapons[0];  //현재 무기
                    curWeaponImages[0].SetActive(true); //현재 무기 ui
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
                    curWeaponImages[1].SetActive(true); //현재 무기 ui
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
