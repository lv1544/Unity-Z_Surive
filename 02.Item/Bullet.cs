using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 BulletRayPos;
    public bool     IsDead;
    public float time = 0;

    public float DestroyTime = 1000f;


    private void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
       // AfterHiT();


    
    }

    public void SetBulletRayPos(Vector3 _rayHit)
    {
        BulletRayPos = _rayHit;
    }

    public void AfterHiT()
    {
        Vector3 dir = BulletRayPos - transform.position;

        if (dir.magnitude < 0.1)
            IsDead = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy"  && collision.gameObject.tag == "MapObject")
        {
            Destroy(gameObject);
        }


    }


}
