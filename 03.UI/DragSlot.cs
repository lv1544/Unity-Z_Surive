using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    //�ڱ� �ڽ��� �ν��Ͻ�
    static public DragSlot instance;
    public Slot dragSlot;
    public enum startSlotState {InvenSlot,QuickSlot,EquipSlot,None };
    public startSlotState startSlotInfo;

    //������ �̹���
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

    public void SetColor(float _alpha) // ������ �̹��� ���İ�
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
