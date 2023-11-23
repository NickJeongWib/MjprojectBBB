using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potal : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject toObj;
    private bool isWarping = false; // ���� ������ üũ�ϴ� ������ �߰�

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObj = other.gameObject;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isWarping)
        {
            StartCoroutine(TeleportRoutine());
        }
    }

    IEnumerator TeleportRoutine()
    {
        isWarping = true; // ���� ����
        yield return new WaitForSeconds(1); // 1�� ���
        targetObj.transform.position = toObj.transform.position; // �÷��̾� �̵�
        isWarping = false; // ���� ����
    }
}