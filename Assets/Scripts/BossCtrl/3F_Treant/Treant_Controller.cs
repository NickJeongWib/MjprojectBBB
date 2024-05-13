using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InfinityPBR;
public enum TreantType
{
    NORMAL,
    SPEED,
    POWER,
}

public enum TreantState
{
    NONE = -1,
    IDLE,
    MOVE,
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
    BlendShapesPresetManager Treant_Type_Shape;     // ���� ���� ���� ��ũ��
    [SerializeField]
    TreantType Treant_Type;       // 3������ ����
    [SerializeField]
    TreantState Treant_State;     // 3�� ���� ���� ����
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
    [SerializeField]
    float Treant_Skill_Delay;


    Vector3 dir;                  // Treant ����

    #region Rotation
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
    #endregion

    #region Move
    public override void Move()
    {
        if (Target == null)
            return;

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
            Treant_State = TreantState.IDLE;
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
        // ���� �Ÿ� �ȿ� �ȵ�� ���� ���
        if (TargetDistance >= ChaseDistance)
        {
            Move();
        }
        else if (TargetDistance < ChaseDistance && !isAttacking) // �������� �ƴϰ� �����Ÿ��ȿ� ��� ���� ��
        {
            NotMove();
        }
    }
    #endregion

    #region Init
    void Treant_Init()
    {
        Treant_Type_Shape = GetComponent<BlendShapesPresetManager>();
        animator = GetComponent<Animator>();
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();

        // �ִ� ü�� ���� ���� ü�� 
        boss_hp_ctrl.BossMaxHP = MaxHP;
        // ���� �� �ʱ�ȭ
        Treant_Type = TreantType.NORMAL;
        Treant_State = TreantState.IDLE;
    }
    #endregion

    #region Treant_Form_Change
    public void Change_Normal_Form()
    {
        // TODO ## 3�� ���� �븻 �� ü����
        Treant_Type = TreantType.NORMAL;
        // ���� ��ȯ
        Treant_Type_Shape.StartTransitionToPreset("Reset");
        animator.SetTrigger("MagicAttack2");
    }

    public void Change_Speed_Form()
    {
        // TODO ## 3�� ���� ���ǵ� �� ü����
        Treant_Type = TreantType.SPEED;
        // ���� ��ȯ
        Treant_Type_Shape.StartTransitionToPreset("Skinny");
        animator.SetTrigger("MagicAttack2");
    }
    public void Change_Power_Form()
    {
        // TODO ## 3�� ���� �Ŀ� �� ü����
        Treant_Type = TreantType.POWER;
        // ���� ��ȯ
        Treant_Type_Shape.StartTransitionToPreset("Fat");
        animator.SetTrigger("MagicAttack2");
    }
    #endregion

    #region next_act

    public void Treant_NextAct()
    {
        // Ÿ���� ���ٸ� return;
        if (Target == null)
            return;

        StartCoroutine(Next_Act());
    }

    IEnumerator Next_Act()
    {
        // �⺻ ���¸� �ٷ� ���� ����
        if (Treant_State == TreantState.IDLE)
        {
            yield return new WaitForSeconds(0.0f);
        }
        else
        {
            // �⺻ ���°� �ƴ� �� 2�ʰ��� �����̸� �ش�
            yield return new WaitForSeconds(Treant_Skill_Delay);
        }

        // �������� ���� ���� ����
        TreantState randomState = (TreantState)Random.Range(0, (int)TreantState.MOVE + 1);
        Treant_State = randomState;

        switch (Treant_Type)
        {
            // �븻 ���� �� ��ų
            case TreantType.NORMAL:
                switch (Treant_State)
                {
                    case TreantState.IDLE:
                        Treant_Idle();
                        break;
                    case TreantState.MOVE:
                        Treant_Move();
                        break;
                    default:
                        break;
                }
                break;

            case TreantType.SPEED:
                switch (Treant_State)
                {
                    case TreantState.IDLE:
                        Treant_Idle();
                        break;
                    case TreantState.MOVE:
                        Treant_Move();
                        break;
                    default:
                        break;
                }
                break;

            case TreantType.POWER:
                switch (Treant_State)
                {
                    case TreantState.IDLE:
                        Treant_Idle();
                        break;
                    case TreantState.MOVE:
                        Treant_Move();
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }
    #endregion

    #region Treant_Idle
    public void Treant_Idle()
    {
        isMove = false;
        animator.SetFloat("Locomotion", 0.5f);
    }
    #endregion

    #region Treant_Move
    public void Treant_Move()
    {
        isMove = true;
    }
    #endregion
}
