using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LootingObject", menuName = "New LootingObject/LootingObject")]
public class SC_LootingObject : ScriptableObject
{
    public string objectName; //���ð��� ������Ʈ �̸�
    public GameObject[] itemPrefabs; // �������� ������, �ڿ�ä�볪 �׿����� �������� ���;� �ϴϱ�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
