using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering.LookDev;
using UnityEditor.Experimental.GraphView;
using GSpawn;

public class BossSkillP : MonoBehaviour
{
    public enum BossSkill
    {
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Skill5
    }
    public bool isSkillRunning = false; // ��ų ���� ����

    public BossAnimator bossAnimator;
    public GameObject JumpEffect;
    public GameObject SpiritEffect;

    public BoxCollider boxCollider;
    public CapsuleCollider JumpAttackRange;

    public GameObject Target;
    public Animator animator;
    public BossLookAt bossLookAt;

    public ShotRazer shotRazer_1;
    public ShotRazer shotRazer_2;
    public ShotRazer shotRazer_3;
    public ShotRazer shotRazer_4;
    public ShotRazer shotRazer_5;
    public ShotRazer shotRazer_6;
    public ShotRazer shotRazer_7;
    public ShotRazer shotRazer_8;

    public GameObject razerMaker_1;
    public GameObject razerMaker_2;

    public float skill1Cooldown = 1000f; // ��Ÿ�� �����ϴ� ��
    public float skill2Cooldown = 1000f;
    public float skill3Cooldown = 5;
    public float skill4Cooldown = 10f;
    public float skill5Cooldown = 10f;

    private const float baseCooldown = 5f;

    // ��Ÿ���� ��� �ϴ� �κа� ����Ǵ� �κ��� ��� AttRadyState�� true�� ���� �����ϵ��� ����
    private bool AttRadyState = true;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        // ��ų ��ٿ��� ��� �մϴ�. AttRadyState�� true�� ���� ��ٿ��� �۵��մϴ�.
        if (AttRadyState)
        {
            skill1Cooldown -= Time.deltaTime;
            skill2Cooldown -= Time.deltaTime;
            skill3Cooldown -= Time.deltaTime;
            skill4Cooldown -= Time.deltaTime;
            skill5Cooldown -= Time.deltaTime;
        }
    }

    public void UseSkill(BossSkill skill)
    {
        // ��ų�� ����� �� AttRadyState�� true�� ��쿡�� �����մϴ�.
        if (AttRadyState)
        {
            // �� ��ų�� ��ٿ��� 0 �����̰� AttRadyState�� true�� ���� ��ų�� �����մϴ�.
            switch (skill)
            {
                case BossSkill.Skill1:
                    if (skill1Cooldown <= 0f)
                    {
                        StartCoroutine(BossSkill1());
                        skill1Cooldown = baseCooldown;
                    }
                    break;
                case BossSkill.Skill2:
                    if (skill2Cooldown <= 0f)
                    {
                        StartCoroutine(BossSkill2());
                        skill2Cooldown = baseCooldown;
                    }
                    break;
                case BossSkill.Skill3:
                    if (skill3Cooldown <= 0f)
                    {
                        StartCoroutine(BossSkill3());
                        skill3Cooldown = baseCooldown;
                    }
                    break;
                case BossSkill.Skill4:
                    if (skill5Cooldown <= 0f)
                    {
                        StartCoroutine(BossSkill4());
                        skill4Cooldown = baseCooldown;
                    }
                    break;
                case BossSkill.Skill5:
                    if (skill5Cooldown <= 0f)
                    {
                        StartCoroutine(BossSkill5());
                        skill5Cooldown = baseCooldown;
                    }
                    break;
            }
        }
    }

    IEnumerator BossSkill1()
    {
        if (isSkillRunning)
        {
            yield break; // �ٸ� ��ų�� ���� ���� ���, ���� ��ų �ߴ�
        }

        Vector3 bossPosition = transform.position; // ���� ��ġ ���� ����
        // Skill1�� ������ ���⿡ �ۼ�
        bossLookAt.isLook = false;

        animator.SetTrigger("doSpirit");

        yield return new WaitForSeconds(1f);
        //effect
        //
        SpiritEffect.SetActive(true);


        yield return new WaitForSeconds(5f);
        SpiritEffect.SetActive(false);
        bossLookAt.isLook = true;
    }

    IEnumerator BossSkill2()
    {
        if (isSkillRunning)
        {
            yield break; // �ٸ� ��ų�� ���� ���� ���, ���� ��ų �ߴ�
        }
        // Skill2�� ������ ���⿡ �ۼ�
        yield return null;
    }

    IEnumerator BossSkill3()
    {
        if (isSkillRunning)
        {
            yield break; // �ٸ� ��ų�� ���� ���� ���, ���� ��ų �ߴ�
        }

        isSkillRunning = true;

        Vector3 jumpStartPosition = transform.position;
        Vector3 jumpEndAttackVec = Target.transform.position;
        StartCoroutine(JumpDuring(jumpStartPosition, jumpEndAttackVec, 1.4f));

        bossLookAt.isLook = false;
        boxCollider.enabled = false;

        animator.SetTrigger("doJumpAttack");
        JumpEffect.SetActive(true);

        yield return new WaitForSeconds(3f);

        JumpAttackRange.enabled = true;
        boxCollider.enabled = true;
        animator.SetTrigger("doReturn");
        StartCoroutine(JumpDuring(jumpEndAttackVec, jumpStartPosition, 1.1f));

        yield return new WaitForSeconds(1f);

        JumpAttackRange.enabled = false;
        boxCollider.enabled = false;

        yield return new WaitForSeconds(10f);

        bossLookAt.isLook = true;

        boxCollider.enabled = true;
        JumpEffect.SetActive(false);
        isSkillRunning = false;
    }

    IEnumerator BossSkill4()
    {
        if (isSkillRunning)
        {
            yield break; // �ٸ� ��ų�� ���� ���� ���, ���� ��ų �ߴ�
        }

        isSkillRunning = true;

        Vector3 bossPosition = transform.position; // ���� ��ġ ���� ����

        bossLookAt.isLook = false;

        razerMaker_1.SetActive(true);

        animator.SetTrigger("doRazer");

        yield return new WaitForSeconds(3f);

        shotRazer_1.UseRazer();
        shotRazer_2.UseRazer();
        shotRazer_3.UseRazer();
        shotRazer_4.UseRazer();

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("doRazerReturn");

        yield return new WaitForSeconds(1f);

        razerMaker_1.SetActive(false);

        yield return new WaitForSeconds(10f);

        bossLookAt.isLook = true;

        isSkillRunning = false;
    }

    IEnumerator BossSkill5()
    {
        if (isSkillRunning)
        {
            yield break; // �ٸ� ��ų�� ���� ���� ���, ���� ��ų �ߴ�
        }

        isSkillRunning = true;

        Vector3 bossPosition = transform.position; // ���� ��ġ ���� ����

        bossLookAt.isLook = false;

        razerMaker_2.SetActive(true);

        animator.SetTrigger("doRazer");

        yield return new WaitForSeconds(3f);

        shotRazer_1.UseRazer();
        shotRazer_2.UseRazer();
        shotRazer_3.UseRazer();
        shotRazer_4.UseRazer();

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("doRazerReturn");

        yield return new WaitForSeconds(1f);

        razerMaker_2.SetActive(false);

        yield return new WaitForSeconds(10f);

        bossLookAt.isLook = true;

        isSkillRunning = false;
    }

    IEnumerator JumpDuring(Vector3 startPosition, Vector3 jumpAttackVec, float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            // Dodge ���� �߿� �÷��̾ �����Դϴ�.
            float t = (Time.time - startTime) / duration; // �ð� ���
            transform.position = Vector3.Lerp(startPosition, jumpAttackVec, t); // ���� �������� ���� �������� t��ŭ �ð� �ҿ�
            yield return null;
        }
        transform.position = jumpAttackVec;
    }
}