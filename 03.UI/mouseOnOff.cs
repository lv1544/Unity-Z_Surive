using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseOnOff : MonoBehaviour
{
    public PlayerMouse playerMouse;
    public bool IsMouseOn = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseOn();
    }

    void UpdateMouseOn()
    {
        if (playerMouse.nowMouseOn == PlayerMouse.MouseCheck.NULL)
        {
            IsMouseOn = false;

            if (this.GetComponentInParent<Outline>() != null)
            {
                this.GetComponentInParent<Outline>().enabled = false;
            }


        }


    }

}
