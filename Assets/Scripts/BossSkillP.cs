using UnityEngine;
using System.Collections;

public class BossSkillP : MonoBehaviour
{
    public enum BossSkill
    {
        Skill1,
        Skill2,
        Skill3,
        Skill4
    }

    public void UseSkill(BossSkill skill)
    {
        //Debug.Log("Skill " + (int)skill + " is used.");

        switch (skill)
        {
            case BossSkill.Skill1:
                StartCoroutine(BossSkill1());
                break;
            case BossSkill.Skill2:
                StartCoroutine(BossSkill2());
                break;
            case BossSkill.Skill3:
                StartCoroutine(BossSkill3());
                break;
            case BossSkill.Skill4:
                StartCoroutine(BossSkill4());
                break;
        }
    }

    IEnumerator BossSkill1()
    {
        // Skill1�� ������ ���⿡ �ۼ�
        yield return null;
    }

    IEnumerator BossSkill2()
    {
        // Skill2�� ������ ���⿡ �ۼ�
        yield return null;
    }

    IEnumerator BossSkill3()
    {
        // Skill3�� ������ ���⿡ �ۼ�
        yield return null;
    }

    IEnumerator BossSkill4()
    {
        // Skill4�� ������ ���⿡ �ۼ�
        yield return null;
    }
}