using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterAction : MonoBehaviour
{
    private PlayerInput  playerInput;
    private PlayerMouse  playerMouse;
    private PlayerPickUp playerPickUp;

    GameObject nearObject;

    //private bool IsItem = false;
    // 등등 이런식으로 인터액션에서는 충돌 여부를 판단해서 불값만 가지고 있는다 
    // 나머지는 가져온 컴포넌트에서  실행한다.
    //이런식으로..
    //private bool IsNPC = false;
    //private bool IsTool = false;

    [SerializeField]
    private Text LogMessage;



    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMouse = GetComponent<PlayerMouse>();
    }

    public void Update()
    {
        if(nearObject != null)
        InterAction();
    }

    public void InterAction()
    {
        //상호작용키  눌렀을때 
        if (playerInput.InterAction)
        {
            if (nearObject.tag == "DropItem")
            {

            }
        }

     
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    //아이템 상호작용
    //        nearObject = other.gameObject;
    //        Debug.Log("상호작용 오브젝트  충돌");
    //    Destroy(nearObject);
            

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    //인접 오브젝트 충돌 범위 벗어남
    //        nearObject = null;
    //}

}
