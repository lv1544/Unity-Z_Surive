using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMapIcon : MonoBehaviour
{
    public Transform target;
    public float height;
    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        tr.position = target.position + new Vector3(0, height, 0);
        transform.eulerAngles = new Vector3(90.0f, 0.0f, transform.rotation.z);

    }



}
