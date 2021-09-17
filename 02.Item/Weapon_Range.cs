using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Range : MonoBehaviour
{
    // ���� ���¸� ǥ���ϴµ� ����� Ÿ���� �����Ѵ�
    public enum State
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ������ ��
    }

    //public State state { get; private set; } // ���� ���� ����
    public State state;

    public Transform fireTransform; // �Ѿ� ����Ʈ ��ġ
    public Transform rayPosition; // �Ѿ��� �߻�� ��ġ

    public Transform gunTransform;  // ���� ��ġ 

    public Bullet bullet;

    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    public ParticleSystem shellEjectEffect; // ź�� ���� ȿ��

    private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������

    private AudioSource gunAudioPlayer; // �� �Ҹ� �����
    public AudioClip shotClip; // �߻� �Ҹ�
    public AudioClip reloadClip; // ������ �Ҹ�

    public float damage = 25; // ���ݷ�
    private float fireDistance = 10f; // �����Ÿ�

    public int ammoRemain; // ���� ��ü ź�� �κ��� �ִ� �Ѿ�
    public int magCapacity = 30; // źâ �뷮
    public int magAmmo; // ���� źâ�� �����ִ� ź��


    public float timeBetFire = 0.12f; // �Ѿ� �߻� ����
    public float reloadTime = 3f; // ������ �ҿ� �ð�
    private float lastFireTime; // ���� ���������� �߻��� ����

    public Transform playerTransform;

    public Inventory PlayerInventory;

    public Text ammoText;

    public bool IsReload;

    public bool IsEnableBefore = false;

    public bool IsInAmmo;


    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunTransform = GetComponent<Transform>();
        // ����� ���� �ΰ��� ����
        bulletLineRenderer.positionCount = 2;
        // ���� �������� ��Ȱ��ȭ
        bulletLineRenderer.enabled = false;
        muzzleFlashEffect.gameObject.SetActive(false);
        shellEjectEffect.gameObject.SetActive(false);

       
        
       



    }
    // Ȱ��ȭ �� ��
    private void OnEnable()
    {


        //if (!IsInAmmo)
        //    AmmoInit();

        // magAmmo = magCapacity;
        // ���� ���� ���¸� ���� �� �غ� �� ���·� ����

        //if(magAmmo >0 )
        //state = State.Ready;
        //if(magAmmo == 0)
        //    state = State.Empty;


        // ���������� ���� �� ������ �ʱ�ȭ
        lastFireTime = 0;

        muzzleFlashEffect.gameObject.SetActive(false);
        shellEjectEffect.gameObject.SetActive(false);

        ammoText.enabled = true;

        ////�Ѿ� �ؽ�Ʈ Ȱ��ȭ
        //// PlayerInventory.CheckRifleAmmo();
        ////ammoRemain = PlayerInventory.RifleAmmoNum;



        //ammoText.text = magAmmo + "/" + ammoRemain;
        //gunTransform.forward = playerTransform.forward;
        //fireTransform.forward = gunTransform.forward; // �ѱ� ���� ���� ����


    }
    //��Ȱ��ȭ �� ��
    private void OnDisable()
    {
        if (magAmmo > 0)
        {
            IsInAmmo = true;
        }


        if(ammoText != null)
        ammoText.enabled = false;

    }

    private void Update()
    {
        if (this.isActiveAndEnabled)
        {
            //PlayerInventory.CheckRifleAmmo();
            ammoRemain = PlayerInventory.RifleAmmoNum;
            ammoText.text = magAmmo + "/" + ammoRemain;
        }

        if (magAmmo > 0 && IsReload == false)
            state = State.Ready;
        if (magAmmo == 0)
            state = State.Empty;
    }

    public void AmmoInit()
    {
        // PlayerInventory.CheckRifleAmmo();
        ammoRemain = PlayerInventory.RifleAmmoNum;
        // ���� źâ�� ����ä���

        if (WeaponManager.currentWeapon != this.gameObject)


            //�κ��� �Ѿ� ������ �Ѿ� źâ���� ���ų� ������ 
            if (PlayerInventory.RifleAmmoNum > magCapacity)
            {
                PlayerInventory.useRifleAmmo(magCapacity);
                ammoRemain = PlayerInventory.RifleAmmoNum;
                magAmmo = magCapacity;
            }
            else
            {
                magAmmo = PlayerInventory.RifleAmmoNum;
                PlayerInventory.useRifleAmmo(magCapacity);
                ammoRemain = 0;
            }

        IsInAmmo = true;
    }



    // �߻� �õ�
    public void Fire()
    {
        // ���� ���°� �߻� ������ ����
        // && ������ �� �߻� �������� timeBetFire �̻��� �ð��� ����
        //PlayerInventory.CheckRifleAmmo();

        //ammoRemain = PlayerInventory.RifleAmmoNum;


        if (state == State.Ready &&
           Time.time >= lastFireTime + timeBetFire && magAmmo > 0 )
        {
            gunTransform.forward = playerTransform.forward;
            fireTransform.forward = playerTransform.forward;
            // ������ �� �߻� ������ ����
            lastFireTime = Time.time;
            // ���� �߻� ó�� ����
                Shot();
            Debug.Log("���");
        }
    }

    // ���� �߻� ó��
    private void Shot()
     {
        // ����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
        RaycastHit hit;
        // �Ѿ��� ���� ���� ������ ����
        Vector3 hitPosition = Vector3.zero;

        // ����ĳ��Ʈ(��������, ����, �浹 ���� �����̳�, �����Ÿ�)
        if (Physics.Raycast(fireTransform.position,
            fireTransform.forward, out hit, fireDistance))
        {
            if(hit.collider == null)
             

            Debug.Log(hit.collider.tag);

            // �浹�� �������κ��� IDamageable ������Ʈ�� �������� �õ�
            IDamageable target =
                hit.collider.GetComponent<IDamageable>();
  
            // �������� ���� IDamageable ������Ʈ�� �������µ� �����ߴٸ�
            if (target != null)
            {
                // ������ OnDamage �Լ��� ������Ѽ� ���濡�� ������ �ֱ�
                target.OnDamage(damage, hit.point, hit.normal);
                Debug.Log("�� ����");
            }

            // ���̰� �浹�� ��ġ ����
            hitPosition = hit.point;
        }
        else
        {
            Debug.Log("������ ����");
            // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
            // �Ѿ��� �ִ� �����Ÿ����� ���ư������� ��ġ�� �浹 ��ġ�� ���
            hitPosition = fireTransform.position +
                          fireTransform.forward * fireDistance * 1000;
        }

        // �߻� ����Ʈ ��� ����
        StartCoroutine(ShotEffect(hitPosition));

        // ���� źȯ�� ���� -1
        magAmmo--;
        if (magAmmo <= 0)
        {
            // źâ�� ���� ź���� ���ٸ�, ���� ���� ���¸� Empty���� ����
            state = State.Empty;
        }



    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.gameObject.SetActive(true);
        shellEjectEffect.gameObject.SetActive(true);

        //�ѱ� ȭ�� ���
        muzzleFlashEffect.Play();
        //ź�� ���� ȿ�� ���
        shellEjectEffect.Play();
        //�Ѱ� �Ҹ� ���
        gunAudioPlayer.PlayOneShot(shotClip);

        // ���� �������� �ѱ��� ��ġ
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        //// ���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.SetPosition(1, hitPosition);
        // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
        bulletLineRenderer.enabled = true;

        //�Ѿ� �߻� ����
        //yield return new WaitForSeconds(0.2f);
        //fireTransform.forward = playerTransform.forward;
        //gunTransform.forward = playerTransform.forward;
        //GameObject instantBullet = Instantiate(bullet.gameObject, fireTransform.position, fireTransform.rotation);
        //Rigidbody bulletrigid = instantBullet.GetComponent<Rigidbody>();
        //bulletrigid.velocity = fireTransform.forward * 200;

        //���̴� ���̴�� ������ �Ѿ��� �Ѿ� ��� ������.


        //var dir = hitPosition - transform.position;
        //Vector3 moveDistance =
        //dir.normalized * 200 * Time.deltaTime;
        ////Ʈ�������� �̿��Ѱ�
        ////transform.position += moveDistance;
        ////������ �ٵ� �̿��� ���� ������Ʈ ��ġ ����
        //bulletrigid.MovePosition(bulletrigid.position + moveDistance);





        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� �Ѿ� ������ �����
        bulletLineRenderer.enabled = false;
        muzzleFlashEffect.gameObject.SetActive(false);
        shellEjectEffect.gameObject.SetActive(false);

    }

    // ������ �õ�
    public bool Reload()
    {
        if (state == State.Reloading ||
            ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            // �̹� ������ ���̰ų�, ���� �Ѿ��� ���ų�
            // źâ�� �Ѿ��� �̹� ������ ��� ������ �Ҽ� ����
            return false;
        }

        // ������ ó�� ����

        //PlayerInventory.CheckRifleAmmo();
        ammoRemain = PlayerInventory.RifleAmmoNum;

        StartCoroutine(ReloadRoutine());
        return true;
    }





    // ���� ������ ó���� ����
    private IEnumerator ReloadRoutine()
    {
        //// ���� ���¸� ������ �� ���·� ��ȯ
        //state = State.Reloading;
        //// ������ �Ҹ� ���
        //gunAudioPlayer.PlayOneShot(reloadClip);

        //// ������ �ҿ� �ð� ��ŭ ó���� ����
        //yield return new WaitForSeconds(reloadTime);

        //// źâ�� ä�� ź���� ����Ѵ�
        //int ammoToFill = magCapacity - magAmmo;

        //// źâ�� ä������ ź���� ���� ź�ຸ�� ���ٸ�,
        //// ä������ ź�� ���� ���� ź�� ���� ���� ���δ�
        //if (ammoRemain < ammoToFill)
        //{
        //    ammoToFill = ammoRemain;
        //}

        //// źâ�� ä���
        //magAmmo += ammoToFill;
        //// ���� ź�࿡��, źâ�� ä�ŭ ź���� �A��
        //ammoRemain -= ammoToFill;

        //// ���� ���� ���¸� �߻� �غ�� ���·� ����
        //state = State.Ready;


        // ���� ���¸� ������ �� ���·� ��ȯ
        IsReload = true;

        state = State.Reloading;
        // ������ �Ҹ� ���
        gunAudioPlayer.PlayOneShot(reloadClip);

        //PlayerInventory.CheckRifleAmmo();
        //ammoRemain = PlayerInventory.RifleAmmoNum;

        // ������ �ҿ� �ð� ��ŭ ó���� ����
        yield return new WaitForSeconds(reloadTime);

        // źâ�� ä�� ź���� ����Ѵ�
        int ammoToFill = magCapacity - magAmmo;

        // źâ�� ä������ ź���� ���� ź�ຸ�� ���ٸ�,
        // ä������ ź�� ���� ���� ź�� ���� ���� ���δ�
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
            ammoRemain = 0;
        }

        // źâ�� ä���
        magAmmo += ammoToFill;

        yield return new WaitForSeconds(0.1f);

        PlayerInventory.useRifleAmmo(ammoToFill);

        //PlayerInventory.CheckRifleAmmo();
       // ammoRemain = PlayerInventory.RifleAmmoNum;



        // ���� ���� ���¸� �߻� �غ�� ���·� ����
        state = State.Ready;
        IsReload = false;

    }

}
