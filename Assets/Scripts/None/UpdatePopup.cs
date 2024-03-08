using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePopup : MonoBehaviour
{
    public GameObject updateUI; // Update �±׸� ���� UI ������Ʈ�� Inspector���� �巡�׷� �������ݴϴ�.
    public Button[] openUpdateButtons; // Update UI�� Ȱ��ȭ�ϱ� ���� ��ư��. Inspector���� �������ݴϴ�.
    public Button[] closeUpdateButtons; // Update UI�� ��Ȱ��ȭ�ϱ� ���� X ��ư��. Inspector���� �������ݴϴ�.

    void Start()
    {
        updateUI.SetActive(false); // ���� �� Update UI ��Ȱ��ȭ

        foreach (Button button in openUpdateButtons)
        {
            button.onClick.AddListener(OpenUpdateUI); // �� ��ư Ŭ�� �̺�Ʈ�� ������ �߰�
        }

        foreach (Button button in closeUpdateButtons)
        {
            button.onClick.AddListener(CloseUpdateUI); // �� X ��ư Ŭ�� �̺�Ʈ�� ������ �߰�
        }
    }

    void OpenUpdateUI()
    {
        updateUI.SetActive(true); // Update UI Ȱ��ȭ
    }

    void CloseUpdateUI()
    {
        updateUI.SetActive(false); // Update UI ��Ȱ��ȭ
    }
}