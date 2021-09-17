using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerPickUp : MonoBehaviour
{
    //필요한 컴포넌트들 
    private PlayerMouse playerMouse;

    public bool canPickUp { get; set; } // 아이템 습득 가능 여부

    //마우스로 아이템 픽업 하기 위해..드래그 앤 드랍 하고 나서 넣자
    private RaycastHit MousehitInfo; // 마우스로부터 충돌체 정보 받아오기

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
