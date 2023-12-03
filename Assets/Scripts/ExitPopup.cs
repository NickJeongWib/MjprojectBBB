using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopup : MonoBehaviour
{
    public GameObject ExitPop; // ExitPop �̹��� ������Ʈ�� Inspector���� �����մϴ�.

    // Update �Լ����� 'Esc' Ű �Է��� üũ�մϴ�.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPop.SetActive(true); // 'Esc' Ű�� ������ ExitPop�� Ȱ��ȭ�մϴ�.
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
    }
}