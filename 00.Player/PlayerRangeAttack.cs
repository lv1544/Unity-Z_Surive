using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeAttack : MonoBehaviour
{


    public Weapon_Range gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
  

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트
    private PlayerMouse playerMouse; // 플래이어 마우스 컴포넌트

    public WeaponManager weaponManager;

    public bool CanRangeAttack;

   

    


    private void Start()
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerMouse = GetComponent<PlayerMouse>();

    }

    private void OnEnable()
    {
        //// 슈터가 활성화될 때 총도 함께 활성화
        ///
        //if(weaponManager.CanRangeAttack)
        //gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        //if (!weaponManager.CanRangeAttack)
        // gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        OnMouseDown();


        if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.range && CanRangeAttack)
        {
            // 입력을 감지하고 총 발사하거나 재장전
            if (playerInput.reload)
            {
                // 재장전 입력 감지시 재장전
                if (gun.Reload())
                {
                    playerAnimator.SetTrigger("Reload");
                    gun.StartCoroutine("ReloadRoutine");


                  // StartCoroutine(ReloadAniRoutine());
                }
            }
        }
    }

    public void Shot()
    {
        if (playerInput.fire && gun.state == Weapon_Range.State.Ready)
        {
            StartCoroutine(ShotAniCoroutine()); //총쏘는 애니 실행
                                                // 발사 입력 감지시 총 발사

        }
    }

    private IEnumerator ReloadAniRoutine()
    {
        playerAnimator.SetBool("IsReload", true);
        yield return new WaitForSeconds(0.001f);
        playerAnimator.SetBool("IsReload", false);
        gun.StartCoroutine("ReloadRoutine");
        //yield return new WaitForSeconds(gun.reloadTime);

  
      

    }

    // 탄약 UI 갱신
    private void UpdateUI()
    {
        //if (gun != null && UIManager.instance != null)
        //{
        //    // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
        //    UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        //}
    }

    // 애니메이터의 IK 갱신 손모양이 이상함 수정해야함
    private void OnAnimatorIK(int layerIndex)
    {
        if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.range)
        {

            // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand,
                leftHandMount.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand,
                leftHandMount.rotation);

            // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            //playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            //playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

            //playerAnimator.SetIKPosition(AvatarIKGoal.RightHand,
            //    rightHandMount.position);
            //playerAnimator.SetIKRotation(AvatarIKGoal.RightHand,
            //    rightHandMount.rotation);
        }
    }

    IEnumerator ShotAniCoroutine()
    {
        playerAnimator.SetBool("IsShot", true);
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetBool("IsShot", false);
        gun.Fire();

    }


    public void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("ui 에 있다.");
          //  Debug.Log("UI 체크 중");
            CanRangeAttack = false;
        }
        else
        {
            CanRangeAttack = true;
           // Debug.Log("공격가능");
        }

    }
}
