using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePopup : MonoBehaviour
{
    public GameObject Updatepopup; // ExitPop �̹��� ������Ʈ�� Inspector���� �����մϴ�.
    public GameObject updateObject; // Update ������Ʈ�� Inspector���� �����մϴ�.

    // Update �Լ����� 'Esc' Ű �Է��� üũ�մϴ�.
    void Update()
    {

    }

    // Ob ��ư�� ������ ���� ������ ������ �Լ��Դϴ�.

    public void OnClickUpdatePop()
    {
        if (updateObject != null) // updateObject�� �����ϸ�
        {
            updateObject.SetActive(true); // �ش� ������Ʈ�� Ȱ��ȭ�մϴ�.
        }
    }
}