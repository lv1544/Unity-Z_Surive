using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : LivingEntity
{
    public enum playerState {IDLE, WALK,RUN,INTERACTION,ATTACK,HURT,ETC}
    public static playerState state;

    public float maxStemina; // 스테미나
    public float curStemina = 100;
    public Slider healthSlider; //체력바
    public float curHealth;

    public Text hpPointText;
    public Slider steminaSlider;
    public Text stPointText;

    //기타 능력치
    public int    Level = 3; //레벨
    public Slider expSlider; //체력바
    public int    curExp = 0; //현재 경험치
    public float    MaxExp; // 최대 경험치
    public Text   expPointText;
    //여기 밑으로 능력치 추가
    public float stregth = 10f;
    public float agility = 10f ;
    public float vitality = 10f;
    public float Intelligence = 10f;
    public float luck = 10f;
    public float statPoint = 0f;
    // 수치관련 텍스트
    public Text levelNum;
    public Text stregthNum;
    public Text agilityNum;
    public Text vitalityNum;
    public Text IntelligenceNum;
    public Text luckNum;
    public Text statPointNum;


    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickUpClip;

    private AudioSource playerAudioPlayer;
    private Animator playerAnimator;

    private PlayerMovement    playerMovement;
    private PlayerRangeAttack playerRangeAttack;
    private PlayerMouse       playerMouse;

    private bool isDamage; // 데미지 타이밍을 주기위한 변수

    public bool       playerInfoUIActivated = false; //플래이어 인포 ui 관련 변수
    public GameObject playerInfoUI; //플래이어 인포 ui 관련 변수
    public bool IsCanRooting = false;
    //damagelog 변수
    public GameObject damageLog;
    public Canvas playerHpBarCanvas;
    public Vector3 Offset = new Vector3(1, 1, 0);

    //컴포넌트 가져오기
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
       
        playerMovement = GetComponent<PlayerMovement>();
        playerRangeAttack = GetComponent<PlayerRangeAttack>();
        playerMouse = GetComponent<PlayerMouse>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //체력 초기화
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        //스테미나 초기화
        steminaSlider.gameObject.SetActive(true);
        steminaSlider.maxValue = maxStemina;
        curStemina = maxStemina;
        steminaSlider.value = curStemina;
        //경험치바
        expSlider.gameObject.SetActive(true);
        expSlider.maxValue = MaxExp;
        expSlider.value = curExp;

        playerMovement.enabled = true;
        playerRangeAttack.enabled = true;
        playerMouse.enabled = true;

        stregth = 10f;
        agility = 10f;
        vitality = 10f;
        Intelligence = 10f;
        luck = 10f;


    }
    //수치 텍스트 수정
    private void UpdateText()
    {
        expSlider.value = curExp;
        hpPointText.text = healthSlider.value + "/" + healthSlider.maxValue; // 헬스바
        stPointText.text = steminaSlider.value + "/" + steminaSlider.maxValue;//스테이나바
        expPointText.text = expSlider.value + "/" + expSlider.maxValue; // 경험치바

        levelNum.text = Level.ToString();
        stregthNum.text = stregth.ToString() ;
        agilityNum.text = agility.ToString();
        vitalityNum.text = vitality.ToString();
        IntelligenceNum.text = Intelligence.ToString();
        luckNum.text = luck.ToString();
        statPointNum.text = statPoint.ToString();

    }

    private void UpdateHpText()
    {
        hpPointText.text = healthSlider.value + "/" + healthSlider.maxValue;
    }

    private void UpdateStText()
    {
        stPointText.text = steminaSlider.value + "/" + steminaSlider.maxValue;
    }




    private void Update()
    {
        UpdateText();
        LevelUp();
        TryOpenPlayerInfoUI();
        //UpdateHpText();
        //UpdateStText();



    }

   public void LevelUp()
    {
        if(curExp >= MaxExp)
        {
            Level += 1;
            curExp = 0;
            MaxExp *= 1.5f;

            stregth += 3;
            agility += 2;
            Intelligence += 2;
            vitality += 1;
            luck += 3;


          //  statPoint = 10;

           

            expSlider.maxValue = Mathf.RoundToInt(MaxExp);
            return;
        }

    }


    //체력회복
    public override void RestoreHealth(float newHealth)
    {
        if (health + newHealth > maxHealth)
        {
            newHealth = maxHealth - health;
            base.RestoreHealth(newHealth);
        }
        else
            base.RestoreHealth(newHealth);

        GameObject InstantDamageLog = Instantiate<GameObject>(damageLog);
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(this.transform.position + Offset);
        InstantDamageLog.transform.localPosition = uiPosition;
        InstantDamageLog.transform.SetParent(playerHpBarCanvas.gameObject.transform);
        var _DamageLog = InstantDamageLog.GetComponent<FloatingText>();
        _DamageLog.damage = newHealth;
        _DamageLog.textState = FloatingText.TextState.playerHeal; //체력 회복 됬을때로 설정

        healthSlider.value = health;
    }

    //플래이어 데미지 입었을때 
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //피격 사운드
        if(!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        //피격 UI나오기
        GameObject InstantDamageLog = Instantiate<GameObject>(damageLog);
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(this.transform.position + Offset);
        InstantDamageLog.transform.localPosition = uiPosition;
        InstantDamageLog.transform.SetParent(playerHpBarCanvas.gameObject.transform);
        var _DamageLog = InstantDamageLog.GetComponent<FloatingText>();
        _DamageLog.damage = damage;
        _DamageLog.textState = FloatingText.TextState.playerDamage; //  플래이어 데미지 로그로 정해줌


        //대미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);

        //체력 슬라이더 갱신
        healthSlider.value = health;

    }

    public override void Die()
    {
        base.Die();
        //체력바 비활성화
        healthSlider.gameObject.SetActive(false);
        //사망음 재생
        playerAudioPlayer.PlayOneShot(deathClip);
        //애니메이터의 Die 트리거 발생
        playerAnimator.SetTrigger("Die");
        //플래이어 조작 컴포넌트 비활성화
        playerMovement.enabled = false;
        playerRangeAttack.enabled = false;
        playerMouse.enabled = false;

    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {//적의 공격
            if (other.tag == "EnemyAttack")
            {
                Debug.Log("플래이어 맞음");
                Transform EnemyTransform = other.GetComponent<Transform>();
                EnemyMeleeAttack enemyScript = other.GetComponent<EnemyMeleeAttack>();
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = other.transform.position - transform.position;

                if (!isDamage)
                    StartCoroutine(OnMeleeDamage(enemyScript.EnemyMeleeDamge, hitPoint, hitNormal));
            }//루팅 오브젝트
            //if (other.tag == "LootingObject")
            //{
            //    Debug.Log("루팅가능");
            //    IsCanRooting = true;
            //}


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "LootingObject")
        {
            Debug.Log("루팅불가");
            IsCanRooting = false ;
        }
    }



    IEnumerator OnMeleeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        isDamage = true;
        //사망하지 않은 경우에만 효과음 재생
        //대미지 적용
        OnDamage(damage, hitPoint, hitNormal);
        //갱신된 체력을 체력 슬라이더에 반영
        yield return new WaitForSeconds(1.8f);
        isDamage = false; //무적타임 끝날때 데미지 받는것 가능
    }

    private void TryOpenPlayerInfoUI()
    {
        if (Input.GetKeyDown(KeyCode.S) && !Gamemanager.isGameover)
        {
            playerInfoUIActivated = !playerInfoUIActivated;

            if (playerInfoUIActivated)
                OpenPlayerInfo();
            else
                ClosePlayerInfo();
        }
    }

    private void OpenPlayerInfo()
    {
        playerInfoUI.SetActive(true);
    }


    private void ClosePlayerInfo()
    {
        playerInfoUI.SetActive(false);
    }




}
