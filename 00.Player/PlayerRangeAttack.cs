using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeAttack : MonoBehaviour
{


    public Weapon_Range gun; // ����� ��
    public Transform gunPivot; // �� ��ġ�� ������
    public Transform leftHandMount; // ���� ���� ������, �޼��� ��ġ�� ����
  

    private PlayerInput playerInput; // �÷��̾��� �Է�
    private Animator playerAnimator; // �ִϸ����� ������Ʈ
    private PlayerMouse playerMouse; // �÷��̾� ���콺 ������Ʈ

    public WeaponManager weaponManager;

    public bool CanRangeAttack;

   

    


    private void Start()
    {
        // ����� ������Ʈ���� ��������
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerMouse = GetComponent<PlayerMouse>();

    }

    private void OnEnable()
    {
        //// ���Ͱ� Ȱ��ȭ�� �� �ѵ� �Բ� Ȱ��ȭ
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
            // �Է��� �����ϰ� �� �߻��ϰų� ������
            if (playerInput.reload)
            {
                // ������ �Է� ������ ������
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
            StartCoroutine(ShotAniCoroutine()); //�ѽ�� �ִ� ����
                                                // �߻� �Է� ������ �� �߻�

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

    // ź�� UI ����
    private void UpdateUI()
    {
        //if (gun != null && UIManager.instance != null)
        //{
        //    // UI �Ŵ����� ź�� �ؽ�Ʈ�� źâ�� ź��� ���� ��ü ź���� ǥ��
        //    UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        //}
    }

    // �ִϸ������� IK ���� �ո���� �̻��� �����ؾ���
    private void OnAnimatorIK(int layerIndex)
    {
        if (WeaponManager.currentWeaponType == WeaponManager.WeaponType.range)
        {

            // IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� ���� ������ �����̿� �����
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand,
                leftHandMount.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand,
                leftHandMount.rotation);

            // IK�� ����Ͽ� �������� ��ġ�� ȸ���� ���� ������ �����̿� �����
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
            //Debug.Log("ui �� �ִ�.");
          //  Debug.Log("UI üũ ��");
            CanRangeAttack = false;
        }
        else
        {
            CanRangeAttack = true;
           // Debug.Log("���ݰ���");
        }

    }
}
