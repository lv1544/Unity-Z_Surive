using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//���콺�� �̿��� Ŭ���� ���� �ɽ�Ʈ �����Ѵ�
public class PlayerMouse : MonoBehaviour, IPointerClickHandler
{
    public enum MouseCheck { PLAYER, NPC, ENEMY, ITEM, LootingObj, UI, NULL };
    public MouseCheck nowMouseOn;
    private RaycastHit mouseHitInfo;  // ���콺�κ��� �浹ü ���� �޾ƿ���  
    public MouseCheck targetType;
    public GameObject TargetObject;  // ��Ŭ���� ������Ʈ ��Ʈ�� Ŭ���̴�. ��Ŭ������ ������ Ÿ���� ����� 
    public Camera followCamera;
    private bool isMove;
    private bool isRightClick;
    private bool isLeftClick;
    public bool IsItemDrag = false;
    public bool CanMeleeAttack;
    public bool CanRangeAttack = false;
     public bool IsSpaceDown = false;
    public bool LootingStart = false;

    private float pickUpRange; //���� ������ ����
    private bool CanInterAction;  //���� �������� ���� 
    private Vector3 destination;
   
    public float moveSpeed = 5f;// �̵��ӵ�
   
    private Quaternion dr; //ȸ�� ���ʹϿ�
    //private float turnSpeed = 10f;
    private Rigidbody playerRigidbody;
    private PlayerInput playerInput;
    private PlayerPickUp playerPickUp;
    private Animator animator;
    private Vector3 stopMouse;


    //�ʿ��� ������Ʈ
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
    private GameObject testObject; // Ŭ��
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
        //���콺Ŭ�� ��ġ�� ���콺 Ŀ�� ��ġ ��¦ �ٸ����� ���콺 ����ĳ��Ʈ ����
        Cursor.visible = false;
        mouseCursor.transform.SetAsLastSibling();
      //  turnSpeed = 10f;
        Vector3 Adjust = new Vector3(1f ,1f);
        mouseCursor.position = Input.mousePosition + Adjust;


    }

    // �ǽð����� ������Ʈ �Ǵ� �κ�
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
        weaponAttackLook();// ��Ŭ�� �� �������� �÷��̾� �ٶ󺸱�
        MoveToMousePos(); //��Ŭ���� �������� �̵�
    }
    //���콺 Ŭ���� ������ �����̱� 
    private void MoveToMousePos()
    {
        rightClick();//��Ŭ��
        mouseMove();// Ŭ������ �̵�
    }

    public void OnPointerClick(PointerEventData eventData)
    {


    }

    //��Ŭ�� �̵� 
    private void rightClick()
    {
        //��Ŭ�� ���� ��, ���콺�� UI�� ������, �������� �������� ���� ��
        if (playerInput.fire2 && nowMouseOn != MouseCheck.UI && !IsItemDrag && !LootingStart)
        {
            isRightClick = true;
            Vector3 HitPoint = mouseHitInfo.point;
            HitPoint.y = 0;
            //��ġ�� ���ϴ� �Լ� 
            SetDestination(HitPoint);
        }
    }


    private void leftClickMove()
    {
        if (playerInput.fire && nowMouseOn != MouseCheck.UI)
        {
                Vector3 HitPoint = mouseHitInfo.point;
                HitPoint.y = 0;
                //��ġ�� ���ϴ� �Լ� 
                SetDestination(HitPoint);
        }
    }


    private void SetDestination(Vector3 dest)
    {
        //���� ��ġ�� �����ϱ� 
        destination = dest;

        //������ ��ġ�� ���ϴ� ���ⱸ�ϱ�
        Vector3 dir = destination - transform.position;

        //��ġ�Ϳ� �Ÿ� 1.f ���� ū ���
        if (dir.magnitude > 1f)
        {
            //�����̽� �ٰ� ������ �ʰ� �������� �������� ���� ���
            if (!IsSpaceDown && !LootingStart)
            {
                //�̵� �ִϸ��̼��� ����ȴ�.
                isMove = true;
                animator.SetBool("IsWalk", true);
            }

            //Ŭ���� �������� �÷��̾� �Ĵٺ���
            dir.y = 0;
            transform.forward = dir.normalized;
        }

    }

    private void mouseMove()
    {
        if (isMove && !IsSpaceDown && !LootingStart)
        {//����ƽ���� ���¸� ����
            PlayerInfo.state = PlayerInfo.playerState.RUN;

            var dir = destination - transform.position;
            Vector3 moveDistance =
            dir.normalized * moveSpeed * Time.deltaTime;
            //Ʈ�������� �̿��Ѱ�
            //transform.position += moveDistance;
            //������ �ٵ� �̿��� ���� ������Ʈ ��ġ ����
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

            //Ŭ�� ������ ��ƼŬ ���� ����Ʈ Ŭ�� �̵��϶��� ����
            if (isRightClick)
            {
                Vector3 ClickPos = destination;
                ClickPos.y = 0.2f;
                testObject.SetActive(true);
                testObject.gameObject.transform.position = ClickPos;
            }

            //�ش������� ���������� 
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
        //���콺 �������� ��ȭ�Ұ�쿡�� üũ
        if (mouseCursor.position != Input.mousePosition)
        {
            mouseCursor.position = Input.mousePosition;
            //���콺 �Է� ��ǥ���� ȭ������ ����ĳ������ ����
            if (Physics.Raycast(followCamera.ScreenPointToRay(Input.mousePosition), out mouseHitInfo))
            {

                //�����ɽ������� ���� �浹ü �������� tag�� �޾ƿ� ���� ���콺�� � ������Ʈ���� �ִ��� Ȯ��
                switch (mouseHitInfo.transform.tag)
                {
                    case "DropItem":
                        LogDisappear();
                        InfoAppear();
                        nowMouseOn = MouseCheck.ITEM;
                        break;
                    case "Enemy":
                        LogDisappear();
                        Debug.Log("�� Ŭ��");
                        nowMouseOn = MouseCheck.ENEMY;
                        break;
                    case "Map":
                        LogDisappear();
                        Debug.Log("�Ƕ�");
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
    
    //���콺 ���̷� �浹ü üũ�ϱ�
    private void InfoAppear()
    {
        //Debug.Log("������ �浹��");
        // LogMessage.gameObject.SetActive(true);
        if(mouseHitInfo.transform.GetComponent<ItemPickUp>())
        LogMessage.text = mouseHitInfo.transform.GetComponent<ItemPickUp>().item.itemName;

        if (mouseHitInfo.transform.GetComponent<LootingObject>())
            LogMessage.text = mouseHitInfo.transform.gameObject.name;

        if (mouseHitInfo.transform.GetComponent<mouseOnOff>().playerMouse == null)
        {
            mouseHitInfo.transform.GetComponent<mouseOnOff>().playerMouse = this;
        }

        //���⼭ �ܰ����� Ȱ��ȭ ����
        if (mouseHitInfo.transform.GetComponent<Outline>())
        {
            
            mouseHitInfo.transform.GetComponent<Outline>().enabled = true;
        }


    } // ���콺�� �������� �浹�ϸ� ������ �̸� �α� ����
    private void LogDisappear()
    {
        LogMessage.text = "";
        // LogMessage.gameObject.SetActive(false);
    }//�α� ����� �Լ�
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
        //Ÿ�� ���ϴ� �κ� 
        if (playerInput.fire && mouseHitInfo.transform != null && !IsItemDrag)
        {
            switch (nowMouseOn)
            {
                case MouseCheck.PLAYER:
                    break;
                case MouseCheck.NPC:
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.NPC;
                    leftClickMove();// npc�� �������� ������ �����Ѵ�
                    break;
                case MouseCheck.ITEM:
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.ITEM;
                    leftClickMove();  //�ֿ���� �̵�
                    break;
                case MouseCheck.LootingObj:
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.LootingObj;
                    SetDestination(TargetObject.GetComponent<Transform>().position);
                    break;
                case MouseCheck.ENEMY:   //���ϰ��
                    TargetObject = mouseHitInfo.transform.gameObject;
                    targetType = MouseCheck.ENEMY;
                    // �������� ��� ������ �̵�
                    if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.melee && !PlayerMeleeAttack.IsAttack)
                    {
                        //�����Ϸ� �̵�  ���� ������ ������  �̵�
                        if (!CanInterAction)
                        {
                            leftClickMove();
                        }
                        else // ������ ����
                        {
                            PlayerMeleeAttack.Swing();
                        }
                    }
                    else if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.range && nowMouseOn != MouseCheck.UI)
                    {
                        playerRangeAttack.Shot();
                    }
                    break;
                case MouseCheck.UI: //  ui�϶�
                    break;
                case MouseCheck.NULL:
                    //Debug.Log("�Ƕ� Ŭ��");
                    break;
            }
        }
    }  

    void InterActions()
    {
        //Ÿ���� �������� ���������� ��ȣ�ۿ� �߻��ϴ� ����
        if (TargetObject != null )
        {
            LivingEntity livingEntity = TargetObject.GetComponent<LivingEntity>();
            if (IsCanInterAction(TargetObject.transform.position))
            {
                if (targetType == MouseCheck.ITEM)
                {
                    // Debug.Log("������ �������");
                    inventory.AcquireItem(TargetObject.GetComponent<ItemPickUp>().item);//������ ȹ��  �ϴ� �Լ�
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
            //�����̽�
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
        yield return new WaitForSeconds(0.01f); //��Ī ui ����


        //searchUIGameObject.SetActive(true);

        float CoolDown = searchUIGameObject.GetComponent<SearchUI>().coolDownTime;
        float tempTime = CoolDown;

        if (other.transform.gameObject.GetComponent<LootingObject>().IsBeforeOpen)//������ ���� ���� ������ �ٷ� Ȯ��
        {
            searchUIGameObject.SetActive(false);
            tempTime = 0.00000001f;
        }
        else
        {
            searchUIGameObject.SetActive(true);
        }

        yield return new WaitForSeconds(tempTime);
        //���⼭ ���� �̺�Ʈ�� ����
        if (other.GetComponent<LootingObject>())
        {
            looting_Inventory.lootingObject = other.transform.gameObject.GetComponent<LootingObject>();
        }

        inventory.InventoryOpen();
        looting_Inventory.TryOpenInventory(); //��Ʈ �κ��丮�� ������


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
            //Debug.Log("ui �� �ִ�.");
            nowMouseOn = MouseCheck.UI;
            Debug.Log("UI üũ ��");
            CanRangeAttack = false;
        }
        else
        {
            CanRangeAttack = true;
        }
    
    }



    private void weaponAttackLook()
    {//��Ŭ���� ������ Ŭ���� ������ �ٶ󺸱�
            if (playerInput.fire && nowMouseOn != MouseCheck.UI && !IsItemDrag)
        {
                    LookTargetObject(mouseHitInfo.point);
        }
    }


    //Ŭ���� ������ �ٶ󺸴� �Լ�
    private void LookTargetObject(Vector3 targetPos)
    {
        //���������� ������
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
        {//���� ����
            if (other.tag == "LootingObject" )
            {
                Debug.Log("���ð���");
           
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

