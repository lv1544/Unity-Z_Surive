using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName ="New Item/item")]
public class Inventory_Item : ScriptableObject
{
    public string itemName; //�������� �̸�
    public Sprite itemImage; //�������� �̹���
    public GameObject itemPrefab; // �������� ������, �ڿ�ä�볪 �׿����� �������� ���;� �ϴϱ�

    public enum ItemType {Equipment,Used,Ingredient,AMMO,ETC}; //�������� ����
    public ItemType itemType;


    //public enum EquipmentType {melee,range,throwing,NoneEquipmet};



    public int itemNum; // ������ ����



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
