using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    //필요한 컴포넌트

    [SerializeField]
    private GameObject SlotParent;
    [SerializeField]
    private PlayerInput playerInput;
    public Slot[] quickSlots;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = SlotParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        usingQuickSlotItem();
    }

    void usingQuickSlotItem()
    {
       if(playerInput.quickSlot1)
        {
            quickSlots[0].useItem();
        }
        if (playerInput.quickSlot2)
        {
            quickSlots[1].useItem();
        }
        if (playerInput.quickSlot3)
        {
            quickSlots[2].useItem();
        }
        if (playerInput.quickSlot4)
        {
            quickSlots[3].useItem();
        }
        if (playerInput.quickSlot5)
        {
            quickSlots[4].useItem();
        }
        if (playerInput.quickSlot6)
        {
            quickSlots[5].useItem();
        }
        if (playerInput.quickSlot7)
        {
            quickSlots[6].useItem();
        }
    }





}
