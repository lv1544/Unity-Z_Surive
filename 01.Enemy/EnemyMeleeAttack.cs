using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float EnemyMeleeDamge;

    Enemy EnemyInfo;




    private void Awake()
    {
        EnemyInfo = GetComponentInParent<Enemy>();
        EnemyMeleeDamge = EnemyInfo.damage;
    }


}
