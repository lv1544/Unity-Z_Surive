using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Melee : MonoBehaviour
{
    public int damage; //데미지
    public float rate; // 공속
    public BoxCollider meleeArea;  //공격범위
    public TrailRenderer trailRenderer;


    private AudioSource MeleeAudioPlayer; // 총 소리 재생기
    public AudioClip    SwingClip;  // 휘두른 소리

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
