using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName ="New Item/item")]
public class Inventory_Item : ScriptableObject
{
    public string itemName; //아이템의 이름
    public Sprite itemImage; //아이템의 이미지
    public GameObject itemPrefab; // 아이템의 프리팹, 자원채취나 죽였을때 아이템이 나와야 하니까

    public enum ItemType {Equipment,Used,Ingredient,AMMO,ETC}; //아이템의 종류
    public ItemType itemType;


    //public enum EquipmentType {melee,range,throwing,NoneEquipmet};



    public int itemNum; // 아이템 갯수



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
