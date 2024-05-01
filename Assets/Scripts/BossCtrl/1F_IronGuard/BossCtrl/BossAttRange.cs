using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttRange : MonoBehaviour
{
    public GameObject bossHPCanvas;
    public GameObject IronGuard_obj;
    [SerializeField]
    BossSkillP BossSkill;

    BossAnimator bossAnimator; // BossAnimator ��ũ��Ʈ�� �����ϴ� ����

    // Start is called before the first frame update
    void Start()
    {
        bossAnimator = FindObjectOfType<BossAnimator>(); // �θ� ������Ʈ�� BossAnimator ������Ʈ�� ã�Ƽ� bossAnimator ������ �Ҵ�
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1�� ���� hp ui����
        if (other.gameObject.CompareTag("P"))
        {
            bossHPCanvas.transform.localScale = Vector3.one;

            bossAnimator.AttRadyState = true; // AttRadyState�� true�� ����

            BossSkill.Target = other.gameObject.transform.parent.gameObject;
            BossSkill.boss_hp_ctrl.BossMaxHP = BossSkill.IronGuard_MaxHP;
            BossSkill.boss_hp_ctrl.BossCurHP = BossSkill.IronGuard_MaxHP;

            bossHPCanvas.GetComponent<BossHP_UI_Ctrl>().BossMax_HP = BossSkill.IronGuard_MaxHP;
            bossHPCanvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossSkill.IronGuard_MaxHP;

            bossHPCanvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();

            this.gameObject.GetComponent<BoxCollider>().enabled = false;

        }
    }

    // �÷��̾ ���� ���� �ȿ� ������ ��
    //void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("P")) // �浹�� ������Ʈ�� �±װ� P���
    //    {
    //        bossAnimator.AttRadyState = true; // AttRadyState�� true�� ����

    //        //bossHPCanvas.SetActive(true);
    //        //bossHPCanvas.GetComponent<BossHP_Ctrl>().BossMax_HP = IronGuard_obj.GetComponent<HPtest>().maxHealth;
    //        //bossHPCanvas.GetComponent<BossHP_Ctrl>().BossCur_HP = IronGuard_obj.GetComponent<HPtest>().maxHealth;
    //    }
    //}

    // �÷��̾ ���� ������ ����� ��
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("P")) // �浹�� ������Ʈ�� �±װ� Player���
    //    {
    //        bossAnimator.AttRadyState = false; // AttRadyState�� false�� ����
    //    }
    //}
}