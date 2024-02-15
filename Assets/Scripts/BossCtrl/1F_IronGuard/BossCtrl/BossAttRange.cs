using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttRange : MonoBehaviour
{
    public GameObject bossHPCanvas;
    public GameObject IronGuard_obj;

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

            //bossHPCanvas.SetActive(true);
            //bossHPCanvas.GetComponent<BossHP_Ctrl>().BossMax_HP = IronGuard_obj.GetComponent<HPtest>().maxHealth;
            //bossHPCanvas.GetComponent<BossHP_Ctrl>().BossCur_HP = IronGuard_obj.GetComponent<HPtest>().maxHealth;
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