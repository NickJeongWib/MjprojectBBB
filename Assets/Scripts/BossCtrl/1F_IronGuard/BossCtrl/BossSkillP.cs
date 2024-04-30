using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class BossSkillP : MonoBehaviour
{
    public Boss_HP_Controller boss_hp_ctrl;

    [SerializeField]
    IronGuard_ObjPool objPool;
    public BossAnimator bossAnimator;
    public GameObject gameClearCanvas;
    public RectTransform gameClearCanvasRect; // ���� Ŭ���� ĵ������ RectTransform

    // public HPtest hp;
    public int IronGuard_MaxHP;

    public GameObject SpiritEffect;
    public GameObject JumpEffect;
    public GameObject DownAttackRange;

    //public GameObject SpiritEffect;
    //public GameObject SpiritEffectPrefab; // SpiritEffect ������ ���� ����

    public BoxCollider boxCollider; //���� ��Ʈ ����
    public CapsuleCollider JumpAttackRange;

    public GameObject Target;
    public Animator animator;
    public BossLookAt bossLookAt;

    public GameObject BossObj;

    public ShotRazer shotRazer_1;
    public ShotRazer shotRazer_2;
    public ShotRazer shotRazer_3;
    public ShotRazer shotRazer_4;
    public ShotRazer shotRazer_5;
    public ShotRazer shotRazer_6;
    public ShotRazer shotRazer_7;
    public ShotRazer shotRazer_8;

    public Transform SpiritPos;
    public Transform JumpPos;

    Transform bossPos;

    
    public GameObject[] razerMaker;
    // public GameObject razerMaker_2;
    [SerializeField]
    GameObject Razer;
    [SerializeField]
    GameObject[] RazerOBJ_Effect;
    // GameObject RazerOBJ_Effect_2;
    [SerializeField]
    int[] SelectRazerNum;

    [SerializeField]
    GameObject[] GuideLine;

    public bool isDead = false;
    [SerializeField]
    Transform Razer_SpawnTr;
    [SerializeField]
    Transform ThrowSword_SpawnTr;
    [SerializeField]
    Transform DownSword_SpawnTr;
    [SerializeField]
    Transform TargetTr;

    void Awake()
    {
        objPool = GetComponent<IronGuard_ObjPool>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        bossPos = GetComponent<Transform>();
        gameClearCanvas.SetActive(false);
    }

    void Start()
    {
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        // �ִ� ü�� ���� ���� ü�� 
        boss_hp_ctrl.BossCurHP = IronGuard_MaxHP;

        StartCoroutine(Think());
    }

    void Update()
    {
        if (boss_hp_ctrl.isDead) // HPtest ��ũ��Ʈ�� isDead�� ����մϴ�.
        {
            StopAllCoroutines();
            return;
        }
    }

    public void Death()
    {
        StopAllCoroutines();
        this.GetComponent<BossLookAt>().isLook = false;
    }

    public void BossThink()
    {
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 4);

        switch (ranAction)
        {
            case 0:
                //�˱� ����
                StartCoroutine(BossSkill1());
                break;

            case 1:
                //3�� ���� ������� ����
                StartCoroutine(BossSkill2());
                break;

            case 2:
                //���� ���� ����
                StartCoroutine(BossSkill3());
                break;

            case 3:
                //������ ����(������) ����
                StartCoroutine(BossSkill4());
                break;

            //case 4:
            //    //������ ����(�𼭸�) ����
            //    StartCoroutine(BossSkill5());
            //    break;
        }
    }
    // TODO ## IronGuard_Skill1 �˱�߻�
    #region IronGuard_Skill1
    IEnumerator BossSkill1()
    {
        animator.SetTrigger("doSpirit");

        yield return new WaitForSeconds(1f);

        // Vector3 targetDirection = Target.transform.position - SpiritPos.position;

        Vector3 targetTrDirection = Target.transform.GetChild(0).transform.position - SpiritPos.position;
        // GameObject SpiritEffectPrf = Instantiate(SpiritEffect);

        GameObject SpiritEffectPrf = objPool.Get_SpiritSword_ObjectFromPool();
        SpiritEffectPrf.transform.position = SpiritPos.position + SpiritPos.forward * 5.0f;
        SpiritEffectPrf.transform.rotation = Quaternion.LookRotation(targetTrDirection);
        // Vector3 SpiritEffectPrf_Quaternion = SpiritEffectPrf.transform.rotation.eulerAngles;
        // Destroy(SpiritEffectPrf, 15.0f);
        // SpiritEffectPrf.transform.forward = targetDirection;

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(GuideLineF(SpiritEffectPrf));
        SpiritEffectPrf.transform.GetChild(4).gameObject.SetActive(true);
        // SpiritEffectPrf.transform.rotation = Quaternion.Euler(SpiritEffectPrf.transform.rotation.x, SpiritEffectPrf.transform.rotation.y, 90.0f);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = false;

        yield return new WaitForSeconds(1.9f);
        // SpiritEffectPrf.transform.GetChild(4).gameObject.SetActive(false);
        SpiritEffectPrf.GetComponent<SwordParticle_Eff>().isBig = true;
        SpiritEffectPrf.GetComponent<SwordParticle_Eff>().isShot = true;

        // Rigidbody SpiritEffect_1 = SpiritEffectPrf.GetComponent<Rigidbody>();
        // SpiritEffect_1.velocity = targetDirection * 5;

        bossLookAt.isLook = true;
        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator GuideLineF(GameObject _obj)
    {
        yield return new WaitForSeconds(0.5f);
        _obj.transform.GetChild(4).gameObject.SetActive(false);
    }
    #endregion

    // TODO ## IronGuard_Skill2 ���� ����
    #region IronGuard_Skill2
    IEnumerator BossSkill2()
    {
        animator.SetTrigger("doDownAttack");
        //
        yield return new WaitForSeconds(1.1f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_1 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);

        GameObject DownAttackRange_1 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_1.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_1.GetComponent<BoxCollider>().enabled = true;
        }

        GuideLine[1].SetActive(false);
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_1));
        DownAttackRange_1.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_1.transform.position = new Vector3(DownAttackRange_1.transform.position.x, 1.0f, DownAttackRange_1.transform.position.z);
        // DownAttackRange_1.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_1.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_1);
        // Destroy(DownAttackRange_1, 10.0f);
        //DownAttackRange_1.transform.forward = targetDirection_1;
        //Vector3 currentEulerAngles_1 = DownAttackRange_1.transform.rotation.eulerAngles;
        //DownAttackRange_1.transform.rotation = Quaternion.Euler(currentEulerAngles_1.x, currentEulerAngles_1.y, 0);
        DownAttackRange_1.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_2 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_2 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_2.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_2.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_2));
        DownAttackRange_2.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_2.transform.position = new Vector3(DownAttackRange_2.transform.position.x, 1.0f, DownAttackRange_2.transform.position.z);
        DownAttackRange_2.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_2);
        //Destroy(DownAttackRange_2, 10.0f);
        //DownAttackRange_2.transform.forward = targetDirection_2;
        //Vector3 currentEulerAngles_2 = DownAttackRange_2.transform.rotation.eulerAngles;
        //DownAttackRange_2.transform.rotation = Quaternion.Euler(currentEulerAngles_2.x, currentEulerAngles_2.y, 0);
        DownAttackRange_2.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_3 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.7f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_3 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_3.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_3.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_3));
        DownAttackRange_3.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_3.transform.position = new Vector3(DownAttackRange_3.transform.position.x, 1.0f, DownAttackRange_3.transform.position.z);
        DownAttackRange_3.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_3);
        //Destroy(DownAttackRange_3, 10.0f);
        //DownAttackRange_3.transform.forward = targetDirection_3;
        //Vector3 currentEulerAngles_3 = DownAttackRange_3.transform.rotation.eulerAngles;
        //DownAttackRange_3.transform.rotation = Quaternion.Euler(currentEulerAngles_3.x, currentEulerAngles_3.y, 0);
        DownAttackRange_3.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;
        animator.SetTrigger("doDownAttack");

        yield return new WaitForSeconds(1.1f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_4 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_4 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_4.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_4.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_4));
        DownAttackRange_4.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_4.transform.position = new Vector3(DownAttackRange_4.transform.position.x, 1.0f, DownAttackRange_4.transform.position.z);
        DownAttackRange_4.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_4);
        //Destroy(DownAttackRange_4, 10.0f);
        //DownAttackRange_4.transform.forward = targetDirection_4;
        //Vector3 currentEulerAngles_4 = DownAttackRange_4.transform.rotation.eulerAngles;
        //DownAttackRange_4.transform.rotation = Quaternion.Euler(currentEulerAngles_4.x, currentEulerAngles_4.y, 0);
        DownAttackRange_4.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_5 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);

        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_5 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_5.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_5.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_5));
        DownAttackRange_5.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_5.transform.position = new Vector3(DownAttackRange_5.transform.position.x, 1.0f, DownAttackRange_5.transform.position.z);
        DownAttackRange_5.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_5);
        // Destroy(DownAttackRange_5, 10.0f);
        //DownAttackRange_5.transform.forward = targetDirection_5;
        //Vector3 currentEulerAngles_5 = DownAttackRange_5.transform.rotation.eulerAngles;
        //DownAttackRange_5.transform.rotation = Quaternion.Euler(currentEulerAngles_5.x, currentEulerAngles_5.y, 0);
        DownAttackRange_5.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_6 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.7f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_6 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_6.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_6.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_6));
        DownAttackRange_6.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_6.transform.position = new Vector3(DownAttackRange_6.transform.position.x, 1.0f, DownAttackRange_6.transform.position.z);
        DownAttackRange_6.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_6);
        // Destroy(DownAttackRange_6, 10.0f);
        //DownAttackRange_6.transform.forward = targetDirection_6;
        //Vector3 currentEulerAngles_6 = DownAttackRange_6.transform.rotation.eulerAngles;
        //DownAttackRange_6.transform.rotation = Quaternion.Euler(currentEulerAngles_6.x, currentEulerAngles_6.y, 0);
        DownAttackRange_6.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(1f);
        //GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;
        animator.SetTrigger("doDownAttack");

        yield return new WaitForSeconds(1.1f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_7 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_7 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_7.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_7.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_7));
        DownAttackRange_7.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_7.transform.position = new Vector3(DownAttackRange_7.transform.position.x, 1.0f, DownAttackRange_7.transform.position.z);
        DownAttackRange_7.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_7);
        // Destroy(DownAttackRange_7, 10.0f);
        //DownAttackRange_7.transform.forward = targetDirection_7;
        //Vector3 currentEulerAngles_7 = DownAttackRange_7.transform.rotation.eulerAngles;
        //DownAttackRange_7.transform.rotation = Quaternion.Euler(currentEulerAngles_7.x, currentEulerAngles_7.y, 0);
        DownAttackRange_7.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);
        //GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_8 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_8 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_8.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_8.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_8));
        DownAttackRange_8.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_8.transform.position = new Vector3(DownAttackRange_8.transform.position.x, 1.0f, DownAttackRange_8.transform.position.z);
        DownAttackRange_8.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_8);
        // Destroy(DownAttackRange_8, 10.0f);
        //DownAttackRange_8.transform.forward = targetDirection_8;
        //Vector3 currentEulerAngles_8 = DownAttackRange_8.transform.rotation.eulerAngles;
        //DownAttackRange_8.transform.rotation = Quaternion.Euler(currentEulerAngles_8.x, currentEulerAngles_8.y, 0);
        DownAttackRange_8.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);
        //GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_9 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.7f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_9 = objPool.GetObjectFromPool();

        // ������Ʈ Ǯ�� �縮�� �� �����Ų collider�� �ٽ� Ų��.
        if (DownAttackRange_9.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_9.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_9));
        DownAttackRange_9.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_9.transform.position = new Vector3(DownAttackRange_9.transform.position.x, 1.0f, DownAttackRange_9.transform.position.z);
        DownAttackRange_9.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_9);
        //Destroy(DownAttackRange_9, 10.0f);
        //DownAttackRange_9.transform.forward = targetDirection_9;
        //Vector3 currentEulerAngles_9 = DownAttackRange_9.transform.rotation.eulerAngles;
        //DownAttackRange_9.transform.rotation = Quaternion.Euler(currentEulerAngles_9.x, currentEulerAngles_9.y, 0);
        DownAttackRange_9.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(2f);

        bossLookAt.isLook = true;
        StartCoroutine(Think());
    }

    public void Combo3_JumpAtk()
    {
        StartCoroutine(Combo3_Jump());
    }

    // ����Ʈ �Ⱥ��̰�
    IEnumerator DownEffect_ActiveF(GameObject effect)
    {
        yield return new WaitForSeconds(1.0f);
        effect.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(2.0f);
        effect.SetActive(false);
    }

    IEnumerator Combo3_Jump()
    {
        bossLookAt.isLook = false;
        Vector3 targetDirection_1 = Target.transform.position - JumpPos.position;;

        yield return new WaitForSeconds(0.3f);

        GameObject DownAttackRange_1 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_1));
        DownAttackRange_1.transform.position = JumpPos.position;
        DownAttackRange_1.transform.rotation = Quaternion.identity;
        DownAttackRange_1.transform.parent = DownSword_SpawnTr;


        //PoolEffectObj.Add(DownAttackRange_1);
        // Destroy(DownAttackRange_1, 10.0f);
        DownAttackRange_1.transform.forward = targetDirection_1;
        Vector3 currentEulerAngles_1 = DownAttackRange_1.transform.rotation.eulerAngles;
        DownAttackRange_1.transform.rotation = Quaternion.Euler(currentEulerAngles_1.x, currentEulerAngles_1.y - 90, 0);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);
    }

    #endregion

    // TODO ## IronGuard_Skill3 9�� ���
    #region IronGuard_Skill3
    IEnumerator BossSkill3()
    {  
        Vector3 jumpStartPosition = transform.position;
        Vector3 targetDirection = Target.transform.position - transform.position;

        Vector3 jumpEndAttackVec = Target.transform.GetChild(0).transform.position - targetDirection.normalized * 5.0f;

        // ���̵� ���� ǥ��

        bossLookAt.isLook = false;
        boxCollider.enabled = false;

        GuideLine[0].SetActive(true);
        GuideLine[0].transform.position = jumpEndAttackVec + bossPos.forward * 15.0f;

        yield return new WaitForSeconds(0.3f);
        // ���̵� ���� ����
        GuideLine[0].SetActive(false);
        // �ش���ġ�� ���ݽ���
        StartCoroutine(JumpDuring(jumpStartPosition, jumpEndAttackVec, 0.5f));

        // ���� �Ÿ�
        // Debug.Log(Vector3.Distance(jumpStartPosition, jumpEndAttackVec));

       

        animator.SetTrigger("doJumpAttack");

        yield return new WaitForSeconds(1.2f);

        // JumpAttackRange.enabled = true;
        Vector3 bossForward = bossPos.position + bossPos.forward * 15.0f;
        GameObject newPrefab = objPool.Get_JumpAtk_ObjectFromPool();
        newPrefab.transform.position = bossForward;
        newPrefab.transform.rotation = Quaternion.identity;
        // GameObject newPrefab = Instantiate(JumpEffect, bossForward, Quaternion.identity);
        // Destroy(newPrefab, 2.0f);

        yield return new WaitForSeconds(0.5f);

        if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) >= 0.0f && Vector3.Distance(jumpStartPosition, jumpEndAttackVec) <= 20.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.8f);
            yield return new WaitForSeconds(0.2f);
        }
        else if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) > 20.0f && Vector3.Distance(jumpStartPosition, jumpEndAttackVec) <= 40.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.9f);
            yield return new WaitForSeconds(0.35f);
        }
        else if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) > 40.0f && Vector3.Distance(jumpStartPosition, jumpEndAttackVec) <= 60.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.8f);
            yield return new WaitForSeconds(0.4f);
        }
        else if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) > 60.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.7f);
            yield return new WaitForSeconds(0.5f);
        }

        boxCollider.enabled = true;
        // animator.SetTrigger("doReturn");
        StartCoroutine(JumpDuring(jumpEndAttackVec, jumpStartPosition, 1.0f));

        yield return new WaitForSeconds(0.8f);

        newPrefab.SetActive(false);
        // JumpAttackRange.enabled = false;
        // boxCollider.enabled = false;

        yield return new WaitForSeconds(1f);

        bossLookAt.isLook = true;
        boxCollider.enabled = true;

        yield return new WaitForSeconds(2f);
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
    #endregion

    // TODO ## IronGuard_Skill4 ���� ������ 
    #region IronGuard_Skill4
    IEnumerator BossSkill4()
    {
        SelectRazerNum = GenerateUniqueRandomValues(4, 0, 7);
        Vector3 bossPosition = transform.position; // ���� ��ġ ���� ����

        Razer.SetActive(true);
        // �������� ���� ������Ʈ�� �ִ� RazerMaker_Ctrl1�� bool�� ����
        Razer.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = false;

        // ������ ���� ���� ���� ����
        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // �������� ���� ������Ʈ Ȱ��ȭ
            razerMaker[SelectRazerNum[i]].SetActive(true);
            RazerOBJ_Effect[SelectRazerNum[i]].SetActive(true);
        }

        bossLookAt.isLook = false;
        
        animator.SetTrigger("doRazer");

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // �������� ���� ������Ʈ �� �߻�
            razerMaker[SelectRazerNum[i]].transform.GetChild(0).GetComponent<ShotRazer>().isShot = true;
        }

        //shotRazer_1.isShot = true;
        //shotRazer_2.isShot = true;
        //shotRazer_3.isShot = true;
        //shotRazer_4.isShot = true;

        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("doRazerReturn");

        yield return new WaitForSeconds(1.5f);
        
        // RazerOBJ_Effect.SetActive(false);

        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // �������� ���� ������Ʈ �� �߻�
            razerMaker[SelectRazerNum[i]].transform.GetChild(0).GetComponent<ShotRazer>().End_Razer_Atk();
            // ��ȯ�� ����Ʈ ����
            RazerOBJ_Effect[SelectRazerNum[i]].SetActive(false);
        }

        //shotRazer_1.End_Razer_Atk();
        //shotRazer_2.End_Razer_Atk();
        //shotRazer_3.End_Razer_Atk();
        //shotRazer_4.End_Razer_Atk();

        // ������ ���� ����
        Razer.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = true;

        //bossLookAt.isLook = true;
        yield return new WaitForSeconds(2f);

        // ������ ���� ���� ���� ����
        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // �������� ���� ������Ʈ Ȱ��ȭ
            razerMaker[SelectRazerNum[i]].SetActive(false);
        }

        StartCoroutine(Think());
    }

    int[] GenerateUniqueRandomValues(int count, int minValue, int maxValue)
    {
        if (count > maxValue - minValue + 1)
        {
            Debug.LogError("Count should be less than or equal to the range of unique values.");
            return null;
        }

        int[] values = new int[maxValue - minValue + 1];

        // �ʱ�ȭ: minValue���� maxValue������ �������� ������ �迭 �ʱ�ȭ
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = minValue + i;
        }

        // Fisher-Yates ���� �˰����� ����Ͽ� �迭 ����
        for (int i = values.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = values[i];
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }

        // count ��ŭ�� ���� ��ȯ
        int[] result = new int[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
    #endregion

    // TODO ## IronGuard_Skill5 ������ ��
    #region IronGuard_Skill5
    //IEnumerator BossSkill5()
    //{
    //    RazerOBJ_Effect_2.SetActive(true);
    //    Vector3 bossPosition = transform.position; // ���� ��ġ ���� ����
    //    // ������ ���� ���� ���� ����
    //    razerMaker_2.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = false;

    //    bossLookAt.isLook = false;

    //    razerMaker_2.SetActive(true);

    //    animator.SetTrigger("doRazer");

    //    // yield return new WaitForSeconds(3f);
    //    yield return new WaitForSeconds(1f);

    //    //shotRazer_5.UseRazer();
    //    //shotRazer_6.UseRazer();
    //    //shotRazer_7.UseRazer();
    //    //shotRazer_8.UseRazer();

    //    shotRazer_5.isShot = true;
    //    shotRazer_6.isShot = true;
    //    shotRazer_7.isShot = true;
    //    shotRazer_8.isShot = true;

    //    yield return new WaitForSeconds(0.5f);

    //    animator.SetTrigger("doRazerReturn");

    //    // yield return new WaitForSeconds(1f);
    //    yield return new WaitForSeconds(1.5f);
    //    RazerOBJ_Effect_2.SetActive(false);
    //    // ������ ����Ʈ �ʱ�ȭ
    //    shotRazer_5.End_Razer_Atk();
    //    shotRazer_6.End_Razer_Atk();
    //    shotRazer_7.End_Razer_Atk();
    //    shotRazer_8.End_Razer_Atk();

    //    // ������ ���� ����
    //    razerMaker_2.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = true;
    //    // razerMaker_2.SetActive(false);

    //    bossLookAt.isLook = true;

    //    yield return new WaitForSeconds(2f);

    //    StartCoroutine(Think());
    //}
    #endregion
}