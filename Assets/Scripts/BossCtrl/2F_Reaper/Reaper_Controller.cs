using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public enum ReaperState
{
    RaidStart,      // 0
    Idle,           // 1
    Move,           // 2
    Teleport,       // 3
    
    BaseAtk_0,      // 4
    BaseAtk_1,      // 5

    Dark_Hand,      // 6
    Dark_Decline,   // 7
    Dark_Soul,      // 8
    Dark_Ball,      // 9

    Dark_Token,
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
    [SerializeField]
    int NextSkillNum;

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
    [SerializeField]
    GameObject BaseAtk_Collider;
    [SerializeField]
    GameObject BaseAtk_GuideLine;

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
    [SerializeField]
    GameObject Dark_Decline_Box_Collider; // �ڽ� �ݶ��̴�
    [SerializeField]
    GameObject Dark_Decline_Circle_Collider; // ��Ŭ �ݶ��̴�
    [SerializeField]
    GameObject Dark_Decline_GuideLine;

    [Header("-----Skill_Dark_Hand-----")]
    [SerializeField]
    GameObject CastingEff;
    [SerializeField]
    GameObject DarkHand_GuideLine;
    [SerializeField]
    GameObject DarkHand2_GuideLine;

    [Header("-----Skill_Dark_Soul-----")]
    [SerializeField]
    GameObject DarkSoul_Skill_Eff;
    [SerializeField]
    float Slow_RotSpeed;
    [SerializeField]
    float DarkSoul_Running_Time;
    [SerializeField]
    GameObject DarkSoul_Collider;
    [SerializeField]
    GameObject DarkSoul_GuideLine;

    [Header("-----Skill_Dark_Ball-----")]
    [SerializeField]
    Transform Center_Tr;                    // ���� ��ġ ��ų ���Ͱ�
    [SerializeField]
    Transform DarkBall_Pos;                 // ��ü ���� ��ų ��ġ
    [SerializeField]
    GameObject Pattern_Pillar_Normal;       // �븻 ���� ���
    [SerializeField]    
    GameObject Pattern_Pillar_Awakening;    // ���� �� ���� ���
    [SerializeField]
    float DarkBall_Delay;                   // �� ������ ������ �ð�
    [SerializeField]
    float Finish_DarkBall;                  // ������ �� ������ �ð�
    [SerializeField]
    GameObject[] DarkBall_Pilar;            // ����� ��ü �迭
    [SerializeField]
    GameObject[] DarkBall_Pilar_Awakening;  // ����� ��ü ���� �� ��� �迭
    [SerializeField]
    GameObject[] DarkBall_Awakening;        // ����� ��ü ������ �迭(�� �ִ� ��ü)
    [SerializeField]
    GameObject DarkBall_Soul_Eff;           // ���� �� ��ü�� ���� ������ ����� ��ȥ(����) 
    [SerializeField]
    int Awakening_Ball_Index;               // ���� �� ��ü�� �ε��� ��
    [SerializeField]
    float DarkBall_Razer_Time;              // ����� ��ü �� ��ȥ �ð�

    [Header("-----Skill_Dark_Token-----")]
    [SerializeField]
    bool[] DarkToken_END;
    [SerializeField]
    int Use_SpAtk_Count;
    [SerializeField]
    GameObject Flooring_Effect;
    [SerializeField]
    GameObject[] Token_obj;
    [SerializeField]
    GameObject[] Token_GuideLine;
    [SerializeField]
    float Token_Delay;


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
    public void Reaper_Short_nextAct(int _skillnum)
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
                // ����ߴ� ��ų ���� ��� ����
                while(true)
                {
                    // ���� ���� ����
                    int randomIndex = Random.Range(0, 3);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }
                

                switch (NextSkillNum)
                {
                    case 0: // �⺻ ����
                        StartCoroutine(BaseAtk_0());
                        break;
                    case 1: // �⺻����
                        StartCoroutine(BaseAtk_1());
                        break;
                    case 2: // ����� ���
                        StartCoroutine(Dark_Decline());
                        break;

                }
            }
            else if (reaperAwakeState == Reaper_Awake.AWAKENING)
            {

                while (true)
                {
                    // ���� ���� ����
                    int randomIndex = Random.Range(0, 4);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }

                switch (NextSkillNum)
                {
                    case 0: // �⺻ ����
                        StartCoroutine(BaseAtk_0());
                        break;
                    case 1: // �⺻ ����
                        StartCoroutine(BaseAtk_1());
                        break;
                    case 2: // ����� ���
                        StartCoroutine(Dark_Decline());
                        break;
                    case 3: // ����� ��ȥ
                        StartCoroutine(Dark_Soul());
                        break;
                }
            }       
        }
    }

    public void Reaper_Long_nextAct(int _skillnum)
    {
        // ���� �������̶�� return
        if (isAttacking == true)
        {
            return;
        }

        // �Ÿ��� �ָ�
        if (TargetDistance > Skill_Think_Range)
        {

            if (reaperAwakeState == Reaper_Awake.NORMAL)
            {
                // ����ߴ� ��ų ���� ��� ����
                while (true)
                {
                    // ���� ���� ����
                    int randomIndex = Random.Range(0, 4);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }

                switch (NextSkillNum)
                {
                    case 0:
                        StartCoroutine(BossMove());
                        break;
                    case 1:
                         StartCoroutine(Teleport());
                        break;
                    case 2:
                        StartCoroutine(Dark_Hand());
                        break;
                    case 3: // ����� ��ü
                        StartCoroutine(Dark_Ball());
                        break;
                }
            }
            else if (reaperAwakeState == Reaper_Awake.AWAKENING)
            {
                // ����ߴ� ��ų ���� ��� ����
                while (true)
                {
                    // ���� ���� ����
                    int randomIndex = Random.Range(0, 5);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }

                switch (NextSkillNum)
                {
                    case 0:
                        StartCoroutine(BossMove());
                        break;
                    case 1:
                        StartCoroutine(Teleport());
                        break;
                    case 2:
                        StartCoroutine(Dark_Hand());
                        break;
                    case 3:
                        StartCoroutine(Dark_Hand2());
                        break;
                    case 4: // ����� ��ü
                        StartCoroutine(Dark_Ball());
                        break;
                }
            }
        }
    }

    public void Reaper_Special_nextAct()
    {
        // ���� �������̶�� return
        if (isAttacking == true)
        {
            return;
        }

        // ����� ��ǥ ����
        StartCoroutine(Dark_Token());

        // Use_SpAtk_Count Ƚ�� ° ��� true 
        DarkToken_END[Use_SpAtk_Count] = true;
        // ��� Ƚ�� ī��Ʈ
        Use_SpAtk_Count++;
        // Debug.Log(Use_SpAtk_Count);
    }
    #endregion

    #region Reaper_PlayerCheck

    public void PlayerCheck()
    {
        if (TargetDistance < Skill_Think_Range)
        {
            isMove = false;
            StopAllCoroutines();
            Reaper_Short_nextAct(Random.Range(0, 3));
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

    #region ReaperDeath
    public void Death()
    {
        StopAllCoroutines();
        isLock = true;
    }
    #endregion

    #region Boss_Reaper_Teleport
    IEnumerator Teleport()
    {
        reaperState = ReaperState.Teleport;
        Reaper_animator.SetTrigger("Teleport");
        // �̵� ����
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);
        // �÷��̾� �������� ������ ������ �Ÿ��� �ڷ���Ʈ
        float randomAngle = Random.Range(0f, 360f);

        // �÷��̾��� ��ġ���� ���Ϸ� �� ��ŭ ��ġ���� * (Skill_Think_Range - 3.0f)��ŭ �Ÿ��� ��ġ
        Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        Vector3 randomPosition = Target.transform.position + randomDirection * (Skill_Think_Range - 7.0f);

        // Y ��ǥ�� 0���� ����
        randomPosition.y = 1.5f;

        // �ڷ���Ʈ
        transform.position = randomPosition;

        yield return new WaitForSeconds(0.5f);

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(1);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
        }
    }

    #endregion

    #region Boss_Move
    IEnumerator BossMove()
    {
        yield return new WaitForSeconds(1.4f);
        // �̵�
        isMove = true;
        reaperState = ReaperState.Move;
    }
    #endregion

    #region Boss_Awake
    void AwakeBoss()
    {
        // ���� ����
        reaperAwakeState = Reaper_Awake.AWAKENING;
        Reaper_animator.SetTrigger("Awakening");
        nextActTime = 1.0f;
        // ���� ��� Ȱ��ȭ
        Pattern_Pillar_Awakening.SetActive(true);
        // �Ϲ� ��� ��Ȱ��ȭ
        Pattern_Pillar_Normal.SetActive(false);
    }
    #endregion

    #region Boss_Atk_0
    // TODO ## Reaper_BaseAtk_0
    IEnumerator BaseAtk_0()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        BaseAtk_GuideLine.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_GuideLine.SetActive(false);

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


        // 4�� �� ���� ����
        yield return new WaitForSeconds(nextActTime);
        isAttacking = false;

        // ����
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            AwakeBoss();
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }


        // ���� ���� ���� ���� üũ
        if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(0);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(0);
        }
    }
    // �⺻ ���� ������ ����Ʈ ����
    public void BaseAtk0_Eff()
    {
        StartCoroutine(Play_BaseAtk0_Eff());
    }

    IEnumerator Play_BaseAtk0_Eff()
    {
        // ����Ʈ �ǰݹ��� ����
        BaseAtk_0_Eff.SetActive(true);
        BaseAtk_Collider.SetActive(true);

        // �ǰݹ��� ����
        yield return new WaitForSeconds(0.1f);
        BaseAtk_Collider.SetActive(false);

        // ȸ�� ����
        yield return new WaitForSeconds(1.9f);
        isLock = false;
        BaseAtk_0_Eff.SetActive(false);
    }

    #endregion

    #region Boss_Atk_1
    // TODO ## Reaper_BaseAtk_1
    IEnumerator BaseAtk_1()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        BaseAtk_GuideLine.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_GuideLine.SetActive(false);

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

        // 4�� �� ����
        yield return new WaitForSeconds(nextActTime);
        // ���� ����
        isAttacking = false;
        // ����
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            AwakeBoss();
            // �����ൿ �ð� + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }

        // ���� ���� ���� ���� üũ
        if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }


        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(1);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(1);
        }
    }

    // �⺻ ���� ������ ����Ʈ ����
    public void BaseAtk1_Eff()
    {
        StartCoroutine(Play_BaseAtk1_Eff());
    }

    IEnumerator Play_BaseAtk1_Eff()
    {
        // ����Ʈ �ǰݹ��� ����
        BaseAtk_1_Eff.SetActive(true);
        BaseAtk_Collider.SetActive(true);

        // �ǰݹ��� ����
        yield return new WaitForSeconds(0.1f);
        BaseAtk_Collider.SetActive(false);

        // ȸ�� ����
        yield return new WaitForSeconds(1.9f);
        isLock = false;
        BaseAtk_1_Eff.SetActive(false);
       
    }
    #endregion

    #region Boss_Atk_2_DarkDecline
    // TODO ## Reaper_DarkDecline
    IEnumerator Dark_Decline()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // ���� �� ����� ���
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // ���� ���� ����
            reaperState = ReaperState.Dark_Decline;

           
            // yield return new WaitForSeconds(0.2f);

            // ���� ����
            isAttacking = true;
            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);

            // �ִϸ��̼� ����_1
            Reaper_animator.SetTrigger("Dark_Decline");
            // ���̵���� Ȱ��ȭ
            Dark_Decline_GuideLine.SetActive(true);

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

            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // �ִϸ��̼� ����_2
            Reaper_animator.SetTrigger("Dark_Decline");
            // ���̵���� Ȱ��ȭ
            Dark_Decline_GuideLine.SetActive(true);

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

            // ���̵���� Ȱ��ȭ
            //Dark_Decline_GuideLine.SetActive(true);
            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // �ִϸ��̼� ����_3
            Reaper_animator.SetTrigger("Dark_Decline");
            // ���̵���� Ȱ��ȭ
            Dark_Decline_GuideLine.SetActive(true);

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
                AwakeBoss();
                // �����ൿ �ð� + 1
                yield return new WaitForSeconds(nextActTime + 1.0f);
            }

            // ���� ���� ���� ���� üũ
            if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }


            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(2);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(2);
            }
        }
        else if (reaperAwakeState == Reaper_Awake.AWAKENING) // ����� ��� ���� ��---------------------------------------
        {
            // ���� ���� ����
            reaperState = ReaperState.Dark_Decline;

            // ���̵���� Ȱ��ȭ
            // Dark_Decline_GuideLine.SetActive(true);
            // yield return new WaitForSeconds(0.2f);

            // ���� ����
            isAttacking = true;
            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            // �ִϸ��̼� ����_1
            Reaper_animator.SetTrigger("Dark_Decline");
            // ���̵���� Ȱ��ȭ
            Dark_Decline_GuideLine.SetActive(true);

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

            // ���̵���� Ȱ��ȭ
            //Dark_Decline_GuideLine.SetActive(true);
            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
            // �ִϸ��̼� ����_2
            Reaper_animator.SetTrigger("Dark_Decline");

            // ���̵���� Ȱ��ȭ
            Dark_Decline_GuideLine.SetActive(true);

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

            // ���̵���� Ȱ��ȭ
            //Dark_Decline_GuideLine.SetActive(true);
            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay �� ����
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // �ִϸ��̼� ����_3
            Reaper_animator.SetTrigger("Dark_Decline");

            // ���̵���� Ȱ��ȭ
            Dark_Decline_GuideLine.SetActive(true);

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

            if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }

            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(10);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(2);
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
        // ���̵���� ��Ȱ��ȭ
        Dark_Decline_GuideLine.SetActive(false);

        // ����� ��� ��ü ���� Ȱ��ȭ
        yield return new WaitForSeconds(0.2f);
        Dark_Decline_Circle_Collider.SetActive(true);

        // ����� ��� ��ü ���� ��Ȱ��ȭ, �ڽ����� Ȱ��ȭ
        yield return new WaitForSeconds(1.4f);
        Dark_Decline_Circle_Collider.SetActive(false);
        Dark_Decline_Box_Collider.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        Dark_Decline_Box_Collider.SetActive(false);
        // ����Ʈ ����
        yield return new WaitForSeconds(0.1f);

        Dark_Decline_Slash.SetActive(false);
    }
    #endregion

    #region Reaper_Atk_3_DarkHand
    // TODO ## Reaper_DarkHand / Reaper_DarkHand2
    IEnumerator Dark_Hand()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

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
        yield return new WaitForSeconds(3.0f);

        //  �ִϸ��̼��� ������ �� �� 2�� ��
        yield return new WaitForSeconds(2.0f);
        // ���� ����
        isAttacking = false;
        isLock = false;

        // ����
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            AwakeBoss();
            // �����ൿ �ð� + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }

        // ���� ���� ���� ���� üũ
        if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(2);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
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

        //���̵���� Ȱ��ȭ, ��ġ ����
        DarkHand_GuideLine.SetActive(true);
        DarkHand_GuideLine.transform.position = Pos;

        yield return new WaitForSeconds(1.5f);

        // ���̵���� ��Ȱ��ȭ
        DarkHand_GuideLine.SetActive(false);

        // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
        GameObject DarkHnad_Explosion = reaper_ObjPoolRef.GetDarkHandFromPool();

        // ���� �ݶ��̴��� ���� �ִٸ�
        if (DarkHnad_Explosion.GetComponent<CapsuleCollider>().enabled == false)
        {
            DarkHnad_Explosion.GetComponent<CapsuleCollider>().enabled = true;
        }

        DarkHnad_Explosion.transform.position = Pos;


        yield return new WaitForSeconds(0.5f);
        DarkHnad_Explosion.GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(3.0f);
        DarkHnad_Explosion.SetActive(false);
    }
    #endregion

    #region Reaper_Atk_4_DarkHand2
    IEnumerator Dark_Hand2()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

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

        if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(3);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
        }
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

        // ���̵���� Ȱ��ȭ �� ��ġ ����
        DarkHand2_GuideLine.SetActive(true);
        DarkHand2_GuideLine.transform.position = Pos;

        yield return new WaitForSeconds(0.5f);

        DarkHand2_GuideLine.SetActive(false);

        // ������Ʈ Ǯ���� ����Ʈ ��ġ �������� ����
        GameObject DarkHnad2_Explosion = reaper_ObjPoolRef.GetDarkHand2FromPool();
        DarkHnad2_Explosion.transform.GetChild(2).gameObject.SetActive(true);
        DarkHnad2_Explosion.transform.GetChild(1).GetChild(8).GetComponent<CapsuleCollider>().enabled = true;


        DarkHnad2_Explosion.transform.position = Pos;

        yield return new WaitForSeconds(1.0f);
        DarkHnad2_Explosion.transform.GetChild(1).GetChild(8).gameObject.SetActive(false);

        yield return new WaitForSeconds(8.0f);
        // �� ��ġ
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(0).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(1).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.SetActive(false);
    }

    #endregion

    #region Reaper_Atk_5_DarkSoul
    // TODO ## Reaper_DarkSoul
    IEnumerator Dark_Soul()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // ���� ���� ����
        reaperState = ReaperState.Dark_Soul;
        // ���̵���� Ȱ��ȭ
        DarkSoul_GuideLine.SetActive(true);

        // ���̵���� ��Ȱ���� ����
        // yield return new WaitForSeconds(1.0f);

        // ���� ����
        isAttacking = true;
        // �̵� ����
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);
        // �ִϸ��̼� ����_1
        Reaper_animator.SetTrigger("Dark_Soul");
        Slow_RotSpeed = 6.0f;

        yield return new WaitForSeconds(1.5f);
        // ���̵���� ��Ȱ��ȭ
        DarkSoul_GuideLine.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        DarkSoul_Collider.SetActive(true);

        yield return new WaitForSeconds(DarkSoul_Running_Time - 4.0f);
        DarkSoul_Collider.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        // ���� ����
        isAttacking = false;
        Slow_RotSpeed = 0.0f;

        if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(10);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(3);
        }
    }

    public void DarkSoul_Eff()
    {
        StartCoroutine(Play_DarkSoul_Eff());
    }

    IEnumerator Play_DarkSoul_Eff()
    {
        if (reaperState == ReaperState.Dark_Soul) // ���� �� 
        {
            Reaper_animator.SetFloat("DarkSoulSpeed", 0.2f);
            DarkSoul_Skill_Eff.SetActive(true);


            yield return new WaitForSeconds(DarkSoul_Running_Time - 4.0f);
            Reaper_animator.SetFloat("DarkSoulSpeed", 1.0f);


            yield return new WaitForSeconds(2.0f);
            DarkSoul_Skill_Eff.SetActive(false);
        }
        else if (reaperState == ReaperState.Dark_Ball)// ���� ��
        {
       

            Reaper_animator.SetFloat("DarkSoulSpeed", 0.1f);
            DarkBall_Soul_Eff.SetActive(true);


            yield return new WaitForSeconds(DarkBall_Razer_Time);
            Reaper_animator.SetFloat("DarkSoulSpeed", 1.0f);
            Debug.Log(1);
            // ��ȥ �ݶ��̴� ����Ʈ ����
            DarkSoul_Collider.SetActive(false);

            yield return new WaitForSeconds(2.0f);
            DarkBall_Soul_Eff.SetActive(false);
        }
    }

    #endregion

    #region Atk_6_Dark_Ball
    // TODO ## Reaper_DarkBall
    IEnumerator Dark_Ball()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // ���� �� ����� ��ü
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                DarkBall_Pilar[i].GetComponent<BoxCollider>().enabled = true;
                DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(83, 34, 191) * 0.01f);
                DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter = false;
            }

            // ���� ����
            reaperState = ReaperState.Dark_Ball;
            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Teleport");
            // �߾����� �̵�
            this.transform.position = Center_Tr.position;
            // ���� ��
            isAttacking = true;
            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);

            yield return new WaitForSeconds(DarkBall_Delay);

            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(DarkBall_Delay);

            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(DarkBall_Delay);

            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(DarkBall_Delay);

            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(Finish_DarkBall);

            // ��� ����
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // ���� ���� �ʾҴٸ�
                if (DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter == false)
                {
                    // �������� ��� ����
                    DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().Pilar_Explosion.SetActive(true);
                }
            }

            yield return new WaitForSeconds(2.0f);

            // ��� ����
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // ���� ��Ȱ��ȭ
                DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().Pilar_Explosion.SetActive(false);
            }

            // ���� ��
            isAttacking = false;


            // ���� ���� ���� ���� üũ
            if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }


            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(3);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(10);
            }
        }
        // ���� �� ����� ��ü
        else if (reaperAwakeState == Reaper_Awake.AWAKENING)
        {
            for (int i = 0; i < DarkBall_Pilar_Awakening.Length; i++)
            {
                DarkBall_Pilar_Awakening[i].GetComponent<BoxCollider>().enabled = true;
                
                // �� �ʱ�ȭ
                if (i == 0) // 1�� �Ķ�
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                     new Color(0, 9, 191) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.BLUE;
                }
                else if (i == 1) // 5�� ���
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(191, 157, 34) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.YELLOW;
                }
                else if (i == 2) // 7�� �ʷ�
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(3, 191, 0) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.GREEN;
                }
                else // i = 3 11�� ����
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(191, 0, 1) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.RED;
                }

                DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter = false;
            }
            // ���� ����
            reaperState = ReaperState.Dark_Ball;
            // �ִϸ��̼� �۵�
            Reaper_animator.SetTrigger("Teleport");
            // �߾����� �̵�
            this.transform.position = Center_Tr.position;

            isAttacking = true;
            // �̵� ����
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            Slow_RotSpeed = 6.5f;

          

            yield return new WaitForSeconds(DarkBall_Delay - 1.0f);
            // �ִϸ��̼� ����_1
            Reaper_animator.SetTrigger("Dark_Soul");

            yield return new WaitForSeconds(1.0f);
            // ���̵���� Ȱ��ȭ
            DarkSoul_GuideLine.SetActive(true);

            yield return new WaitForSeconds(DarkBall_Delay - 2.0f);
            DarkSoul_GuideLine.SetActive(false);
            DarkSoul_Collider.SetActive(true);

            // ���� ��ü ����
            yield return new WaitForSeconds(2.0f);
            // ����� ��ü Ȱ��ȭ
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // ����� ��ü ���� ��ġ �ʱ�ȭ
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // �ε��� ����
            Awakening_Ball_Index++;

            // �Ķ� ��ü ����
            yield return new WaitForSeconds(DarkBall_Delay);
            // ����� ��ü Ȱ��ȭ
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // ����� ��ü ���� ��ġ �ʱ�ȭ
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // �ε��� ����
            Awakening_Ball_Index++;

            // ��� ��ü ����
            yield return new WaitForSeconds(DarkBall_Delay);
            // ����� ��ü Ȱ��ȭ
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // ����� ��ü ���� ��ġ �ʱ�ȭ
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // �ε��� ����
            Awakening_Ball_Index++;

            // �ʷ� ��ü ����
            yield return new WaitForSeconds(DarkBall_Delay);
            // ����� ��ü Ȱ��ȭ
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // ����� ��ü ���� ��ġ �ʱ�ȭ
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // �ε��� �ʱ�ȭ
            Awakening_Ball_Index = 0;

            yield return new WaitForSeconds((DarkBall_Razer_Time - (4 * DarkBall_Delay)));

            // ��� ����
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // ���� ���� �ʾҴٸ�
                if (DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter == false)
                {
                    // �������� ��� ����
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Awakening_Pilar_Explosion.SetActive(true);
                }
            }

            yield return new WaitForSeconds(2.0f);

            // ��� ����
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // ���� ��Ȱ��ȭ
                DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Awakening_Pilar_Explosion.SetActive(false);
            }

            // ���� ��
            isAttacking = false;
            Slow_RotSpeed = 0.0f;

            if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }

            // �Ÿ��� ���� ���� ����
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(4);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(10);
            }
        }
    }

    public void DarkBall_Eff()
    {
        StartCoroutine(Play_DarkBall_Eff());
    }

    IEnumerator Play_DarkBall_Eff()
    {
        yield return new WaitForSeconds(0);

        // ���� �� 
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // ����� ��ü ��������
            GameObject DarkBall = reaper_ObjPoolRef.GetDarkBallFromPool();
            DarkBall.transform.position = DarkBall_Pos.position;
        }
        else // ���� ��
        {
            // ���� ���� �� ��ü �����ϱ� ������ ��� 
        }
    }
    #endregion

    #region Dark_Token
    // TODO ## Reaper_Token
    IEnumerator Dark_Token()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // ���� ����
            reaperState = ReaperState.Dark_Token;
        // �ִϸ��̼� �۵�
        Reaper_animator.SetTrigger("Teleport");
        // �߾����� �̵�
        this.transform.position = Center_Tr.position;
        // ���� ��
        isAttacking = true;
        this.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        //ȸ�� ����
        isLock = true;
        // �̵� ����
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);

        // ù��°
        yield return new WaitForSeconds(Token_Delay);
        // ���� Ȱ��ȭ
        Flooring_Effect.SetActive(true);

        // ù��°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[0].SetActive(true);
        Token_GuideLine[0].SetActive(true);

        // �ι�°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[1].SetActive(true);
        Token_GuideLine[0].SetActive(false);
        Token_GuideLine[1].SetActive(true);

        // ����°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[2].SetActive(true);
        Token_GuideLine[1].SetActive(false);
        Token_GuideLine[2].SetActive(true);

        // �׹�°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[3].SetActive(true);
        Token_GuideLine[2].SetActive(false);
        Token_GuideLine[3].SetActive(true);

        // �ټ���°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[4].SetActive(true);
        Token_GuideLine[3].SetActive(false);
        Token_GuideLine[4].SetActive(true);

        // ������°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[5].SetActive(true);
        Token_GuideLine[4].SetActive(false);
        Token_GuideLine[5].SetActive(true);

        // �ϰ���°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[6].SetActive(true);
        Token_GuideLine[5].SetActive(false);
        Token_GuideLine[6].SetActive(true);


        // ������°
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[7].SetActive(true);
        Token_GuideLine[6].SetActive(false);
        Token_GuideLine[7].SetActive(true);


        yield return new WaitForSeconds(Token_Delay);
        Token_GuideLine[7].SetActive(false);

        yield return new WaitForSeconds(2);

        // ���� ����Ʈ ��Ȱ��ȭ
        Flooring_Effect.SetActive(false);

        // ���� ��
        isAttacking = false;
        //ȸ�� ����
        isLock = false;

        // �Ÿ��� ���� ���� ����
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(10);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
        }
    }
    #endregion
}
