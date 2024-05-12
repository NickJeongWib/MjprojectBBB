using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TreantType
{
    NORMAL,
    SPEED,
    POWER,
}

public enum TreantState
{
    Move,
}

public class Treant_Controller : Boss_BehaviorCtrl_Base
{
    [Header("----Treant_Animation_Variable---")]
    [SerializeField]
    Animator animator;
    [SerializeField]
    float startVal;
    [SerializeField]
    float endVal;
    [SerializeField]
    float lerpTime;

    [Header("----Treant_HP_Variable---")]
    [SerializeField]
    Boss_HP_Controller boss_hp_ctrl;        // HP ��Ʈ�ѷ�
    [SerializeField]
    int MaxHP;                              // �ִ� ü��


    [Header("----Treant_Target_Variable---")]
    public GameObject Target;
    [SerializeField]
    float TargetDistance;
    [SerializeField]
    float ChaseDistance;

    [Header("----Treant_State_Variable---")]
    [SerializeField]
    bool isMove;                  // ������ üũ ����
    [SerializeField]
    bool isAttacking;             // ���� üũ ����
    [SerializeField]
    bool isLock;                  // ȸ�� ���ɿ��� üũ ����
    [SerializeField]
    float Treant_MoveSpeed;       // �̵��ӵ�
    [SerializeField]
    float Treant_RotSpeed;

    Vector3 dir;                  // Treant ����

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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Treant_RotSpeed);
    }

    #region Move
    public override void Move()
    {
        if (Target == null)
            return;

        isMove = true;

        if (isMove && isAttacking == false)
        {
            transform.Translate(Vector3.forward * Treant_MoveSpeed * Time.deltaTime);
            animator.SetFloat("Locomotion", 1.0f);
        }
    }

    public void NotMove()
    {
        if (Target == null)
            return;

        isMove = false;

        if (!isMove && isAttacking == false)
        {
            transform.Translate(Vector3.forward * 0.0f * Time.deltaTime);
            animator.SetFloat("Locomotion", 0.5f);
        }
    }
    #endregion


    #region Awake / Start / Update
    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Treant_Init();
    }

    void Update()
    {
        // �÷��̾ null�� �ƴ϶��
        if (Target != null)
        {
            TargetDistance = Vector3.Distance(Target.transform.position, this.transform.position);
        }

        LookAtPlayer(); // �÷��̾� ���� ��ȯ
    }

    void FixedUpdate()
    {
        if (TargetDistance > ChaseDistance)
        {
            Move();
        }
        else
        {
            NotMove();
        }
    }
    #endregion


    #region Init
    void Treant_Init()
    {
        animator = GetComponent<Animator>();
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        // �ִ� ü�� ���� ���� ü�� 
        boss_hp_ctrl.BossMaxHP = MaxHP;
    }
    #endregion
}
