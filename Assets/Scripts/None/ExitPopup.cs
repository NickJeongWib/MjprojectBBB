using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopup : MonoBehaviour
{
    public GameObject ExitPop; // ExitPop �̹��� ������Ʈ�� Inspector���� �����մϴ�.

    public bool isPaused;

    // Update �Լ����� 'Esc' Ű �Է��� üũ�մϴ�.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ�� ���ȴ��� Ȯ��
        {
            if (isPaused)
            {
                OnClickXb(); // ExitPop�� ��Ȱ��ȭ�Ͽ� �˾��� �ݽ��ϴ�.
            }
            else
            {
                OnClickExitOpBtn(); // ExitPop�� Ȱ��ȭ
            }
        }
    }

    

    // Ob ��ư�� ������ ���� ������ ������ �Լ��Դϴ�.
    public void OnClickOb()
    {
        Application.Quit(); // ������ �����մϴ�.
    }

    // Xb ��ư�� ������ ���� ������ ������ �Լ��Դϴ�.
    public void OnClickXb()
    {
        ExitPop.SetActive(false); // ExitPop�� ��Ȱ��ȭ�Ͽ� �˾��� �ݽ��ϴ�.
        isPaused = false;
    }

    public void OnClickExitOpBtn()
    {
        ExitPop.SetActive(true); // ExitPop�� Ȱ��ȭ
        isPaused = true;
    }
}