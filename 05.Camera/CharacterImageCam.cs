using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterImageCam : MonoBehaviour
{
    //ĳ���� Ʈ������
    public Transform Player;
    //ī�޶���� �Ÿ�
    public float dist = 1f;
    //ī�޶��� ����
    public float height = 1f;

    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = Player.position - (1 * Vector3.forward * dist) + (Vector3.up * height);
        tr.LookAt(Player);
    }
}
