using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� ���ӽ����̽�
using UnityEngine.UI; // UI�� ����ϱ� ���� ���ӽ����̽�

public class InGameSetting : MonoBehaviour
{
    public GameObject settingsUI; // Setting�̶�� �±׸� ���� UI ������Ʈ�� Inspector���� �巡�׷� �������ݴϴ�..
    private bool isPaused = false; // ������ �Ͻ� �����Ǿ����� Ȯ���ϴ� �÷���

    void Start()
    {
        settingsUI.SetActive(false); // ���� �� ���� UI ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �ð��� 1�� ������ ���� 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ�� ���ȴ��� Ȯ��
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

    void PauseGame()
    {
        settingsUI.SetActive(true); // ���� UI Ȱ��ȭ
        Time.timeScale = 0f; // ���� �ð��� 0���� ������ ���� �Ͻ� ����
        isPaused = true; // �Ͻ� ���� �÷��� ����
    }

    void ResumeGame()
    {
        settingsUI.SetActive(false); // ���� UI ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �ð��� 1�� ������ ���� �簳
        isPaused = false; // �Ͻ� ���� �÷��� ����
    }

    void GoToMainScene()
    {

        SceneManager.LoadScene("Main"); // Main �� �ε�
    }
}