using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    private Text  damageLog; //데미지 로그 텍스트
    Color alpha; // 알파값 수정용 Color 변수

    public enum TextState{enemyDamage,playerDamage,playerHeal}  //어느 상황에서 텍스트가 출력되는가
    public TextState textState;

    private float moveSpeed; //UI 이동 스피드
    private float alphaSpeed; //UI 알파값 전환 속도 
    private float destroyTime; // 오브젝트 수명
    public float  damage; // 로그에 들어갈 숫자

    private Camera uiCamera; //UI 카메라를 담을 변수
    private Canvas canvas;  // 캔버스 
    private RectTransform rectParent;  // 부모의 recttransform 변수를 저장할 변수
    private RectTransform rectHp;  // 자신의 rectransform 변수를 저장할 변수

    public Vector3 offset = Vector3.zero; //hpBar 위치 조절용, offset은 어디에 Text를 위치 조정
    public Transform enemyTr; // 적 캐릭터의 위치
    Vector3 vector;

    // 변수 초기화
    void Start()
    {
        moveSpeed = 1.0f;
        destroyTime = 0.5f;
        alphaSpeed = 0.5f;
        damageLog = GetComponent<Text>();
        alpha = damageLog.color;
        Invoke("DestroyObject", destroyTime); // 생존시간 이후 파괴됨
        canvas = GetComponentInParent<Canvas>(); 
        uiCamera = canvas.worldCamera;  // 카메라는 현재 월드 카메라
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {   //렉트 트랜스폼의 위치를 시간에 따라 상승시킨다.
        vector.Set(transform.position.x, transform.position.y + (moveSpeed + Time.deltaTime), transform.position.z);
        transform.position = vector;
        //수명이 줄어든다. 
        destroyTime -= Time.deltaTime;
        // 수명이 지나면 파괴된다.
        if (destroyTime <= 0)      
        {
            Destroy(this.gameObject);
        }
        //알파값을 시간에 따라 서서히 0으로 만든다
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        damageLog.color = alpha;
        // 데미지 로그가 발행하는 상황에 따라 색이 달라진다.
        switch (textState)
        {
            case TextState.enemyDamage: //좀비가 데미지 입을때
                damageLog.text = "-" + damage.ToString(); // - 를 숫자 앞에 적음
                damageLog.color = new Color(255 / 255f, 10 / 255f, 10 / 255f); //빨강색
                break;
            case TextState.playerDamage:// 플래이거가 데미지 잆을때
                damageLog.text = "-" + damage.ToString();// - 를 숫자 앞에 적음
                damageLog.color = new Color(255 / 255f, 10 / 255f, 10 / 255f);//빨강색
                break;
            case TextState.playerHeal: //플래이거가 회복이 될때
                damageLog.text = "+" + damage.ToString();// + 를 숫자 앞에 적음
                damageLog.color = new Color(10 / 255f, 255 / 255f, 10 / 255f);// 녹색
                break;
            default:
                break;
        }

    }



    private void DestroyObject()
    {
        Destroy(gameObject);
    }



}
