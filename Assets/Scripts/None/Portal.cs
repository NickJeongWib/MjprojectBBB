using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject toObj;

    public AudioSource audioSource; // ȿ������ ����� AudioSource
    public AudioClip portalSound; // ��Ż ȿ����

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P"))
        {
            // �÷��̾� �̵�
            targetObj.transform.position = toObj.transform.position;

            // �ڽ� ������Ʈ�鵵 ���� ��ġ�� �̵�
            foreach (Transform childTransform in targetObj.transform)
            {
                childTransform.position = toObj.transform.position;
            }
        }
        if (other.CompareTag("Player"))
        {
            targetObj = other.gameObject;

            // ��Ż ȿ���� ���
            audioSource.PlayOneShot(portalSound, 1.0f); // ��Ż ȿ���� ��� (������ 1.0)
        }
    }
}