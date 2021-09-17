using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour ,IDamageable
{
    public float maxHealth;  //���� ü��
   // public float health { get; protected set; } // ���� ü��

    public float health;


    public bool dead { get; protected set; } // ��� ����
    public event Action onDeath;

    //����ü�� Ȱ��ȭ �� �� ���¸� ����
    protected virtual void OnEnable()
    {
        //������� ���� ���·� ����
        dead = false;
        health = maxHealth;
    }




    //����� �Դ� ���
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        //����� ��ŭ ü�°���
        health -= damage;

        //ü���� 0���� && ���� ���� �ʾҴٸ� ���ó�� ����
        if(health <= 0 && !dead)
        {
            Die();
        }
    }

    //ü���� ȸ���ϴ� ���
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)
        {
            //�̹� ����ϸ� ȸ�� ����
            return;
        }

        //ü�� �߰�
        health += newHealth;

        
        

    }

    //���ó��
    public virtual void Die()
    {
        //ondeath �̺�Ʈ�� ��ϵ� �޼��尡 ������ ����
        if(onDeath != null)
        {
            onDeath();
        }
        //��� ���¸� ������ ����
        dead = true;
    }


}
