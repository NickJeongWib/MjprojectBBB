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

public enum Treant_Normal_State
{
    NONE = -1, 
  
    IDLE,
    MOVE,
    LEAFMISSALE,
    LEAFBREATH,
    NORMALATTACK,
    BARRIER,
    LEAFTURN,
    LEAFPLACE,

    END,
}

public enum Treant_Speed_State
{
    NONE = -1,
   
    IDLE,
    MOVE,
    NORMALATTACK,
    BARRIER,
    END,
}

public enum Treant_Power_State
{
    NONE = -1,
   
    IDLE,
    MOVE,
    NORMALATTACK,
    BARRIER,
    END,
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
    [SerializeField]
    GameObject TreantAtkRange;
    public bool isStartRaid;

    [Header("----Treant_State_Variable---")]
    [SerializeField]
    BlendShapesPresetManager Treant_Type_Shape;     // ���� ���� ���� ��ũ��
    [SerializeField]
    TreantType Treant_Type;       // 3������ ����
    [SerializeField]
    Treant_Normal_State TreantNormalState;     // 3�� ���� �븻�� ���� ����
    [SerializeField]
    Treant_Speed_State TreantSpeedState;
    [SerializeField]
    Treant_Power_State TreantPowerState;
    [SerializeField]
    bool isMove;                        // ������ üũ ����
    [SerializeField]
    public bool isAttacking;            // ���� üũ ����
    [SerializeField]
    bool isLock;                        // ȸ�� ���ɿ��� üũ ����
    [SerializeField]
    float Treant_MoveSpeed;             // �̵��ӵ�
    [SerializeField]
    float Treant_RotSpeed;
    [SerializeField]
    float Treant_Slow_RotSpeed;
    [SerializeField]
    float Treant_Skill_Delay;
    [SerializeField]
    bool isThink;

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
        // ȸ�� ��� ��ų�� ���� ȸ������ �������� ����
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (Treant_RotSpeed - Treant_Slow_RotSpeed));
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
            //animator.SetFloat("Locomotion", 1.0f);
            animator.SetBool("isMove", true);
        }
    }

    public void NotMove()
    {
        if (Target == null)
            return;

        isMove = false;

        if (!isMove && isAttacking == false)
        {
            // �븻 ���·� ���� �� 
            if (Treant_Type == TreantType.NORMAL)
            {
                TreantNormalState = Treant_Normal_State.IDLE;
                TreantPowerState = Treant_Power_State.NONE;
                TreantSpeedState = Treant_Speed_State.NONE;
            }
            else if (Treant_Type == TreantType.POWER) // �Ŀ������� ���� ��
            {
                TreantNormalState = Treant_Normal_State.NONE;
                TreantPowerState = Treant_Power_State.IDLE;
                TreantSpeedState = Treant_Speed_State.NONE;
            }
            else // ���ǵ� ������ ���� ��
            {
                TreantNormalState = Treant_Normal_State.NONE;
                TreantPowerState = Treant_Power_State.NONE;
                TreantSpeedState = Treant_Speed_State.IDLE;
            }

            transform.Translate(Vector3.forward * 0.0f * Time.deltaTime);
            //animator.SetFloat("Locomotion", 0.5f);
            animator.SetBool("isMove", false);
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

        Target = GameObject.FindGameObjectWithTag("Player");
        // �ִ� ü�� ���� ���� ü�� 
        boss_hp_ctrl.BossMaxHP = MaxHP;
        // ���� �� �ʱ�ȭ
        Treant_Type = TreantType.NORMAL;
        TreantNormalState = Treant_Normal_State.IDLE;
        TreantPowerState = Treant_Power_State.NONE;
        TreantSpeedState = Treant_Speed_State.NONE;
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
        // Ÿ���� ���ų� �������̸� return;
        if (Target == null || isAttacking || isThink)
            return;

        //StartCoroutine(Next_Act());

        Next_Act();
    }

    // IEnumerator Next_Act()
    public void Next_Act()
    {
        isThink = true;

        // ���� �ൿ ����
        if (Treant_Type == TreantType.NORMAL)
        {
            // �������� ���� ���� ����
            Treant_Normal_State randomNormalState = (Treant_Normal_State)Random.Range(2, (int)Treant_Normal_State.END);
            TreantNormalState = randomNormalState;

            Debug.Log(randomNormalState);
        }
        else if (Treant_Type == TreantType.POWER)
        {
            // �������� ���� ���� ����
            Treant_Power_State randomPowerState = (Treant_Power_State)Random.Range(2, (int)Treant_Power_State.END);
            TreantPowerState = randomPowerState;

            Debug.Log(randomPowerState);

        }
        else if (Treant_Type == TreantType.SPEED)
        {
            // �������� ���� ���� ����
            Treant_Speed_State randomSpeedState = (Treant_Speed_State)Random.Range(2, (int)Treant_Speed_State.END);
            TreantSpeedState = randomSpeedState;

            Debug.Log(randomSpeedState);
        }

        // �⺻ ���°� �ƴ� �� 2�ʰ��� �����̸� �ش�
        //yield return new WaitForSeconds(0.0f);

        // ����� ���� �Ÿ����� �ָ� �̵� ����
        if (TargetDistance >= ChaseDistance)
        {
            if (Treant_Type == TreantType.NORMAL)
            {
                TreantNormalState = Treant_Normal_State.MOVE;
            }
            else if (Treant_Type == TreantType.POWER)
            {
                TreantPowerState = Treant_Power_State.MOVE;
            }
            else
            {
                TreantSpeedState = Treant_Speed_State.MOVE;
            }
        }

        switch (Treant_Type)
        {
            // �븻 ���� �� ��ų
            case TreantType.NORMAL:
                switch (TreantNormalState)
                {
                    //case Treant_Normal_State.IDLE:
                    //    Treant_Idle();
                    //    break;
                    case Treant_Normal_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Normal_State.NORMALATTACK:
                        Normal_Attack();
                        break;
                    case Treant_Normal_State.BARRIER:
                        Treant_Barrier();
                        break;
                    case Treant_Normal_State.LEAFTURN:
                        Treant_LeafTurn();
                        break;
                    case Treant_Normal_State.LEAFBREATH:
                        Treant_LeafBreath();
                        break;
                    case Treant_Normal_State.LEAFPLACE:
                        Treant_LeafPlace();
                        break;
                    case Treant_Normal_State.LEAFMISSALE:
                        Treant_LeafMissale();
                        break;
                    default:
                        break;
                }
                break;
                // ���ǵ� ���϶�
            case TreantType.SPEED:
                switch (TreantSpeedState)
                {
                    case Treant_Speed_State.IDLE:
                        Treant_Idle();
                        break;
                    case Treant_Speed_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Speed_State.NORMALATTACK:
                        Normal_Attack();
                        break;
                    case Treant_Speed_State.BARRIER:
                        Treant_Barrier();
                        break;
                    default:
                        break;
                }
                break;
                // �Ŀ� ���϶�
            case TreantType.POWER:
                switch (TreantPowerState)
                {
                    case Treant_Power_State.IDLE:
                        Treant_Idle();
                        break;
                    case Treant_Power_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Power_State.NORMALATTACK:
                        Normal_Attack();
                        break;
                    case Treant_Power_State.BARRIER:
                        Treant_Barrier();
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
        //�ٽ� Ȱ��ȭ
        TreantAtkRange.GetComponent<SphereCollider>().enabled = false;
    }

    public void Treant_Idle_NextAct()
    {
        // �������̸�
        if (isThink)
        {
            isThink = false;
            return;
        }

        if (!isThink && isStartRaid)
        {
            Treant_NextAct();
        }
  
    }
    #endregion

    #region Treant_Move
    // TODO ## Treant_Move
    public void Treant_Move()
    {
        isMove = true;
        isThink = false;
    }
    #endregion

    #region Treant_NormalAttack
    // TODO ## Treant_NormalAttack
    public void Normal_Attack()
    {
        isAttacking = true;
        isLock = true;
        animator.SetTrigger("PunchBig");
    }

    public void Normal_Attack_Next()
    {
        // ���� ���� ����
        StartCoroutine(Normal_Attack_Next_Motion());
    }

    IEnumerator Normal_Attack_Next_Motion()
    {
        isLock = false;

        yield return new WaitForSeconds(1.0f);
        isLock = true;
        animator.SetTrigger("PunchSmall");
    }

    public void Normal_Attack_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region Treant_Barrier
    // TODO ## Treant_Barrier
    public void Treant_Barrier()
    {
        isAttacking = true;
        animator.SetBool("isBlock", true);

       
    }

    public void Treant_Barrier_End()
    {
        StartCoroutine(Treant_Barrier_Start_Event());
    }

    IEnumerator Treant_Barrier_Start_Event()
    {
        yield return new WaitForSeconds(5.0f);
        isAttacking = false;
        animator.SetTrigger("BlockStop");
    }

    public void Treant_Barrier_End_Event()
    {
        animator.SetBool("isBlock", false);
        isThink = false;
    }
    #endregion

    // Normal Form
    #region Treant_LeafTurn
    // TODO ## Treant_LeafTurn
    public void Treant_LeafTurn()
    {
        isAttacking = true;
        isLock = true;
        animator.SetTrigger("LeafTurn");
    }

    public void Treant_LeafTurn_End()
    {
        isAttacking = false;
        isThink = false;
        isLock = false;
    }
    #endregion

    #region Treant_LeafBreath
    // TODO ## Treant_LeafBreath
    public void Treant_LeafBreath()
    {
        isAttacking = true;
        animator.SetTrigger("LeafBreath");
    }

    public void Treant_LeafBreath_Start()
    {
        Treant_Slow_RotSpeed = 4.0f;
    }

    public void Treant_LeafBreath_End()
    {
        Treant_Slow_RotSpeed = 0.0f;
        isAttacking = false;
        isThink = false;
    }
    #endregion

    #region Treant_LeafPlace
    // TODO ## Treant_LeafPlace
    public void Treant_LeafPlace()
    {
        isAttacking = true;
        animator.SetTrigger("LeafPlace");
    }

    public void Treant_LeafPlace_Start()
    {
        
    }

    public void Treant_LeafPlace_End()
    {
        isAttacking = false;
        isThink = false;
    }
    #endregion

    #region Treant_LeafMissale
    // TODO ## Treant_LeafMissale
    public void Treant_LeafMissale()
    {
        isAttacking = true;
        animator.SetTrigger("LeafMissale");
    }

    public void Treant_LeafMissale_Start()
    {

    }

    public void Treant_LeafMissale_End()
    {
        isAttacking = false;
        isThink = false;
    }
    #endregion

}
