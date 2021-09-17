using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera; //UI ī�޶� ���� ����
    private Canvas canvas;  // ĵ���� 
    private RectTransform rectParent;  // �θ��� recttransform ������ ������ ����
    private RectTransform rectHp;  // �ڽ��� rectransform ������ ������ ����

    public Vector3 offset = Vector3.zero; //hpBar ��ġ ������, offset�� ��� hpBar�� ��ġ ��� ����
    public Transform enemyTr; // �� ĳ������ ��ġ


    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>(); //�θ� �������ִ� canvas ��������, Enemy HpBar canvas��
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();

    }

    private void LateUpdate()
    {
        //���� ��ǥ�� ��ũ�� ��ǥ��
        var screenPos = Camera.main.WorldToScreenPoint(enemyTr.position + offset); //������ǥ(3D)�� ��

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); //��ũ����ǥ���� ĵ�������� ����� �� �ִ� ��ǥ�� ����?

        rectHp.localPosition = localPos; //�� ��ǥ�� localPos�� ����, �ű⿡ hpbar�� ���




    }


}
