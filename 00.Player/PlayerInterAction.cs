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
    // ��� �̷������� ���;׼ǿ����� �浹 ���θ� �Ǵ��ؼ� �Ұ��� ������ �ִ´� 
    // �������� ������ ������Ʈ����  �����Ѵ�.
    //�̷�������..
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
        //��ȣ�ۿ�Ű  �������� 
        if (playerInput.InterAction)
        {
            if (nearObject.tag == "DropItem")
            {

            }
        }

     
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    //������ ��ȣ�ۿ�
    //        nearObject = other.gameObject;
    //        Debug.Log("��ȣ�ۿ� ������Ʈ  �浹");
    //    Destroy(nearObject);
            

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    //���� ������Ʈ �浹 ���� ���
    //        nearObject = null;
    //}

}
