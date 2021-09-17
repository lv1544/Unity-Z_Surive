using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private WeaponManager weaponManager;
    private PlayerInput playerInput;

    public Animator animator; // 무기에 맞는 애니매이션
    public Weapon_Melee weapon_Melee; // 사용할 근접무기
    public bool IsAttack; // 공격 중인지 아닌지
    private PlayerMouse playerMouse; // 플래이어 마우스 컴포넌트
    private Animator playerAnimator; // 애니메이터 컴포넌트
 

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
        Debug.Log("플래이어 스윙");
        playerAnimator.SetBool("IsMeleeAttack", true);
        yield return new WaitForSeconds(0.1f);
        weapon_Melee.meleeArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        weapon_Melee.meleeArea.enabled = false;
        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetBool("IsMeleeAttack", false); // false 가 빨리 되어야 빨리 공격한다
        IsAttack = false;
    }









}
