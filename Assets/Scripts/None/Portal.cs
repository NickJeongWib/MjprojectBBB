using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // ���� ����Ʈ ������Ʈ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WarpPlayer"))
        {
            // �÷��̾ ���� ����Ʈ ��ġ�� �̵�
            other.transform.position = toObj.transform.position;

            // ��Ż ȿ���� ��� ������ ����
            //audioSource.PlayOneShot(portalSound, 1.0f); // ��Ż ȿ���� ��� �ڵ�
        }
    }
}
