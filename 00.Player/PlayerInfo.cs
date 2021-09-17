using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : LivingEntity
{
    public enum playerState {IDLE, WALK,RUN,INTERACTION,ATTACK,HURT,ETC}
    public static playerState state;

    public float maxStemina; // ���׹̳�
    public float curStemina = 100;
    public Slider healthSlider; //ü�¹�
    public float curHealth;

    public Text hpPointText;
    public Slider steminaSlider;
    public Text stPointText;

    //��Ÿ �ɷ�ġ
    public int    Level = 3; //����
    public Slider expSlider; //ü�¹�
    public int    curExp = 0; //���� ����ġ
    public float    MaxExp; // �ִ� ����ġ
    public Text   expPointText;
    //���� ������ �ɷ�ġ �߰�
    public float stregth = 10f;
    public float agility = 10f ;
    public float vitality = 10f;
    public float Intelligence = 10f;
    public float luck = 10f;
    public float statPoint = 0f;
    // ��ġ���� �ؽ�Ʈ
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

    private bool isDamage; // ������ Ÿ�̹��� �ֱ����� ����

    public bool       playerInfoUIActivated = false; //�÷��̾� ���� ui ���� ����
    public GameObject playerInfoUI; //�÷��̾� ���� ui ���� ����
    public bool IsCanRooting = false;
    //damagelog ����
    public GameObject damageLog;
    public Canvas playerHpBarCanvas;
    public Vector3 Offset = new Vector3(1, 1, 0);

    //������Ʈ ��������
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
        //ü�� �ʱ�ȭ
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        //���׹̳� �ʱ�ȭ
        steminaSlider.gameObject.SetActive(true);
        steminaSlider.maxValue = maxStemina;
        curStemina = maxStemina;
        steminaSlider.value = curStemina;
        //����ġ��
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
    //��ġ �ؽ�Ʈ ����
    private void UpdateText()
    {
        expSlider.value = curExp;
        hpPointText.text = healthSlider.value + "/" + healthSlider.maxValue; // �ｺ��
        stPointText.text = steminaSlider.value + "/" + steminaSlider.maxValue;//�����̳���
        expPointText.text = expSlider.value + "/" + expSlider.maxValue; // ����ġ��

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


    //ü��ȸ��
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
        _DamageLog.textState = FloatingText.TextState.playerHeal; //ü�� ȸ�� �������� ����

        healthSlider.value = health;
    }

    //�÷��̾� ������ �Ծ����� 
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //�ǰ� ����
        if(!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        //�ǰ� UI������
        GameObject InstantDamageLog = Instantiate<GameObject>(damageLog);
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(this.transform.position + Offset);
        InstantDamageLog.transform.localPosition = uiPosition;
        InstantDamageLog.transform.SetParent(playerHpBarCanvas.gameObject.transform);
        var _DamageLog = InstantDamageLog.GetComponent<FloatingText>();
        _DamageLog.damage = damage;
        _DamageLog.textState = FloatingText.TextState.playerDamage; //  �÷��̾� ������ �α׷� ������


        //����� ����
        base.OnDamage(damage, hitPoint, hitNormal);

        //ü�� �����̴� ����
        healthSlider.value = health;

    }

    public override void Die()
    {
        base.Die();
        //ü�¹� ��Ȱ��ȭ
        healthSlider.gameObject.SetActive(false);
        //����� ���
        playerAudioPlayer.PlayOneShot(deathClip);
        //�ִϸ������� Die Ʈ���� �߻�
        playerAnimator.SetTrigger("Die");
        //�÷��̾� ���� ������Ʈ ��Ȱ��ȭ
        playerMovement.enabled = false;
        playerRangeAttack.enabled = false;
        playerMouse.enabled = false;

    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {//���� ����
            if (other.tag == "EnemyAttack")
            {
                Debug.Log("�÷��̾� ����");
                Transform EnemyTransform = other.GetComponent<Transform>();
                EnemyMeleeAttack enemyScript = other.GetComponent<EnemyMeleeAttack>();
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = other.transform.position - transform.position;

                if (!isDamage)
                    StartCoroutine(OnMeleeDamage(enemyScript.EnemyMeleeDamge, hitPoint, hitNormal));
            }//���� ������Ʈ
            //if (other.tag == "LootingObject")
            //{
            //    Debug.Log("���ð���");
            //    IsCanRooting = true;
            //}


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "LootingObject")
        {
            Debug.Log("���úҰ�");
            IsCanRooting = false ;
        }
    }



    IEnumerator OnMeleeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        isDamage = true;
        //������� ���� ��쿡�� ȿ���� ���
        //����� ����
        OnDamage(damage, hitPoint, hitNormal);
        //���ŵ� ü���� ü�� �����̴��� �ݿ�
        yield return new WaitForSeconds(1.8f);
        isDamage = false; //����Ÿ�� ������ ������ �޴°� ����
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
