using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Melee : MonoBehaviour
{
    public int damage; //������
    public float rate; // ����
    public BoxCollider meleeArea;  //���ݹ���
    public TrailRenderer trailRenderer;


    private AudioSource MeleeAudioPlayer; // �� �Ҹ� �����
    public AudioClip    SwingClip;  // �ֵθ� �Ҹ�

    public void MeleeAttacking()
    {
        StartCoroutine("Swing");
    }

    IEnumerator Swing()
    {

        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        yield return new WaitForSeconds(0.01f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.3f);
        trailRenderer.enabled = false;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
