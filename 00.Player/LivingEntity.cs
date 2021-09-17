using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour ,IDamageable
{
    public float maxHealth;  //시작 체력
   // public float health { get; protected set; } // 현재 체력

    public float health;


    public bool dead { get; protected set; } // 사망 상태
    public event Action onDeath;

    //생명체가 활성화 될 때 상태를 리셋
    protected virtual void OnEnable()
    {
        //사망하지 않은 상태로 시작
        dead = false;
        health = maxHealth;
    }




    //대미지 입는 기능
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //대미지 만큼 체력감소
        health -= damage;

        //체력이 0이하 && 아직 죽지 않았다면 사망처리 실행
        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    //체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)
        {
            //이미 사망하면 회복 못함
            return;
        }

        //체력 추가
        health += newHealth;

        
        

    }

    //사망처리
    public virtual void Die()
    {
        //ondeath 이벤트에 등록된 메서드가 있으면 실행
        if(onDeath != null)
        {
            onDeath();
        }
        //사망 상태를 참으로 변경
        dead = true;
    }


}
