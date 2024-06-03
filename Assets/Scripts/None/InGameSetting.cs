using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� ���ӽ����̽�
using UnityEngine.UI; // UI�� ����ϱ� ���� ���ӽ����̽�

public class InGameSetting : MonoBehaviour
{
    public GameObject settingsUI; // Setting�̶�� �±׸� ���� UI ������Ʈ�� Inspector���� �巡�׷� �������ݴϴ�.
    public Button mainSceneButton; // Main ������ �̵��ϱ� ���� ��ư. Inspector���� �������ݴϴ�.
    public bool isPaused = false; // ������ �Ͻ� �����Ǿ����� Ȯ���ϴ� �÷���

    public void Start()
    {
        settingsUI.SetActive(false); // ���� �� ���� UI ��Ȱ��ȭ
        //Time.timeScale = 1f; // ���� �ð��� 1�� ������ ���� ����

        //mainSceneButton.onClick.AddListener(GoToMainScene); // ��ư Ŭ�� �̺�Ʈ�� ������ �߰�
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.GMInstance.Get_PlaySceneManager().isCutScene == false) // ESCŰ�� ���ȴ��� Ȯ��
        {
            if (isPaused)
            {
                ResumeGame(); // ������ �̹� �Ͻ� ���� ���¶�� �簳
            }
            else
            {
                PauseGame(); // �ƴ϶�� �Ͻ� ����
            }
        }
    }

    public void PauseGame()
    {
        settingsUI.SetActive(true); // ���� UI Ȱ��ȭ
        Time.timeScale = 0f; // ���� �ð��� 0���� ������ ���� �Ͻ� ����
        isPaused = true; // �Ͻ� ���� �÷��� ����
    }

    public void ResumeGame()
    {
        settingsUI.SetActive(false); // ���� UI ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �ð��� 1�� ������ ���� �簳
        isPaused = false; // �Ͻ� ���� �÷��� ����
    }

    public void GoToMainScene()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }

        GameManager.GMInstance.cur_Scene = Define.Cur_Scene.NONE;
        SceneManager.LoadScene("Loading"); // Main �� �ε�
    }

    public void OnClickRePlay_Btn()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}