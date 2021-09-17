using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool PauseMenuActivated = false;

    [SerializeField]
    private GameObject go_PauseMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) &&  !Gamemanager.isGameover)
        {
            PauseMenuActivated = !PauseMenuActivated;
            if (PauseMenuActivated)
                OpenPauseMenu();
            else
                ClosePauseMenu();

        }
    }

    private void OpenPauseMenu()
    {
        go_PauseMenu.SetActive(true);
        Time.timeScale = 0; //일시정지 시작
        return;
    }

    private void ClosePauseMenu()
    {
        go_PauseMenu.SetActive(false);
        Time.timeScale = 1; //일시정지 끝
        return;
    }
    public void ClickMenu()
    {
        Debug.Log("메뉴");
        SceneManager.LoadScene("GameTiltle");
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
    }

    public void ClickSave()
    {
        Debug.Log("저장");
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
