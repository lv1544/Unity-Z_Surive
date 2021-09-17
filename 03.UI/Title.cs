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

        //레이케스트 끄기
        if (mouseCursor.GetComponent<Graphic>())
            mouseCursor.GetComponent<Graphic>().raycastTarget = false;

    }

    private void Update()
    {
        mouseCursor.position = Input.mousePosition;
    }

    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
    }

    public void ClickExit()
    {
        Debug.Log("종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }


}
