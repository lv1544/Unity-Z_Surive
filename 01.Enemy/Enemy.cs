using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class Enemy : LivingEntity
{
    public PlayerInfo player; // 그냥 모든 몬스터를 플래이어랑 연결시킴 경험치 주기 위해
    public LayerMask whatIsTarget; // 추적대상 레이어

    private LivingEntity targetEntity; // 추적 대상
    private NavMeshAgent pathFinder;// 경로 계산 ai 에이전트

    public ParticleSystem hitEffect; //  피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망 시 재생할 소리
    public AudioClip hitSound; // 피벽 시 재생할 소리

    private Animator enemyAnimator; // 애니매이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer enemyRenderer; // 렌더러 컴포넌트

    public BoxCollider meleeArea; // 박스 콜라이더
    public bool isChase; // 추적중일때 
    public bool isAttack; //공격중일때 

    private bool isTargetDead;

    public int   EnemyExp; //경험치
    public float damage;// 공격력
    public float timeBetAttack = 0.3f; //  공격 간격
    public float TraceRadius; // 추적 반경
    public float AttackRadius; // 공격 반경
    private float lastAttackTime; // 마지막 공격 시점 
    private Rigidbody rigidbody; // 에너미 리지드 바디
    private bool isDamage; // 데미지 타이밍을 주기위한 변수
    public Transform DropPoint; //아이템 버려지는 위치
    public GameObject[] DropItems; // 드랍아이템

    //HpBarUI 추가한 변수
    public GameObject hpBarPrefab; //Instantiate 메서드로 복제할 프리펩을 담을 변수
    public Vector3 hpBarOffset = new Vector3(-0.5f, 2.4f, 0);
    public Canvas enemyHpBarCanvas;
    public Slider enemyHpBarSlider; //Slider의 초기 세팅, Hp 갱신에 사용할 Slider를 담을 변수
    //damagelog 변수
    public GameObject damageLog;
    //미니맵 아이콘
    public GameObject minimapIcon;


    private bool hasTarget
    {
        get
        {  //추적대상이 존재하고 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
           //그렇지 않다면 false
            return false;
        }
    }

    private bool CanAttack
    {
        get
        {  //추적대상이 존재하고 대상이 사망하지 않았고, 타겟의 위치와의 거리가 어텍 반경보다 작다면
            if (targetEntity != null && !targetEntity.dead && (targetEntity.transform.position - transform.position).magnitude <= AttackRadius)
            {


                return true;
            }
            //그렇지 않다면 false
            return false;
        }


    }




    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerInfo>(); // 플래이어 인포랑 연결 시킴

       

    }

    protected override void OnEnable()
    {
        //LivingEntity의 OnEnable() 실행(상태초기화)
        base.OnEnable();
    }

    // 적 AI의 초기 스펙을 결정하는 셋업 메서드, 적 생성기에서 사용한다.
    public void SetUp(float newHealth,float newDamage, float newSpeed, Color skinColor)
    {
        //체력 설정
        maxHealth = newHealth;
        health = newHealth;
        //공격력 설정
        damage = newDamage;
        //내비메시 에이전트의 이동 속도 설정
        pathFinder.speed = newSpeed;
        //렌더러가 사용 중인 머터리얼의 컬러를 변경, 외형 색이 변함
        enemyRenderer.material.color = skinColor;

    }


    // Start is called before the first frame update
    void Start()
    {//게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        SetHpBar();
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        //hasTarget의 여부에 따라 애니매이션 파라미터를 셋 불 한다
       // enemyAnimator.SetBool("HasTarget", hasTarget);

        enemyAttack();

    }

   void SetHpBar()
    {
        enemyHpBarCanvas = GameObject.Find("Enemy HpBar Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, enemyHpBarCanvas.transform);
        var _hpbar = hpBar.GetComponent<EnemyHpBar>();
        _hpbar.enemyTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;

        enemyHpBarSlider =  hpBar.GetComponent<Slider>();
    }



    //주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        //살아 있는 동안 무한 루프
        while (!dead)
        {
           if(hasTarget && !CanAttack)
            {
                //추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
                enemyAnimator.SetBool("HasTarget", true);
                enemyAnimator.SetBool("IsAttack", false);
                meleeArea.enabled = false;
            }
            else if (CanAttack)
            {
                pathFinder.isStopped = true;
                enemyAnimator.SetBool("HasTarget", false);
                meleeAttackFunc();
               // enemyAnimator.SetBool("IsAttack", true);
            }
            else
            {
                //추적 대상 없음 : AI 이동 중지
                pathFinder.isStopped = true;
                enemyAnimator.SetBool("HasTarget", false);
                enemyAnimator.SetBool("IsAttack", false);
                meleeArea.enabled = false;
                //20유닛의 반지름을 가진 가상의 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
                //단, whatisTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, TraceRadius, whatIsTarget);

                //모든 콜라이더를 순회하면서 살아 있는 LivingEntity 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    //콜라이더로부터 LivingEnitiy 컴포넌트 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEnitiy 컴포넌트가 존재하며, 해당 LivingEnitiy 가 살아 있다면
                    if(livingEntity != null && !livingEntity.dead)
                    {
                        //추적 대상을 해당 LivingEnitiy로 설정
                        targetEntity = livingEntity;
                        //for 문 루프 즉시 정지
                        break;
                    }
                }
            }
            //0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.2f);
        }
    }

    //적 대미지 입었을 때 
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {    //죽지 않았을 경우 피격 효과 발생
        if (!dead)
        {
            //공격받은 지점과 방향으로 파티클 효과 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            //피격 효과음 재생
            enemyAudioPlayer.PlayOneShot(hitSound);
            //피격 UI나오기
            GameObject InstantDamageLog = Instantiate<GameObject>(damageLog);
            Vector3 uiPosition = Camera.main.WorldToScreenPoint(this.transform.position);
            InstantDamageLog.transform.localPosition = uiPosition;
            InstantDamageLog.transform.SetParent(enemyHpBarCanvas.gameObject.transform);
            var _DamageLog = InstantDamageLog.GetComponent<FloatingText>();
            _DamageLog.damage = damage;
            _DamageLog.textState = FloatingText.TextState.enemyDamage; // 적이 맞은 상황으로 정해줌
        }
        // LivingEntity의 Ondamage()를 실행하여 대미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
        //체력 갱신
        enemyHpBarSlider.value = health;
    }


    //사망처리
    public override void Die()
    {
        //아이템 드랍
        enemyDropItem();
        //사망 처리
        base.Die();
        //다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }
        //AI 추적을 중지하고 내비매시 컴포넌트 비활성화
        pathFinder.isStopped = true;
        pathFinder.enabled = false;
        //사망 애니메이션 재생
        enemyAnimator.SetTrigger("Die");
        //사망 효과음 재생
        enemyAudioPlayer.PlayOneShot(deathSound);
        rigidbody.isKinematic = true;
        meleeArea.enabled = false;
        player.curExp += EnemyExp; // 죽었을때 경험치 증가

        if(minimapIcon != null)
        {
            minimapIcon.SetActive(false);
        }


    }

    void enemyDropItem()//적 아이템 드랍
    {
        //게임아이템 버리기
        GameObject instantDropItem = Instantiate(DropItems[0], DropPoint.position, DropPoint.rotation);

    }

    void enemyAttack()
    {
        if(CanAttack)
        {
            StartCoroutine(ZombieMeleeAttack());
        }


    }

    //근접 무기 타격을 위한 충돌 체크
    private void OnTriggerEnter(Collider other)
    {
        if (!dead && !isDamage)
        {
            if (other.tag == "Melee" && other.isTrigger)
            {
                Weapon_Melee meleeWeapon = other.GetComponent<Weapon_Melee>();
                Transform playerTransform = other.GetComponent<Transform>();
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = other.transform.position - transform.position;
                StartCoroutine(OnMeleeDamage(meleeWeapon.damage, hitPoint, hitNormal));
            }
        }
    }




    //몬스터 근접 대미지 받는것
    IEnumerator OnMeleeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
       // Debug.Log("몬스터 맞음");

        isDamage = true;
        //사망하지 않은 경우에만 효과음 재생
       // Debug.Log("몬스터 대미지 입음");

        //넉백 하는 부분
        hitNormal = hitNormal.normalized;
        rigidbody.isKinematic = false; // 리지드 바디 정적 끄기
        float knockBackStat =5;   // 넉백 수치 나중에 여기다 무기 스텟 값으로 넣어준다
        rigidbody.AddForce(hitNormal * -knockBackStat, ForceMode.Impulse);

      
        //대미지 적용
        OnDamage(damage, hitPoint, hitNormal);
        //갱신된 체력을 체력 슬라이더에 반영
        yield return new WaitForSeconds(1f);
        isDamage = false; //무적타임 끝날때 데미지 받는것 가능
        rigidbody.isKinematic = true; // 리지드 바디 정적 다시 설정
    }

    IEnumerator ZombieMeleeAttack()
    {
        //isAttack = true;
        enemyAnimator.SetBool("IsAttack",true);
        yield return new WaitForSeconds(1f);
      //  enemyAnimator.SetBool("IsAttack", false);
       // isAttack = false;
    }

    void meleeAttackFunc()
    {
        Vector3 meleePoint = meleeArea.transform.position;
        Vector3 playerPos = targetEntity.transform.position;
        Vector3 HitPoint = playerPos - meleePoint;
        

        //거리를 이용한 충돌 조건
        if(Math.Abs(HitPoint.magnitude) < 2f && Math.Abs(HitPoint.magnitude) > 1f)
        {
            meleeArea.enabled = true;
        }
        else
        {
            meleeArea.enabled = false;
        }


    }




}




