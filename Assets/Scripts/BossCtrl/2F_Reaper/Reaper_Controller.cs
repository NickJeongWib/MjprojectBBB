using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReaperState
{
    RaidStart,      // 0
    Idle,           // 1
    Move,           // 2

    BaseAtk_0,      // 3
    BaseAtk_1,      // 4


    Dark_Hand,      // 5
    Dark_Decline,   // 6
}

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    [Header("-----Reaper State-----")]
    public ReaperState reaperState;
    public float Skill_Think_Range;
    public float TargetDistance;
    public float LockCloseDistance;

    public GameObject Target;   // �÷��̾�
    public Reaper_Atk_Range Reaper_AtkRange;
    Vector3 dir; // ����
    Rigidbody rigid;
    [SerializeField]
    float Boss_RotSpeed;
    [SerializeField]
    float moveSpeed;
    public bool isLock;               // ���� ���� ����
    public bool isAttacking;          // ���� �� ���� ����
    public bool CanAttack;

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // �ִϸ�����

    public bool isMove;         // �̵� ����
    public bool isTargetFind;   // ù ���� ����

    [Header("-----Skill Dark Decline-----")]
    public float Dark_Decline_Delay;

    #region Reaper_Rotate
    public override void LookAtPlayer()
    {
        // �÷��̾ ã�� �� ���ٸ� ���� ����
        if (Target == null || isLock)
            return;

        // �÷��̾ �ٶ󺸵���
        // this.transform.LookAt(Target.transform);

        dir = Target.transform.position - transform.position;
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
        Reaper_animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        StartCoroutine(SeePlayer());
    }

    IEnumerator SeePlayer()
    {
        Reaper_animator.SetTrigger("IsFindPlayer");

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length + 3.0f);

        // �Ÿ��� ���� ���Ÿ� ���� �ٰŸ� ����
        if (TargetDistance > Skill_Think_Range)
        {
            Reaper_Long_nextAct();
        }
        else if(TargetDistance <= Skill_Think_Range)
        {
            Reaper_Short_nextAct();
        }
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

        // �Ÿ��� ������ �Ÿ����� �ָ� ȸ��
        if (TargetDistance <= LockCloseDistance)
        {
            
        }
        else if (TargetDistance > LockCloseDistance)
        {
            LookAtPlayer();
        }  
    }


    #region Reaper_Next_Skill
    void Reaper_Short_nextAct()
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

    void Reaper_Long_nextAct()
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
            int randomIndex = Random.Range(0, 2);

            switch (randomIndex)
            {
                case 0:
                    // �̵�
                    isMove = true;
                    reaperState = ReaperState.Move;
                    break;

                case 1:
                    StartCoroutine(Dark_Hand());
                    break;
            }
        }
    }
    #endregion

    #region Reaper_Atk_Think
    // TODO ## 2�� ���� ���� ����
    public void Skill_Think()
    {
        if (isAttacking == true)
        {
            return;
        }

        if (TargetDistance <= Skill_Think_Range)
        {
            // ����
            int randomIndex = Random.Range(0, 3);

            switch (randomIndex)
            {
                case 0:
                    Reaper_animator.SetTrigger("BaseAtk_0");
                    break;
                case 1:
                    Reaper_animator.SetTrigger("BaseAtk_1");
                    break;
                case 2:
                    Reaper_animator.SetTrigger("Dark_Decline");
                    break;      
            }
        }
        else if (TargetDistance > Skill_Think_Range)
        {
            // ���Ÿ�
            int randomIndex = Random.Range(0, 2);

            switch (randomIndex)
            {
                case 0:
                    isMove = true;
                    break;

                case 1:
                    
                    Reaper_animator.SetTrigger("Dark_Hand");
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

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        // Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

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

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

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

        // Dark_Decline_Delay �� ����
        yield return new WaitForSeconds(Dark_Decline_Delay);
        // �ִϸ��̼� ����_2
        Reaper_animator.SetTrigger("Dark_Decline");

        // Dark_Decline_Delay �� ����
        yield return new WaitForSeconds(Dark_Decline_Delay);
        // �ִϸ��̼� ����_3
        Reaper_animator.SetTrigger("Dark_Decline");

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

        // �̵� ����
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);


        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

        // ���� ����
        isAttacking = false;

        // 2�� ��
        yield return new WaitForSeconds(2.0f);

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
