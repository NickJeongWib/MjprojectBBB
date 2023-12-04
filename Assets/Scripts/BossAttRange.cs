using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttRange : MonoBehaviour
{
    BossAnimator bossAnimator; // BossAnimator ��ũ��Ʈ�� �����ϴ� ����

    // Start is called before the first frame update
    void Start()
    {
        bossAnimator = FindObjectOfType<BossAnimator>(); // �θ� ������Ʈ�� BossAnimator ������Ʈ�� ã�Ƽ� bossAnimator ������ �Ҵ�
    }

    // �÷��̾ ���� ���� �ȿ� ������ ��
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("P")) // �浹�� ������Ʈ�� �±װ� P���
        {
            bossAnimator.AttRadyState = true; // AttRadyState�� true�� ����
        }
    }

    // �÷��̾ ���� ������ ����� ��
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P")) // �浹�� ������Ʈ�� �±װ� Player���
        {
            bossAnimator.AttRadyState = false; // AttRadyState�� false�� ����
        }
    }
}