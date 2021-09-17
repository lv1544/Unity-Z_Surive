using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//플래이어 인풋은 키보드와 추후 모바일 조이스틱에서 사용한다
public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름
    public string mouseLeftName = "Fire1"; // 공격을 위한 입력 버튼 이름
    public string mouseRightName = "Fire2"; // 공격을 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름
    public string InterActionButtonName = "InterAction";  //상호작용키
    public string SpaceStop = "SpaceStop"; // 캐릭터 정지

    public float  move { get; private set; }  //감지된 이동값
    public float  rotate { get; private set; } // 감지된 회전 입력값
    public bool   click { get; private set; } // 감지된 클릭 
    public bool   fire { get; private set; } // 감지된 발사 입력값
    public bool   fire2 { get; private set; } // 감지된 발사 입력값
    public bool   reload { get; private set; } // 감지된 재장전 입력값
    public bool   InterAction { get; private set; } // 감지된 재장전 입력값

    public bool  quickSlot1 { get; private set; } // 퀵슬롯1
    public bool  quickSlot2 { get; private set; } // 퀵슬롯2
    public bool  quickSlot3 { get; private set; } // 퀵슬롯3
    public bool  quickSlot4 { get; private set; } // 퀵슬롯4
    public bool  quickSlot5 { get; private set; } // 퀵슬롯5
    public bool  quickSlot6 { get; private set; } // 퀵슬롯6
    public bool  quickSlot7 { get; private set; } // 퀵슬롯7


    // Update is called once per frame
    void Update()
    {
        //// 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        //if (GameManager.instance != null
        //    && GameManager.instance.isGameover)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    reload = false;
        //    return;
        //}

        //나중에 
        // move에 관한 입력 감지
        //move = Input.GetAxis(moveAxisName);
        // rotate에 관한 입력 감지
        //rotate = Input.GetAxis(rotateAxisName);
        
        
        //click 감지
        click = Input.GetButtonDown(mouseLeftName);
        // fire에 관한 입력 감지
        fire = Input.GetButton(mouseLeftName);
        // fire에 관한 입력 감지
        fire2 = Input.GetButton(mouseRightName);
        // reload에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName);
        //상호작용키 
        InterAction = Input.GetButtonDown(InterActionButtonName);

        quickSlot1 = Input.GetButtonDown("QuickSlot1");
        quickSlot2 = Input.GetButtonDown("QuickSlot2");
        quickSlot3 = Input.GetButtonDown("QuickSlot3");
        quickSlot4 = Input.GetButtonDown("QuickSlot4");
        quickSlot5 = Input.GetButtonDown("QuickSlot5");
        quickSlot6 = Input.GetButtonDown("QuickSlot6");
        quickSlot7 = Input.GetButtonDown("QuickSlot7");




    }
}
