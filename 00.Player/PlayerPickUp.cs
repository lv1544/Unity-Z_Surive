using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerPickUp : MonoBehaviour
{
    //�ʿ��� ������Ʈ�� 
    private PlayerMouse playerMouse;

    public bool canPickUp { get; set; } // ������ ���� ���� ����

    //���콺�� ������ �Ⱦ� �ϱ� ����..�巡�� �� ��� �ϰ� ���� ����
    private RaycastHit MousehitInfo; // ���콺�κ��� �浹ü ���� �޾ƿ���

    [SerializeField]
    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        canPickUp = false;
    }
    

    // Update is called once per frame
    void Update()
    {

    }

    public void ItemInfoApper()
    {

    }




}
