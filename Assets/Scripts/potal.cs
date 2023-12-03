using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject toObj;
    private bool isWarping = false;

    public AudioSource audioSource; // ȿ������ ����� AudioSource
    public AudioClip portalSound; // ��Ż ȿ����

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObj = other.gameObject;

            // ��Ż ȿ���� ���
            audioSource.PlayOneShot(portalSound, 1.0f); // ��Ż ȿ���� ��� (������ 1.0)
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
        isWarping = true;
        yield return new WaitForSeconds(1);

        // �÷��̾� �̵�
        targetObj.transform.position = toObj.transform.position;

        // �ڽ� ������Ʈ�鵵 ���� ��ġ�� �̵�
        foreach (Transform childTransform in targetObj.transform)
        {
            childTransform.position = toObj.transform.position;
        }

        isWarping = false;
    }
}