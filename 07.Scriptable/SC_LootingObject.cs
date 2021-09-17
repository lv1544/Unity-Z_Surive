using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LootingObject", menuName = "New LootingObject/LootingObject")]
public class SC_LootingObject : ScriptableObject
{
    public string objectName; //루팅가능 오브젝트 이름
    public GameObject[] itemPrefabs; // 아이템의 프리팹, 자원채취나 죽였을때 아이템이 나와야 하니까

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
