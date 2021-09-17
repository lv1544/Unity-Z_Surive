using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "Stage1";
    public RectTransform mouseCursor;

    void Start()
    {
        Cursor.visible = false;
        mouseCursor.transform.SetAsLastSibling();
        mouseCursor.position = Input.mousePosition;

        //�����ɽ�Ʈ ����
        if (mouseCursor.GetComponent<Graphic>())
            mouseCursor.GetComponent<Graphic>().raycastTarget = false;

    }

    private void Update()
    {
        mouseCursor.position = Input.mousePosition;
    }

    public void ClickStart()
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }

    public void ClickExit()
    {
        Debug.Log("����");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }


}
