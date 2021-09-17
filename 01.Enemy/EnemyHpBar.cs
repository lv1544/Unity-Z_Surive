using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera; //UI 카메라를 담을 변수
    private Canvas canvas;  // 캔버스 
    private RectTransform rectParent;  // 부모의 recttransform 변수를 저장할 변수
    private RectTransform rectHp;  // 자신의 rectransform 변수를 저장할 변수

    public Vector3 offset = Vector3.zero; //hpBar 위치 조절용, offset은 어디에 hpBar를 위치 출력 할지
    public Transform enemyTr; // 적 캐릭터의 위치


    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>(); //부모가 가지고있는 canvas 가져오기, Enemy HpBar canvas임
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();

    }

    private void LateUpdate()
    {
        //윌드 좌표를 스크린 좌표로
        var screenPos = Camera.main.WorldToScreenPoint(enemyTr.position + offset); //월드좌표(3D)를 스

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); //스크린좌표에서 캔버스에서 사용할 수 있는 좌표로 변경?

        rectHp.localPosition = localPos; //그 좌표를 localPos에 저장, 거기에 hpbar를 출력




    }


}
