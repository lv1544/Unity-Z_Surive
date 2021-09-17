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
        Time.timeScale = 0; //�Ͻ����� ����
        return;
    }

    private void ClosePauseMenu()
    {
        go_PauseMenu.SetActive(false);
        Time.timeScale = 1; //�Ͻ����� ��
        return;
    }
    public void ClickMenu()
    {
        Debug.Log("�޴�");
        SceneManager.LoadScene("GameTiltle");
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }

    public void ClickSave()
    {
        Debug.Log("����");
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
