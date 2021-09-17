using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//마우스를 이용한 클릭과 레이 케스트 관련한다
public class PlayerMouse : MonoBehaviour, IPointerClickHandler
{
    public enum MouseCheck { PLAYER, NPC, ENEMY, ITEM, LootingObj, UI, NULL };
    public MouseCheck nowMouseOn;
    private RaycastHit mouseHitInfo;  // 마우스로부터 충돌체 정보 받아오기  
    public MouseCheck targetType;
    public GameObject TargetObject;  // 왼클릭이 오브젝트 컨트롤 클릭이다. 왼클릭으로 누르면 타겟이 생긴다 
    public Camera followCamera;
    private bool isMove;
    private bool isRightClick;
    private bool isLeftClick;
    public bool IsItemDrag = false;
    public bool CanMeleeAttack;
    public bool CanRangeAttack = false;
     public bool IsSpaceDown = false;
    public bool LootingStart = false;

    private float pickUpRange; //습득 가능한 범위
    private bool CanInterAction;  //습득 가능한지 여부 
    private Vector3 destination;
   
    public float moveSpeed = 5f;// 이동속도
   
    private Quaternion dr; //회전 쿼터니온
    //private float turnSpeed = 10f;
    private Rigidbody playerRigidbody;
    private PlayerInput playerInput;
    private PlayerPickUp playerPickUp;
    private Animator animator;
    private Vector3 stopMouse;


    //필요한 컴포넌트
    [SerializeField]
    private Text LogMessage;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private RectTransform mouseCursor;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Looting_Inventory looting_Inventory;
    [SerializeField]
    private GameObject testObject; // 클릭
    [SerializeField]
    private PlayerMeleeAttack PlayerMeleeAttack;
    [SerializeField]
    private PlayerRangeAttack playerRangeAttack;
    [SerializeField]
    private PlayerInfo playerInfo;
    [SerializeField]
    private GameObject searchUIGameObject;

    private void Awake()
    {
        pickUpRange = 1f;
        PlayerMeleeAttack = GetComponent<PlayerMeleeAttack>();
        playerRangeAttack = GetComponent<PlayerRangeAttack>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //마우스클릭 위치와 마우스 커서 위치 살짝 다르게함 마우스 레이캐스트 때문
        Cursor.visible = false;
        mouseCursor.transform.SetAsLastSibling();
      //  turnSpeed = 10f;
        Vector3 Adjust = new Vector3(1f ,1f);
        mouseCursor.position = Input.mousePosition + Adjust;


    }

    // 실시간으로 업데이트 되는 부분
    private void Update()
    {
        ChecKObject();
        ObjectClick();
        InterActions();
        UpdateSpaceDown();
        stopAttack();
        OnMouseDown();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        weaponAttackLook();// 좌클릭 한 방향으로 플래이어 바라보기
        MoveToMousePos(); //우클릭한 지점으로 이동
    }
    //마우스 클릭한 곳으로 움직이기 
    private void MoveToMousePos()
    {
        rightClick();//우클릭
        mouseMove();// 클릭이후 이동
    }

    public void OnPointerClick(PointerEventData eventData)
    {


    }

    //우클릭 이동 
    private void rightClick()
    {
        //우클릭 했을 때, 마우스가 UI에 있을때, 아이템을 루팅하지 않을 때
        if (playerInput.fire2 && nowMouseOn != MouseCheck.UI && !IsItemDrag && !LootingStart)
        {
            isRightClick = true;
            Vector3 HitPoint = mouseHitInfo.point;
            HitPoint.y = 0;
            //위치를 정하는 함수 
            SetDestination(HitPoint);
        }
    }


    private void leftClickMove()
    {
        if (playerInput.fire && nowMouseOn != MouseCheck.UI)
        {
                Vector3 HitPoint = mouseHitInfo.point;
                HitPoint.y = 0;
                //위치를 정하는 함수 
                SetDestination(HitPoint);
        }
    }


    private void SetDestination(Vector3 dest)
    {
        //가는 위치를 설정하기 
        destination = dest;

        //설정한 위치를 향하는 방향구하기
        Vector3 dir = destination - transform.position;

        //위치와에 거리 1.f 보다 큰 경우
        if (dir.magnitude > 1f)
        {
            //스페이스 바가 누르지 않고 아이템을 루팅하지 않으 경우
            if (!IsSpaceDown && !LootingStart)
            {
                //이동 애니매이션이 실행된다.
                isMove = true;
                animator.SetBool("IsWalk", true);
            }

            //클릭한 방향으로 플래이어 쳐다보기
            dir.y = 0;
            transform.forward = dir.normalized;
        }

    }

    private void mouseMove()
    {
        if (isMove && !IsSpaceDown && !LootingStart)
        {//스테틱으로 상태를 정함
            PlayerInfo.state = PlayerInfo.playerState.RUN;

            var dir = destination - transform.position;
            Vector3 moveDistance =
            dir.normalized * moveSpeed * Time.deltaTime;
            //트랜스폼을 이용한것
            //transform.position += moveDistance;
            //리지드 바디를 이용해 게임 오브젝트 위치 변경
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

            //클릭 지점에 파티클 생성 라이트 클릭 이동일때만 수행
            if (isRightClick)
            {
                Vector3 ClickPos = destination;
                ClickPos.y = 0.2f;
                testObject.SetActive(true);
                testObject.gameObject.transform.position = ClickPos;
            }

            //해당지점에 도착했을때 
            if (Vector3.Distance(transform.position, destination) <= 0.5f)
            {
                PlayerInfo.state = PlayerInfo.playerState.IDLE;
                isMove = false;
                isRightClick = false;
                animator.SetBool("IsWalk", false);
                testObject.SetActive(false);
                return;
            }
        }
    }

    private void ChecKObject()
    {
        //마우스 포지션이 변화할경우에만 체크
        if (mouseCursor.position != Input.mousePosition)
        {
            mouseCursor.position = Input.mousePosition;
            //마우스 입력 좌표에서 화변으로 레이캐스팅을 실현
            if (Physics.Raycast(followCamera.ScreenPointToRay(Input.mousePosition), out mouseHitInfo))
            {

                //레이케스팅으로 인한 충돌체 정보에서 tag를 받아와 현재 마우스가 어떤 오브젝트위에 있는지 확인
                switch (mouseHitInfo.transform.tag)
                {
                    case "DropItem":
                        LogDisappear();
                        InfoAppear();
                        nowMouseOn = MouseCheck.ITEM;
                        break;
                    case "Enemy":
                        LogDisappear();
                        Debug.Log("적 클릭");
                        nowMouseOn = MouseCheck.ENEMY;
                        break;
                    case "Map":
                        LogDisappear();
                        Debug.Log("맨땅");
                        nowMouseOn = MouseCheck.NULL;
                        break;
                    case "LootingObject":
                        LogDisappear();
                        InfoAppear();
                        nowMouseOn = MouseCheck.LootingObj;
                        break;
                }
            }
        }
    }  
    
    //마우스 레이로 충돌체 체크하기
    private void InfoAppear()
    {
        //Debug.Log("아이템 충돌중");
        // LogMessage.gameObject.SetActive(true);
        if(mouseHitInfo.transform.GetComponent<ItemPickUp>())
        LogMessage.text = mouseHitInfo.transform.GetComponent<ItemPickUp>().item.itemName;

        if (mouseHitInfo.transform.GetComponent<LootingObject>())
            LogMessage.text = mouseHitInfo.transform.gameObject.name;

        if (mouseHitInfo.transform.GetComponent<mouseOnOff>().playerMouse == null)
        {
            mouseHitInfo.transform.GetComponent<mouseOnOff>().playerMouse = this;
        }

        //여기서 외곽선을 활성화 하자
        if (mouseHitInfo.transform.GetComponent<Outline>())
        {
            
            mouseHitInfo.transform.GetComponent<Outline>().enabled = true;
        }


    } // 마우스에 아이템이 충돌하면 아이템 이름 로그 띄우기
    private void LogDisappear()
    {
        LogMessage.text = "";
        // LogMessage.gameObject.SetActive(false);
    }//로그 지우는 함수
    private bool IsCanInterAction(Vector3 MousePos)
    {
        float UpdateRange = (MousePos - transform.position).magnitude;

        if (pickUpRange >= UpdateRange)
            CanInterAction = true;
        else
            CanInterAction = false;


        return CanInterAction;
    }  
    private void ObjectClick()
    {
        //타겟 정하는 부분 
        if (playerInput.fire && mouseHitInfo.transform != null && !IsItemDrag)
        {
            switch (nowMouseOn)
            {
                case MouseCheck.PLAYER:
                    break;
                case MouseCheck.NPC:
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.NPC;
                    leftClickMove();// npc나 아이템을 누르면 접근한다
                    break;
                case MouseCheck.ITEM:
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.ITEM;
                    leftClickMove();  //주우려고 이동
                    break;
                case MouseCheck.LootingObj:
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.LootingObj;
                    SetDestination(TargetObject.GetComponent<Transform>().position);
                    break;
                case MouseCheck.ENEMY:   //적일경우
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.ENEMY;
                    // 근접무기 들고 있으면 이동
                    if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.melee && !PlayerMeleeAttack.IsAttack)
                    {
                        //공격하러 이동  공격 범위에 없으면  이동
                        if (!CanInterAction)
                        {
                            leftClickMove();
                        }
                        else // 있으면 공격
                        {
                            PlayerMeleeAttack.Swing();
                        }
                    }
                    else if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.range && nowMouseOn != MouseCheck.UI)
                    {
                        playerRangeAttack.Shot();
                    }
                    break;
                case MouseCheck.UI: //  ui일때
                    break;
                case MouseCheck.NULL:
                    //Debug.Log("맨땅 클릭");
                    break;
            }
        }
    }  

    void InterActions()
    {
        //타겟이 정해지고 접근했을때 상호작용 발생하는 지점
        if (TargetObject != null )
        {
            LivingEntity livingEntity = TargetObject.GetComponent<LivingEntity>();
            if (IsCanInterAction(TargetObject.transform.position))
            {
                if (targetType == MouseCheck.ITEM)
                {
                    // Debug.Log("아이템 습득시점");
                    inventory.AcquireItem(TargetObject.GetComponent<ItemPickUp>().item);//아이템 획득  하는 함수
                    Destroy(TargetObject);
                }
                if (targetType == MouseCheck.ENEMY && livingEntity !=null && !livingEntity.dead)
                {
                    LookTargetObject(TargetObject.transform.position);
                    CanMeleeAttack = true;
                }
                else
                {
                    CanMeleeAttack = false;
                }
            }
        }
    


    }

    void UpdateSpaceDown()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //스페이스
            IsSpaceDown = true;

            PlayerInfo.state = PlayerInfo.playerState.IDLE;
            isMove = false;
            isRightClick = false;
            animator.SetBool("IsWalk", false);
            testObject.SetActive(false);


            Debug.Log("Space Down");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsSpaceDown = false;
            Debug.Log("Space UP");
        }

    }
    IEnumerator Looting(Collider other)
    {
        //IsSpaceDown = true;
        LootingStart = true;
        PlayerInfo.state = PlayerInfo.playerState.IDLE;
        isMove = false;
        isRightClick = false;
        animator.SetBool("IsWalk", false);
        testObject.SetActive(false);
        yield return new WaitForSeconds(0.01f); //서칭 ui 등장


        //searchUIGameObject.SetActive(true);

        float CoolDown = searchUIGameObject.GetComponent<SearchUI>().coolDownTime;
        float tempTime = CoolDown;

        if (other.transform.gameObject.GetComponent<LootingObject>().IsBeforeOpen)//예전에 열은 적이 있으면 바로 확인
        {
            searchUIGameObject.SetActive(false);
            tempTime = 0.00000001f;
        }
        else
        {
            searchUIGameObject.SetActive(true);
        }

        yield return new WaitForSeconds(tempTime);
        //여기서 조사 이벤트가 생김
        if (other.GetComponent<LootingObject>())
        {
            looting_Inventory.lootingObject = other.transform.gameObject.GetComponent<LootingObject>();
        }

        inventory.InventoryOpen();
        looting_Inventory.TryOpenInventory(); //루트 인벤토리가 열린다


        yield return new WaitForSeconds(0.05f);
        //IsSpaceDown = false;
        playerInfo.IsCanRooting = false;
        LootingStart = false;
    }


    void stopAttack()
    {
        if(IsSpaceDown)
        {
            if(playerInput.fire && WeaponManager.currentWeapon != null && nowMouseOn != MouseCheck.UI &&CanRangeAttack)
            {
                if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.melee && !PlayerMeleeAttack.IsAttack)
                {
                   PlayerMeleeAttack.Swing();
                }
                else if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.range)
                {
                    playerRangeAttack.Shot();
                }
            }

      
        }


    }


    public void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("ui 에 있다.");
            nowMouseOn = MouseCheck.UI;
            Debug.Log("UI 체크 중");
            CanRangeAttack = false;
        }
        else
        {
            CanRangeAttack = true;
        }
    
    }



    private void weaponAttackLook()
    {//좌클릭을 했을때 클릭한 지점을 바라보기
            if (playerInput.fire && nowMouseOn != MouseCheck.UI && !IsItemDrag)
        {
                    LookTargetObject(mouseHitInfo.point);
        }
    }


    //클릭한 지점을 바라보는 함수
    private void LookTargetObject(Vector3 targetPos)
    {
        //루팅중이지 않을떼
        if(!LootingStart)
        {
            Vector3 dir = targetPos - transform.position;
            dir.y = 0;
            transform.forward = dir.normalized;
        }
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( !playerInfo.dead && targetType == MouseCheck.LootingObj)
        {//적의 공격
            if (other.tag == "LootingObject" )
            {
                Debug.Log("루팅가능");
           
                StartCoroutine(Looting(other));


            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LootingObject")
        {
            looting_Inventory.TryOpenInventory();
            inventory.InventoryClose();
        }
    }








}

