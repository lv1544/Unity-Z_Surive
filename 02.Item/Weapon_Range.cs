using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Range : MonoBehaviour
{
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    //public State state { get; private set; } // 현재 총의 상태
    public State state;

    public Transform fireTransform; // 총알 이펙트 위치
    public Transform rayPosition; // 총알이 발사될 위치

    public Transform gunTransform;  // 총의 위치 

    public Bullet bullet;

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public float damage = 25; // 공격력
    private float fireDistance = 10f; // 사정거리

    public int ammoRemain; // 남은 전체 탄약 인벤에 있는 총알
    public int magCapacity = 30; // 탄창 용량
    public int magAmmo; // 현재 탄창에 남아있는 탄약


    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 3f; // 재장전 소요 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    public Transform playerTransform;

    public Inventory PlayerInventory;

    public Text ammoText;

    public bool IsReload;

    public bool IsEnableBefore = false;

    public bool IsInAmmo;


    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunTransform = GetComponent<Transform>();
        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;
        muzzleFlashEffect.gameObject.SetActive(false);
        shellEjectEffect.gameObject.SetActive(false);

       
        
       



    }
    // 활성화 될 때
    private void OnEnable()
    {


        //if (!IsInAmmo)
        //    AmmoInit();

        // magAmmo = magCapacity;
        // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경

        //if(magAmmo >0 )
        //state = State.Ready;
        //if(magAmmo == 0)
        //    state = State.Empty;


        // 마지막으로 총을 쏜 시점을 초기화
        lastFireTime = 0;

        muzzleFlashEffect.gameObject.SetActive(false);
        shellEjectEffect.gameObject.SetActive(false);

        ammoText.enabled = true;

        ////총알 텍스트 활성화
        //// PlayerInventory.CheckRifleAmmo();
        ////ammoRemain = PlayerInventory.RifleAmmoNum;



        //ammoText.text = magAmmo + "/" + ammoRemain;
        //gunTransform.forward = playerTransform.forward;
        //fireTransform.forward = gunTransform.forward; // 총구 방향 전방 통일


    }
    //비활성화 될 때
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
        // 현재 탄창을 가득채우기

        if (WeaponManager.currentWeapon != this.gameObject)


            //인벤에 총알 갯수가 총알 탄창보다 같거나 많을때 
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



    // 발사 시도
    public void Fire()
    {
        // 현재 상태가 발사 가능한 상태
        // && 마지막 총 발사 시점에서 timeBetFire 이상의 시간이 지남
        //PlayerInventory.CheckRifleAmmo();

        //ammoRemain = PlayerInventory.RifleAmmoNum;


        if (state == State.Ready &&
           Time.time >= lastFireTime + timeBetFire && magAmmo > 0 )
        {
            gunTransform.forward = playerTransform.forward;
            fireTransform.forward = playerTransform.forward;
            // 마지막 총 발사 시점을 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리 실행
                Shot();
            Debug.Log("사격");
        }
    }

    // 실제 발사 처리
    private void Shot()
     {
        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 총알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        // 레이캐스트(시작지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if (Physics.Raycast(fireTransform.position,
            fireTransform.forward, out hit, fireDistance))
        {
            if(hit.collider == null)
             

            Debug.Log(hit.collider.tag);

            // 충돌한 상대방으로부터 IDamageable 오브젝트를 가져오기 시도
            IDamageable target =
                hit.collider.GetComponent<IDamageable>();
  
            // 상대방으로 부터 IDamageable 오브젝트를 가져오는데 성공했다면
            if (target != null)
            {
                // 상대방의 OnDamage 함수를 실행시켜서 상대방에게 데미지 주기
                target.OnDamage(damage, hit.point, hit.normal);
                Debug.Log("적 맞음");
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            Debug.Log("맞은거 없음");
            // 레이가 다른 물체와 충돌하지 않았다면
            // 총알이 최대 사정거리까지 날아갔을때의 위치를 충돌 위치로 사용
            hitPosition = fireTransform.position +
                          fireTransform.forward * fireDistance * 1000;
        }

        // 발사 이펙트 재생 시작
        StartCoroutine(ShotEffect(hitPosition));

        // 남은 탄환의 수를 -1
        magAmmo--;
        if (magAmmo <= 0)
        {
            // 탄창에 남은 탄약이 없다면, 총의 현재 상태를 Empty으로 갱신
            state = State.Empty;
        }



    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.gameObject.SetActive(true);
        shellEjectEffect.gameObject.SetActive(true);

        //총구 화염 재생
        muzzleFlashEffect.Play();
        //탄피 배출 효과 재생
        shellEjectEffect.Play();
        //총격 소리 재생
        gunAudioPlayer.PlayOneShot(shotClip);

        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        //// 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        //총알 발사 시점
        //yield return new WaitForSeconds(0.2f);
        //fireTransform.forward = playerTransform.forward;
        //gunTransform.forward = playerTransform.forward;
        //GameObject instantBullet = Instantiate(bullet.gameObject, fireTransform.position, fireTransform.rotation);
        //Rigidbody bulletrigid = instantBullet.GetComponent<Rigidbody>();
        //bulletrigid.velocity = fireTransform.forward * 200;

        //레이는 레이대로 나가고 총알은 총알 대로 나간다.


        //var dir = hitPosition - transform.position;
        //Vector3 moveDistance =
        //dir.normalized * 200 * Time.deltaTime;
        ////트랜스폼을 이용한것
        ////transform.position += moveDistance;
        ////리지드 바디를 이용해 게임 오브젝트 위치 변경
        //bulletrigid.MovePosition(bulletrigid.position + moveDistance);





        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
        muzzleFlashEffect.gameObject.SetActive(false);
        shellEjectEffect.gameObject.SetActive(false);

    }

    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading ||
            ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            // 이미 재장전 중이거나, 남은 총알이 없거나
            // 탄창에 총알이 이미 가득한 경우 재장전 할수 없다
            return false;
        }

        // 재장전 처리 시작

        //PlayerInventory.CheckRifleAmmo();
        ammoRemain = PlayerInventory.RifleAmmoNum;

        StartCoroutine(ReloadRoutine());
        return true;
    }





    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        //// 현재 상태를 재장전 중 상태로 전환
        //state = State.Reloading;
        //// 재장전 소리 재생
        //gunAudioPlayer.PlayOneShot(reloadClip);

        //// 재장전 소요 시간 만큼 처리를 쉬기
        //yield return new WaitForSeconds(reloadTime);

        //// 탄창에 채울 탄약을 계산한다
        //int ammoToFill = magCapacity - magAmmo;

        //// 탄창에 채워야할 탄약이 남은 탄약보다 많다면,
        //// 채워야할 탄약 수를 남은 탄약 수에 맞춰 줄인다
        //if (ammoRemain < ammoToFill)
        //{
        //    ammoToFill = ammoRemain;
        //}

        //// 탄창을 채운다
        //magAmmo += ammoToFill;
        //// 남은 탄약에서, 탄창에 채운만큼 탄약을 뺸다
        //ammoRemain -= ammoToFill;

        //// 총의 현재 상태를 발사 준비된 상태로 변경
        //state = State.Ready;


        // 현재 상태를 재장전 중 상태로 전환
        IsReload = true;

        state = State.Reloading;
        // 재장전 소리 재생
        gunAudioPlayer.PlayOneShot(reloadClip);

        //PlayerInventory.CheckRifleAmmo();
        //ammoRemain = PlayerInventory.RifleAmmoNum;

        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);

        // 탄창에 채울 탄약을 계산한다
        int ammoToFill = magCapacity - magAmmo;

        // 탄창에 채워야할 탄약이 남은 탄약보다 많다면,
        // 채워야할 탄약 수를 남은 탄약 수에 맞춰 줄인다
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
            ammoRemain = 0;
        }

        // 탄창을 채운다
        magAmmo += ammoToFill;

        yield return new WaitForSeconds(0.1f);

        PlayerInventory.useRifleAmmo(ammoToFill);

        //PlayerInventory.CheckRifleAmmo();
       // ammoRemain = PlayerInventory.RifleAmmoNum;



        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
        IsReload = false;

    }

}
