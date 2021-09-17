using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    //자기 자신의 인스턴스
    static public DragSlot instance;
    public Slot dragSlot;
    public enum startSlotState {InvenSlot,QuickSlot,EquipSlot,None };
    public startSlotState startSlotInfo;

    //아이템 이미지
    [SerializeField]
    private Image imageItem;

    private void Start()
    {
        instance = this;
        SetColor(0);
    }


    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha) // 아이템 이미지 알파값
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
