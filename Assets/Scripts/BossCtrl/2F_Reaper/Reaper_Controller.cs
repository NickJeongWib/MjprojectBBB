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
}

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    [Header("-----Reaper State-----")]
    public ReaperState reaperState;
    public bool isLock;               // ���� ���� ����
    public bool isAttacking;          // ���� �� ���� ����

    [Header("-----Reaper Reference-----")]
    public Reaper_Atk_Range Reaper_AtkRange; //  
    public Boss_HP_Controller boss_hp_ctrl;  // HP ��Ʈ�ѷ�

    [Header("-----Reaper Variable-----")]
    public GameObject Target;       // �÷��̾�
    public float TargetDistance;    // �÷��̾���� �Ÿ�

    [Header("-----Reaper State Variable-----")]
    public int MaxHP;   // ���� ü��

    [SerializeField]
    float Boss_RotSpeed;    //  ȸ�� �ӵ�
    [SerializeField]
    float moveSpeed;        // ������ �ӵ�
    public float Skill_Think_Range; // ��ų ���� ���� ����


    Vector3 dir; // ����
    // Rigidbody rigid;

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // �ִϸ�����
    public bool isMove;         // �̵� ����

    [Header("-----Skill_ BaseAtk_0-----")]
    [SerializeField]
    float BaseAtk_0_LockTime; // �⺻ ����_0 ȸ�� ����

    [Header("-----Skill_ BaseAtk_1-----")]
    [SerializeField]
    float BaseAtk_1_LockTime; // // �⺻ ����_1 ȸ�� ����

    [Header("-----Skill_Dark_Decline-----")]
    public float Dark_Decline_Delay; // ����� ��� ��ų ��� ������
    [SerializeField]
    float Decline_LockTime; // ȸ�� ���� �ð�
    [SerializeField]
    float Decline_UnLockTime; // ȸ�� ���� ���� �ð�

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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Boss_RotSpeed);
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

    // Start is called before the first frame update
    void Start()
    {
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        Reaper_animator = GetComponent<Animator>();
        // rigid = GetComponent<Rigidbody>();

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

    #region Reaper_Idle
    public void Reaper_Idle()
    {
        reaperState = ReaperState.Idle;
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
        yield return new WaitForSeconds(4.0f);
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
        yield return new WaitForSeconds(4.0f);

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
    #endregion

    #region Boss_Atk_2_DarkDecline
    // TODO ## Reaper_DarkDecline
    IEnumerator Dark_Decline()
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

        yield return new WaitForSeconds(Decline_UnLockTime);
        isLock = false;

        // Dark_Decline_Delay �� ����
        yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
        // �ִϸ��̼� ����_2
        Reaper_animator.SetTrigger("Dark_Decline");

        yield return new WaitForSeconds(Decline_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Decline_UnLockTime);
        isLock = false;

        // Dark_Decline_Delay �� ����
        yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

        // �ִϸ��̼� ����_3
        Reaper_animator.SetTrigger("Dark_Decline");

        yield return new WaitForSeconds(Decline_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Decline_UnLockTime);
        isLock = false;

        // Dark_Decline_Delay �� ����
        yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

        // 4�� �� ����
        yield return new WaitForSeconds(4.0f);

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
    #endregion

    #region Reaper_Atk_3_DarkHand
    // TODO ## Reaper_DarkHand
    IEnumerator Dark_Hand()
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
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

        // 2�� ��
        yield return new WaitForSeconds(2.0f);
        // ���� ����
        isAttacking = false;
        isLock = false;

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
}
