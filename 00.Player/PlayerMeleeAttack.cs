using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private WeaponManager weaponManager;
    private PlayerInput playerInput;

    public Animator animator; // ���⿡ �´� �ִϸ��̼�
    public Weapon_Melee weapon_Melee; // ����� ��������
    public bool IsAttack; // ���� ������ �ƴ���
    private PlayerMouse playerMouse; // �÷��̾� ���콺 ������Ʈ
    private Animator playerAnimator; // �ִϸ����� ������Ʈ
 

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        playerInput = GetComponent<PlayerInput>();
        playerMouse = GetComponent<PlayerMouse>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Swing()
    {
        StartCoroutine(MeleeAttack());
    }


    
    public IEnumerator MeleeAttack()
    {
        IsAttack = true;
        Debug.Log("�÷��̾� ����");
        playerAnimator.SetBool("IsMeleeAttack", true);
        yield return new WaitForSeconds(0.1f);
        weapon_Melee.meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        weapon_Melee.meleeArea.enabled = false;
        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetBool("IsMeleeAttack", false); // false �� ���� �Ǿ�� ���� �����Ѵ�
        IsAttack = false;
    }









}
