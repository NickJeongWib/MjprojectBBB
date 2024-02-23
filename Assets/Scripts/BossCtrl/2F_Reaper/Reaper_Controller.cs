using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReaperState
{
    RaidStart,      // 0
    Idle,           // 1
    Move,           // 2
    Teleport,
    
    BaseAtk_0,      // 
    BaseAtk_1,      // 

    Dark_Hand,      // 
    Dark_Decline,   // 
    Dark_Soul,
}

public enum Reaper_Awake
{
    NORMAL,
    AWAKENING,
}

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    [Header("-----Reaper State-----")]
    public ReaperState reaperState;
    public Reaper_Awake reaperAwakeState;
    public bool isLock;               // ���� ���� ����
    public bool isAttacking;          // ���� �� ���� ����

    [Header("-----Reaper Reference-----")]
    public Reaper_Atk_Range Reaper_AtkRange; //  
    public Boss_HP_Controller boss_hp_ctrl;  // HP ��Ʈ�ѷ�
    [SerializeField]
    Reaper_ObjPool reaper_ObjPoolRef;

    [Header("-----Reaper Variable-----")]
    public GameObject Target;       // �÷��̾�
    public float TargetDistance;    // �÷��̾���� �Ÿ�
    [SerializeField]
    GameObject DeathSycthe;

    [Header("-----Reaper State Variable-----")]
    public int MaxHP;   // ���� ü��

    [SerializeField]
    float Boss_RotSpeed;    //  ȸ�� �ӵ�
    [SerializeField]
    float moveSpeed;        // ������ �ӵ�
    public float Skill_Think_Range; // ��ų ���� ���� ����
    [SerializeField]
    GameObject Skill_Pos; // ��ų ���� ��ġ
    [SerializeField]
    GameObject Skill_Look; // ��ų�� �ٶ󺸴� ����
    Vector3 dir; // ����
    // Rigidbody rigid;

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // �ִϸ�����
    public bool isMove;         // �̵� ����
    [SerializeField]
    float nextActTime;

    [Header("-----Awakening-----")]
    [SerializeField]
    GameObject Aura;

    [Header("-----Skill_ BaseAtk_0-----")]
    [SerializeField]
    float BaseAtk_0_LockTime; // �⺻ ����_0 ȸ�� ����
    [SerializeField]
    GameObject BaseAtk_0_Eff;

    [Header("-----Skill_ BaseAtk_1-----")]
    [SerializeField]
    float BaseAtk_1_LockTime; // // �⺻ ����_1 ȸ�� ����
    [SerializeField]
    GameObject BaseAtk_1_Eff;

    [Header("-----Skill_Dark_Decline-----")]
    public float Dark_Decline_Delay; // ����� ��� ��ų ��� ������
    [SerializeField]
    GameObject Dark_Decline_Slash;
    [SerializeField]
    float DarkDecline_Rot;
    [SerializeField]
    float DarkDecline_Dis;  // ����� ��� ���� �Ÿ�
    [SerializeField]
    float Decline_LockTime; // ȸ�� ���� �ð�
    [SerializeField]
    float Decline_UnLockTime; // ȸ�� ���� ���� �ð�

    [Header("-----Skill_Dark_Hand-----")]
    [SerializeField]
    GameObject CastingEff;

    [Header("-----Skill_Dark_Soul-----")]
    [SerializeField]
    GameObject DarkSoul_Skill_Eff;
    [SerializeField]
    float Slow_RotSpeed;
    [SerializeField]
    float DarkSoul_Running_Time;

    #region Reaper_Rotate
    public override void LookAtPlayer()
    {
        // �÷��̾ ã�� �� ���ٸ� ���� ����
        if (Target == null || isLock)
            return;

        // �÷��̾ �ٶ󺸵���
        // this.transform.LookAt(Target.transform);

        dir = Target.transform.position - transform.position;
        // y�� ���� ����
        dir.y = 0.0f;

        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (Boss_RotSpeed - Slow_RotSpeed));
    }
    #endregion

    #region Reaper_Move
    public override void Move()
    {
        // �÷��̾ ã�� �� ���ٸ� ���� ����
        if (Target == null)
            return;

        if (isMove && isAttacking == false)
        {
            Reaper_animator.SetBool("isMove", isMove);
            // ������ �̵�
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        //else if (!isMove && isAttacking == false)
        //{
        //    reaperState = ReaperState.Idle;
        //    Reaper_animator.SetBool("isMove", isMove);
        //}
    }
    #endregion

    #region Start/Update
    // Start is called before the first frame update
    void Start()
    {
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        reaper_ObjPoolRef = GetComponent<Reaper_ObjPool>();
        Reaper_animator = GetComponent<Animator>();
        reaperAwakeState = Reaper_Awake.NORMAL;

        // �ִ� ü�� ���� ���� ü�� 
        boss_hp_ctrl.BossMaxHP = MaxHP;
    }

    private void FixedUpdate()
    {
        // ���� ���°� �����̴� Move�� ����� �ָ�
        if (reaperState == ReaperState.Move && TargetDistance > Skill_Think_Range)
        {
            Move();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ null�� �ƴ϶��
        if (Target != null)
        {
            TargetDistance = Vector3.Distance(Target.transform.position, this.transform.position);
        }

        LookAtPlayer();
    }
    #endregion

    #region Reaper_Next_Skill
    public void Reaper_Short_nextAct()
    {
        // ���� �������̶�� return
        if (isAttacking == true)
        {
            return;
        }

        // �Ÿ��� ������
        if (TargetDistance <= Skill_Think_Range)
        {
            if (reaperAwakeState == Reaper_Awake.NORMAL)
            {
                // ���� ���� ����
                int randomIndex = Random.Range(0, 3);

                switch (randomIndex)
                {
                    case 0:
                        StartCoroutine(BaseAtk_0());
                        break;
                    case 1:
                        StartCoroutine(BaseAtk_1());
                        break;
                    case 2:
                        StartCoroutine(Dark_Decline());
                        break;
                }
            }
            else if (reaperAwakeState == Reaper_Awake.AWAKENING)
            {
                // ���� ���� ����
                int randomIndex = Random.Range(0, 4);

                switch (3)
                {
                    case 0:
                        StartCoroutine(BaseAtk_0());
                        break;
                    case 1:
                        StartCoroutine(BaseAtk_1());
                        break;
                    case 2:
                        StartCoroutine(Dark_Decline());
                        break;
                    case 3:
                        StartCoroutine(Dark_Soul());
                        break;
                }
            }       
        }
    }

    public void Reaper_Long_nextAct()
    {
        // ���� �������̶�� return
        if (isAttacking == true)
        {
            return;
        }

        // �Ÿ��� �ָ�
        if (TargetDistance > Skill_Think_Range)
        {
            // ���Ÿ�
            int randomIndex = Random.Range(0, 3);

            switch (randomIndex)
            {
                case 0:
                    // �̵�
                    isMove = true;
                    reaperState = ReaperState.Move;
                    break;

                case 1:
                    StartCoroutine(Teleport());
                    break;

                case 2:
                    StartCoroutine(Dark_Hand());
                    break;
            }
        }
    }
    #endregion

    #region Reaper_PlayerCheck

    public void PlayerCheck()
    {
        if (TargetDistance < Skill_Think_Range)
        {
            isMove = false;
            Reaper_Short_nextAct();
        }
    }

    #endregion

    #region Reaper Equip / UnEquip Scythe

    public void Equip_Scythe()
    {
        DeathSycthe.SetActive(true);
    }

    public void UnEquip_Scythe()
    {
        DeathSycthe.SetActive(false);
    }

    #endregion

    #region Reaper_Idle
    public void Reaper_Idle()
    {
        // reaperState = ReaperState.Idle;
    }
    #endregion

    #region Awakening
    public void Aura_On()
    {
        Aura.SetActive(true);
    }
    #endregion

    #region Boss_Reaper_Teleport
    IEnumerator Teleport()
    {
        reaperState = ReaperState.Teleport;
        Reaper_animator.SetTrigger("Teleport");


        // �÷��̾� �������� ������ ������ �Ÿ��� �ڷ���Ʈ
        float randomAngle = Random.Range(0f, 360f);

        // �÷��̾��� ��ġ���� ���Ϸ� �� ��ŭ ��ġ���� * (Skill_Think_Range - 3.0f)��ŭ �Ÿ��� ��ġ
        Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        Vector3 randomPosition = Target.transform.position + randomDirection * (Skill_Think_Range - 5.0f);

        // Y ��ǥ�� 0���� ����
        randomPosition.y = 1.5f;

        // �ڷ���Ʈ
        transform.position = randomPosition;

        yield return new WaitForSeconds(0.1f);

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }

    #endregion

    #region Boss_Atk_0
    // TODO ## Reaper_BaseAtk_0
    IEnumerator BaseAtk_0()
    {
        // ���� ���� ����
        reaperState = ReaperState.BaseAtk_0;
        // �ִϸ��̼� ����
        Reaper_animator.SetTrigger("BaseAtk_0");
        // ���� ��
        isAttacking = true;
        // �̵� ����
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);

        yield return new WaitForSeconds(BaseAtk_0_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length - BaseAtk_0_LockTime);
        // Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        isLock = false;

        // 4�� �� ���� ����
        yield return new WaitForSeconds(nextActTime);
        isAttacking = false;

        // ����
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // ���� ����
            reaperAwakeState = Reaper_Awake.AWAKENING;
            Reaper_animator.SetTrigger("Awakening");
            nextActTime = 1.0f;
            // �����ൿ �ð� + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }


        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }
    // �⺻ ���� ������ ����Ʈ ����
    public void BaseAtk0_Eff()
    {
        StartCoroutine(Play_BaseAtk0_Eff());
    }

    IEnumerator Play_BaseAtk0_Eff()
    {
        BaseAtk_0_Eff.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        BaseAtk_0_Eff.SetActive(false);
    }

    #endregion

    #region Boss_Atk_1
    // TODO ## Reaper_BaseAtk_1
    IEnumerator BaseAtk_1()
    {
        // ���� ���� ����
        reaperState = ReaperState.BaseAtk_1;
        // �ִϸ��̼� ����
        Reaper_animator.SetTrigger("BaseAtk_1");
        // ���� ��
        isAttacking = true;
        // �̵� ����
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);

        yield return new WaitForSeconds(BaseAtk_1_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length - BaseAtk_1_LockTime);
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        isLock = false;

        // 4�� �� ����
        yield return new WaitForSeconds(nextActTime);

        // ���� ����
        isAttacking = false;

        // ����
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // ���� ����
            reaperAwakeState = Reaper_Awake.AWAKENING;
            Reaper_animator.SetTrigger("Awakening");
            nextActTime = 1.0f;
           
            // �����ൿ �ð� + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }


        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }

    // �⺻ ���� ������ ����Ʈ ����
    public void BaseAtk1_Eff()
    {
        StartCoroutine(Play_BaseAtk1_Eff());
    }

    IEnumerator Play_BaseAtk1_Eff()
    {
        BaseAtk_1_Eff.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        BaseAtk_1_Eff.SetActive(false);
    }
    #endregion

    #region Boss_Atk_2_DarkDecline
    // TODO ## Reaper_DarkDecline
    IEnumerator Dark_Decline()
    {
        // ���� �� ����� ���
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // ���� ���� ����
            reaperState = ReaperState.Dark_Decline;
            // ���� ����
            isAttacking = true;
            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            // �ִϸ��̼� ����_1
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
            GameObject DarkDeclineEff_1 = reaper_ObjPoolRef.GetDarkDeclineFromPool();
            DarkDeclineEff_1.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d1 = DarkDeclineEff_1.transform.position - Skill_Look.transform.position;
            d1.y = 0.0f;
            Quaternion q1 = Quaternion.LookRotation(d1);
            DarkDeclineEff_1.transform.rotation = q1 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // ������Ʈ Ǯ�� ��Ȱ��ȭ
            //DarkDeclineEff_1.SetActive(false);

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
            // �ִϸ��̼� ����_2
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
            GameObject DarkDeclineEff_2 = reaper_ObjPoolRef.GetDarkDeclineFromPool();
            // DarkDeclineEff_2.transform.forward = Vector3.right;
            DarkDeclineEff_2.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d2 = DarkDeclineEff_2.transform.position - Skill_Look.transform.position;
            d2.y = 0.0f;
            Quaternion q2 = Quaternion.LookRotation(d2);
            DarkDeclineEff_2.transform.rotation = q2 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;


            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // �ִϸ��̼� ����_3
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
            GameObject DarkDeclineEff_3 = reaper_ObjPoolRef.GetDarkDeclineFromPool();
            //DarkDeclineEff_3.transform.forward = Vector3.right;
            DarkDeclineEff_3.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;
            Vector3 d3 = DarkDeclineEff_3.transform.position - Skill_Look.transform.position;
            d3.y = 0.0f;
            Quaternion q3 = Quaternion.LookRotation(d3);
            DarkDeclineEff_3.transform.rotation = q3 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 4�� �� ����
            yield return new WaitForSeconds(nextActTime);
            DarkDeclineEff_1.SetActive(false);
            DarkDeclineEff_2.SetActive(false);
            DarkDeclineEff_3.SetActive(false);

            // ���� ����
            isAttacking = false;

            // ����
            if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
            {
                // ���� ����
                reaperAwakeState = Reaper_Awake.AWAKENING;
                Reaper_animator.SetTrigger("Awakening");
                nextActTime = 1.0f;
                // �����ൿ �ð� + 1
                yield return new WaitForSeconds(nextActTime + 1.0f);
            }

            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct();
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct();
            }
        }
        else if (reaperAwakeState == Reaper_Awake.AWAKENING) // ����� ��� ���� ��---------------------------------------
        {
            // ���� ���� ����
            reaperState = ReaperState.Dark_Decline;
            // ���� ����
            isAttacking = true;
            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            // �ִϸ��̼� ����_1
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
            GameObject DarkDeclineEff_1 = reaper_ObjPoolRef.GetDarkDecline2FromPool();
            DarkDeclineEff_1.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d1 = DarkDeclineEff_1.transform.position - Skill_Look.transform.position;
            d1.y = 0.0f;
            Quaternion q1 = Quaternion.LookRotation(d1);
            DarkDeclineEff_1.transform.rotation = q1 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // ������Ʈ Ǯ�� ��Ȱ��ȭ
            //DarkDeclineEff_1.SetActive(false);

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
            // �ִϸ��̼� ����_2
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
            GameObject DarkDeclineEff_2 = reaper_ObjPoolRef.GetDarkDecline2FromPool();
            // DarkDeclineEff_2.transform.forward = Vector3.right;
            DarkDeclineEff_2.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d2 = DarkDeclineEff_2.transform.position - Skill_Look.transform.position;
            d2.y = 0.0f;
            Quaternion q2 = Quaternion.LookRotation(d2);
            DarkDeclineEff_2.transform.rotation = q2 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;


            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // �ִϸ��̼� ����_3
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
            GameObject DarkDeclineEff_3 = reaper_ObjPoolRef.GetDarkDecline2FromPool();
            //DarkDeclineEff_3.transform.forward = Vector3.right;
            DarkDeclineEff_3.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;
            Vector3 d3 = DarkDeclineEff_3.transform.position - Skill_Look.transform.position;
            d3.y = 0.0f;
            Quaternion q3 = Quaternion.LookRotation(d3);
            DarkDeclineEff_3.transform.rotation = q3 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 4�� �� ����
            yield return new WaitForSeconds(nextActTime);
            DarkDeclineEff_1.SetActive(false);
            DarkDeclineEff_2.SetActive(false);
            DarkDeclineEff_3.SetActive(false);

            // ���� ����
            isAttacking = false;

            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct();
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct();
            }
        }
    }
    // ������ ����Ʈ ����
    public void Dark_Decline_Eff()
    {
        StartCoroutine(Play_Dark_Decline_Eff());
    }

    IEnumerator Play_Dark_Decline_Eff()
    {
        Dark_Decline_Slash.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        Dark_Decline_Slash.SetActive(false);
    }
    #endregion

    #region Reaper_Atk_3_DarkHand
    // TODO ## Reaper_DarkHand / Reaper_DarkHand2
    IEnumerator Dark_Hand()
    {
        // ����� ���� ���� �� 
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // ���� ����
            reaperState = ReaperState.Dark_Hand;
            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Dark_Hand");

            // ���� ��
            isAttacking = true;
            isLock = true;

            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);

            // �ִϸ��̼��� ������ �� ��
            yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
            // Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

            // 2�� ��
            yield return new WaitForSeconds(2.0f);
            // ���� ����
            isAttacking = false;
            isLock = false;

            // ����
            if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
            {
                // ���� ����
                reaperAwakeState = Reaper_Awake.AWAKENING;
                Reaper_animator.SetTrigger("Awakening");
                nextActTime = 1.0f;
                // �����ൿ �ð� + 1
                yield return new WaitForSeconds(nextActTime + 1.0f);
            }

            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct();
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct();
            }
        }
        else // ����� ���� ���� ��
        {
            // ���� ����
            reaperState = ReaperState.Dark_Hand;
            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("UnEquip_Scythe");
            // ���� ��
            isAttacking = true;
            isLock = true;
            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            // �ִϸ��̼��� ������ �� ��
            yield return new WaitForSeconds(2.0f);

            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Casting");
            // �ִϸ��̼��� ������ �� ��
            yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length + 2.0f);
            isLock = false;
            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Dark_Hand_2");
            // �ִϸ��̼��� ������ �� ��
            yield return new WaitForSeconds(4.0f);

            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Dark_Hand_2");
            // �ִϸ��̼��� ������ �� ��
            yield return new WaitForSeconds(4.0f);

            isLock = true;
            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Equip_Scythe");

            // �ִϸ��̼��� ������ �� ��
            yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length + nextActTime);

            isLock = false;
            // ���� ����
            isAttacking = false;
      
            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct();
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct();
            }
        }
    }

    // TODO ## Reaper DarkHand ����Ʈ ����
    public void DarkHand_Eff()
    {
        StartCoroutine(Play_DarkHand_Eff());
    }

    IEnumerator Play_DarkHand_Eff()
    {
        Vector3 Pos = Target.transform.position;

        yield return new WaitForSeconds(1.5f);

        // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
        GameObject DarkHnad_Explosion = reaper_ObjPoolRef.GetDarkHandFromPool();
        DarkHnad_Explosion.transform.position = Pos;

        yield return new WaitForSeconds(5.0f);
        DarkHnad_Explosion.SetActive(false);
    }

    // ĳ���� ����Ʈ 
    public void Casting_Eff()
    {
        StartCoroutine(Play_Casting_Eff());
    }
    IEnumerator Play_Casting_Eff()
    {
        CastingEff.SetActive(true);
        CastingEff.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(5.0f);

        CastingEff.SetActive(false);
    }

    // ��������Ʈ ����
    // TODO ## Reaper DarkHand2 ����Ʈ ����
    public void DarkHand2_Eff()
    {
        StartCoroutine(Play_DarkHand2_Eff());
    }

    IEnumerator Play_DarkHand2_Eff()
    {
        Vector3 Pos = Target.transform.position;

        yield return new WaitForSeconds(0.5f);

        // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
        GameObject DarkHnad2_Explosion = reaper_ObjPoolRef.GetDarkHand2FromPool();
        DarkHnad2_Explosion.transform.GetChild(2).gameObject.SetActive(true);

        DarkHnad2_Explosion.transform.position = Pos;

        yield return new WaitForSeconds(10.0f);
        // �� ��ġ
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(0).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(1).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.SetActive(false);
    }

    #endregion

    #region Reaper_Atk_4_DarkSoul
    // TODO ## Reaper_DarkSoul
    IEnumerator Dark_Soul()
    {
        // ���� ���� ����
        reaperState = ReaperState.Dark_Soul;
        // ���� ����
        isAttacking = true;
        // �̵� ����
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);
        // �ִϸ��̼� ����_1
        Reaper_animator.SetTrigger("Dark_Soul");
        Slow_RotSpeed = 6.0f;

        yield return new WaitForSeconds(DarkSoul_Running_Time);
        // ���� ����
        isAttacking = false;
        Slow_RotSpeed = 0.0f;

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }

    public void DarkSoul_Eff()
    {
        StartCoroutine(Play_DarkSoul_Eff());
    }

    IEnumerator Play_DarkSoul_Eff()
    {
        Reaper_animator.SetFloat("DarkSoulSpeed", 0.2f);
        DarkSoul_Skill_Eff.SetActive(true);
        

        yield return new WaitForSeconds(DarkSoul_Running_Time - 4.0f);
        Reaper_animator.SetFloat("DarkSoulSpeed", 1.0f);

        
        yield return new WaitForSeconds(2.0f);
        DarkSoul_Skill_Eff.SetActive(false);
    }

    #endregion
}
