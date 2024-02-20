using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Target : MonoBehaviour
{
    [SerializeField]
    Reaper_Controller ReaperCtrl;

    [SerializeField]
    GameObject HP_Canvas;
    [SerializeField]
    Boss_HP_Controller boss_hp_ctrl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<SphereCollider>().enabled = false;

            // ���� �ȿ� ����� �� Ÿ���� �����Ѵ�
            ReaperCtrl.Target = other.gameObject;
            ReaperCtrl.reaperState = ReaperState.RaidStart;

            // HP�� Ȱ��ȭ
            ReaperCtrl.boss_hp_ctrl.Boss_HP_Canvas.SetActive(true);

            // HP�� UI ��Ʈ�ѷ��� ���� ������ HP�� �ش�
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossMax_HP = boss_hp_ctrl.BossMaxHP;
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = boss_hp_ctrl.BossMaxHP;
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();

            // ���̵� ���� �� ���� ü�� �ִ� ü������ ����
            boss_hp_ctrl.BossCurHP = boss_hp_ctrl.BossMaxHP;
            
            StartCoroutine(SeePlayer());
        }
    }

    IEnumerator SeePlayer()
    {
        ReaperCtrl.Reaper_animator.SetTrigger("IsFindPlayer");

        yield return new WaitForSeconds(ReaperCtrl.Reaper_animator.GetCurrentAnimatorStateInfo(0).length + 3.0f);

        // �Ÿ��� ���� ���Ÿ� ���� �ٰŸ� ����
        if (ReaperCtrl.TargetDistance > ReaperCtrl.Skill_Think_Range)
        {
            ReaperCtrl.Reaper_Long_nextAct();
        }
        else if (ReaperCtrl.TargetDistance <= ReaperCtrl.Skill_Think_Range)
        {
            ReaperCtrl.Reaper_Short_nextAct();
        }
    }
}
