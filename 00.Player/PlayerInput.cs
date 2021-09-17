using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�÷��̾� ��ǲ�� Ű����� ���� ����� ���̽�ƽ���� ����Ѵ�
public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string rotateAxisName = "Horizontal"; // �¿� ȸ���� ���� �Է��� �̸�
    public string mouseLeftName = "Fire1"; // ������ ���� �Է� ��ư �̸�
    public string mouseRightName = "Fire2"; // ������ ���� �Է� ��ư �̸�
    public string reloadButtonName = "Reload"; // �������� ���� �Է� ��ư �̸�
    public string InterActionButtonName = "InterAction";  //��ȣ�ۿ�Ű
    public string SpaceStop = "SpaceStop"; // ĳ���� ����

    public float  move { get; private set; }  //������ �̵���
    public float  rotate { get; private set; } // ������ ȸ�� �Է°�
    public bool   click { get; private set; } // ������ Ŭ�� 
    public bool   fire { get; private set; } // ������ �߻� �Է°�
    public bool   fire2 { get; private set; } // ������ �߻� �Է°�
    public bool   reload { get; private set; } // ������ ������ �Է°�
    public bool   InterAction { get; private set; } // ������ ������ �Է°�

    public bool  quickSlot1 { get; private set; } // ������1
    public bool  quickSlot2 { get; private set; } // ������2
    public bool  quickSlot3 { get; private set; } // ������3
    public bool  quickSlot4 { get; private set; } // ������4
    public bool  quickSlot5 { get; private set; } // ������5
    public bool  quickSlot6 { get; private set; } // ������6
    public bool  quickSlot7 { get; private set; } // ������7


    // Update is called once per frame
    void Update()
    {
        //// ���ӿ��� ���¿����� ����� �Է��� �������� �ʴ´�
        //if (GameManager.instance != null
        //    && GameManager.instance.isGameover)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    reload = false;
        //    return;
        //}

        //���߿� 
        // move�� ���� �Է� ����
        //move = Input.GetAxis(moveAxisName);
        // rotate�� ���� �Է� ����
        //rotate = Input.GetAxis(rotateAxisName);
        
        
        //click ����
        click = Input.GetButtonDown(mouseLeftName);
        // fire�� ���� �Է� ����
        fire = Input.GetButton(mouseLeftName);
        // fire�� ���� �Է� ����
        fire2 = Input.GetButton(mouseRightName);
        // reload�� ���� �Է� ����
        reload = Input.GetButtonDown(reloadButtonName);
        //��ȣ�ۿ�Ű 
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
