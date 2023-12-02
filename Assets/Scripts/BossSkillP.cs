using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering.LookDev;
using UnityEditor.Experimental.GraphView;
using GSpawn;

public class BossSkillP : MonoBehaviour
{
    public BossAnimator bossAnimator;

    public GameObject SpiritEffect;
    public GameObject JumpEffect;

    //public GameObject SpiritEffect;
    //public GameObject SpiritEffectPrefab; // SpiritEffect ������ ���� ����

    public BoxCollider boxCollider; //���� ��Ʈ ����
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

    public Transform SpiritPos;

    Transform bossPos;

    public GameObject razerMaker_1;
    public GameObject razerMaker_2;

    public bool isDead = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        bossPos = GetComponent<Transform>();
    }

    void Start()
    {
        StartCoroutine(Think());
    }

    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
    }

    public void BossThink()
    {
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);

        switch (ranAction)
        {
            case 0:
                //�˱� ����
                StartCoroutine(BossSkill1());
                break;

            case 1:
                //3�� ���� ������� ����
                StartCoroutine(BossSkill1());
                break;

            case 2:
                //���� ���� ����
                StartCoroutine(BossSkill1());
                break;

            case 3:
                //������ ����(������) ����
                StartCoroutine(BossSkill1());
                break;

            case 4:
                //������ ����(�𼭸�) ����
                StartCoroutine(BossSkill1());
                break;
        }
    }

    IEnumerator BossSkill1()
    {
        bossLookAt.isLook = false;
        animator.SetTrigger("doSpirit");

        yield return new WaitForSeconds(1f);

        GameObject SpiritEffectPrf = Instantiate(SpiritEffect, SpiritPos.position, Quaternion.identity);

        float scaleSpeed = 1f;

        Vector3 currentScale = SpiritEffectPrf.transform.localScale;
        Vector3 targetScale = new Vector3(3f, 3f, 3f);

        currentScale = Vector3.Lerp(currentScale, targetScale, scaleSpeed * Time.deltaTime);

        SpiritEffectPrf.transform.localScale = currentScale;

        yield return new WaitForSeconds(3f);

        Rigidbody SpiritEffect_1 = SpiritEffectPrf.GetComponent<Rigidbody>();
        SpiritEffect_1.velocity = SpiritPos.forward * 20000;

        yield return new WaitForSeconds(5f);

        bossLookAt.isLook = true;
        StartCoroutine(Think());
    }

    /*IEnumerator BossSkill1()
    {
        Vector3 bossPosition = transform.position; // ���� ��ġ ���� ����

        bossLookAt.isLook = false;

        animator.SetTrigger("doSpirit");

        yield return new WaitForSeconds(1f);

        // Slash �±׸� ���� ������Ʈ�� ã���ϴ�.
        GameObject[] slashObjects = GameObject.FindGameObjectsWithTag("Slash");

        foreach (GameObject slashObject in slashObjects)
        {
            // Slash �±׸� ���� ������Ʈ�� ������ �����մϴ�.
            slashObject.transform.rotation = Quaternion.LookRotation(transform.forward);
            // SpiritEffect �������� �ν��Ͻ�ȭ�Ͽ� ������Ʈ�� �����մϴ�.
            GameObject effect = Instantiate(SpiritEffectPrefab, slashObject.transform.position, slashObject.transform.rotation);
            // ����Ʈ�� �����մϴ�.
            effect.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

        // Slash �±׸� ���� ������Ʈ�� ����Ʈ�� �����մϴ�.
        foreach (GameObject slashObject in slashObjects)
        {
            GameObject effect = slashObject.transform.Find("SpiritEffect(Clone)").gameObject;
            effect.SetActive(false);
        }

        bossLookAt.isLook = true;
    }*/

    IEnumerator BossSkill2()
    {
        yield return null;
    }

    IEnumerator BossSkill3()
    {  
        Vector3 jumpStartPosition = transform.position;
        Vector3 targetDirection = Target.transform.position - transform.position;

        Vector3 jumpEndAttackVec = Target.transform.position - targetDirection.normalized * 5.0f;
        StartCoroutine(JumpDuring(jumpStartPosition, jumpEndAttackVec, 1.4f));

        bossLookAt.isLook = false;
        boxCollider.enabled = false;

        animator.SetTrigger("doJumpAttack");

        yield return new WaitForSeconds(1.5f);

        JumpAttackRange.enabled = true;
        Vector3 bossForward = bossPos.position + bossPos.forward * 5.0f;
        GameObject newPrefab = Instantiate(JumpEffect, bossForward, Quaternion.identity);
        Destroy(newPrefab, 1.0f);

        yield return new WaitForSeconds(2f);

        
        boxCollider.enabled = true;
        animator.SetTrigger("doReturn");
        StartCoroutine(JumpDuring(jumpEndAttackVec, jumpStartPosition, 1.1f));

        yield return new WaitForSeconds(1f);

        JumpAttackRange.enabled = false;
        boxCollider.enabled = false;

        yield return new WaitForSeconds(10f);

        bossLookAt.isLook = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }

    IEnumerator BossSkill4()
    {
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
        StartCoroutine(Think());
    }

    IEnumerator BossSkill5()
    {
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
        StartCoroutine(Think());
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