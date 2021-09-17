using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    private Text  damageLog; //������ �α� �ؽ�Ʈ
    Color alpha; // ���İ� ������ Color ����

    public enum TextState{enemyDamage,playerDamage,playerHeal}  //��� ��Ȳ���� �ؽ�Ʈ�� ��µǴ°�
    public TextState textState;

    private float moveSpeed; //UI �̵� ���ǵ�
    private float alphaSpeed; //UI ���İ� ��ȯ �ӵ� 
    private float destroyTime; // ������Ʈ ����
    public float  damage; // �α׿� �� ����

    private Camera uiCamera; //UI ī�޶� ���� ����
    private Canvas canvas;  // ĵ���� 
    private RectTransform rectParent;  // �θ��� recttransform ������ ������ ����
    private RectTransform rectHp;  // �ڽ��� rectransform ������ ������ ����

    public Vector3 offset = Vector3.zero; //hpBar ��ġ ������, offset�� ��� Text�� ��ġ ����
    public Transform enemyTr; // �� ĳ������ ��ġ
    Vector3 vector;

    // ���� �ʱ�ȭ
    void Start()
    {
        moveSpeed = 1.0f;
        destroyTime = 0.5f;
        alphaSpeed = 0.5f;
        damageLog = GetComponent<Text>();
        alpha = damageLog.color;
        Invoke("DestroyObject", destroyTime); // �����ð� ���� �ı���
        canvas = GetComponentInParent<Canvas>(); 
        uiCamera = canvas.worldCamera;  // ī�޶�� ���� ���� ī�޶�
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {   //��Ʈ Ʈ�������� ��ġ�� �ð��� ���� ��½�Ų��.
        vector.Set(transform.position.x, transform.position.y + (moveSpeed + Time.deltaTime), transform.position.z);
        transform.position = vector;
        //������ �پ���. 
        destroyTime -= Time.deltaTime;
        // ������ ������ �ı��ȴ�.
        if (destroyTime <= 0)      
        {
            Destroy(this.gameObject);
        }
        //���İ��� �ð��� ���� ������ 0���� �����
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        damageLog.color = alpha;
        // ������ �αװ� �����ϴ� ��Ȳ�� ���� ���� �޶�����.
        switch (textState)
        {
            case TextState.enemyDamage: //���� ������ ������
                damageLog.text = "-" + damage.ToString(); // - �� ���� �տ� ����
                damageLog.color = new Color(255 / 255f, 10 / 255f, 10 / 255f); //������
                break;
            case TextState.playerDamage:// �÷��̰Ű� ������ ������
                damageLog.text = "-" + damage.ToString();// - �� ���� �տ� ����
                damageLog.color = new Color(255 / 255f, 10 / 255f, 10 / 255f);//������
                break;
            case TextState.playerHeal: //�÷��̰Ű� ȸ���� �ɶ�
                damageLog.text = "+" + damage.ToString();// + �� ���� �տ� ����
                damageLog.color = new Color(10 / 255f, 255 / 255f, 10 / 255f);// ���
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
