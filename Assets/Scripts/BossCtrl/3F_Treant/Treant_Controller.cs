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
    LEAFMISSALE, // 2
    LEAFBREATH, // 3
    NORMALATTACK,
    BARRIER,
    LEAFTURN, // 6
    LEAFPLACE, // 7
    FORMCHANGE,
    END,
}

public enum Treant_Speed_State
{
    NONE = -1,
   
    IDLE,
    MOVE,
    TRUNWHEEL, // 2
    DASH,      // 3
    NORMALATTACK, // 4
    BARRIER,  //5
    EXPLOSION, // 6
    CLAP, // 7
    FORMCHANGE,
    END,
}

public enum Treant_Power_State
{
    NONE = -1,
   
    IDLE,
    MOVE,
    NORMALATTACK, // 2
    BARRIER,        // 3
    GOLEM_RECALL,   // 4
    THROW_STONE,    // 5
    HULK_BURST_1,   // 6
    HULK_BURST_2,   // 7
    FORMCHANGE,     
    END,
}

public class Treant_Controller : Boss_BehaviorCtrl_Base
{
    #region Variable
    [Header("----Treant_Ref_Variable---")]
    [SerializeField]
    Treant_ObjPool Skill_Obj_Pool;

    [Header("----Treant_Animation_Variable---")]
    [SerializeField]
    public Animator animator;
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
    public bool isStartRaid;
    [SerializeField]
    Transform Skill_StartPos;
    [SerializeField]
    Transform LookDir;

    [Header("----Treant_State_Variable---")]
    public bool isBarrier;                          // ��� ���� üũ
    public bool isStartFormChange;                  // ���ݺ��� ���� ��ȯ����
    [SerializeField]
    bool CanFormChange;                             // ���� ��ȯ ����
    [SerializeField]
    int ChangeForm_Skill_Max_Count;                 // ��� ���� ���� �Ұ��� ����
    [SerializeField]
    int FormChange_Count;                           // ��ų�� �� �� ����� ī��Ʈ
    [SerializeField]
    BlendShapesPresetManager Treant_Type_Shape;     // ���� ���� ���� ��ũ��
    [SerializeField]
    TreantType Treant_Type;                         // 3������ ����
    [SerializeField]
    Treant_Normal_State TreantNormalState;          // 3�� ���� �븻�� ���� ����
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
    float Treant_Normal_MoveSpeed;
    [SerializeField]
    float Treant_Speed_MoveSpeed;
    [SerializeField]
    float Treant_Power_MoveSpeed;
    [SerializeField]
    float Treant_RotSpeed;
    [SerializeField]
    float Treant_Slow_RotSpeed;
    [SerializeField]
    float Treant_Skill_Delay;
    [SerializeField]
    bool isThink;
    [SerializeField]
    bool isCurIdle;

    Vector3 dir;                  // Treant ����

    [Header("----Treant_Skill_Variable---")]
    [SerializeField]
    GameObject Shield_VFX;
    [SerializeField]
    GameObject Normal_Atk_R_VFX;
    [SerializeField]
    GameObject Normal_Atk_L_VFX;
    
    [Header("----Treant_FormChange_Variable---")]
    [SerializeField]
    GameObject FormChange_VFX;

    [Header("----Treant_Normal_LeafMissale_Variable---")]
    [SerializeField]
    GameObject NormalAtk_GuideLine;

    [Header("----Treant_Normal_LeafMissale_Variable---")]
    [SerializeField]
    float LeafMissale_Time;
    [SerializeField]
    float LeafMissale_Speed;
    [SerializeField]
    GameObject LeafMissale_VFX;
    [SerializeField]
    GameObject LeafMissale_GuideLine;

    [Header("----Treant_Normal_LeafTurn_Variable---")]
    [SerializeField]
    GameObject LeafTurn_VFX;
    [SerializeField]
    GameObject LeafTurn_GuideLine;

    [Header("----Treant_Normal_LeafBreath_Variable---")]
    [SerializeField]
    GameObject LeafBreath_VFX;
    [SerializeField]
    GameObject LeafBreath_GuideLine;

    [Header("----Treant_Speed_Dash_Variable---")]
    [SerializeField]
    int DashCount;
    [SerializeField]
    float DashTime;
    [SerializeField]
    float DashSpeed;
    [SerializeField]
    int ReDash_Percent;
    [SerializeField]
    bool isReDash;
    [SerializeField]
    GameObject DashGuideLine;
    [SerializeField]
    GameObject Dash_VFX;
    [SerializeField]
    bool isEnterCoroutine;

    [Header("----Treant_Speed_TurnWheel_Variable---")]
    [SerializeField]
    float TurnWheel_Time;
    [SerializeField]
    float TurnWheel_Speed;
    [SerializeField]
    GameObject TurnWheel_GuideLine;
    [SerializeField]
    GameObject TurnWheel_VFX;

    [Header("----Treant_Speed_Explosion_Variable---")]
    [SerializeField]
    GameObject Energy_VFX;
    [SerializeField]
    GameObject Explosion_GuideLine;
    [SerializeField]
    GameObject Shooting_VFX;

    [Header("----Treant_Speed_Clap_Variable---")]
    [SerializeField]
    GameObject Clap_VFX;
    [SerializeField]
    GameObject Clap_After_VFX;
    [SerializeField]
    GameObject Clap_Guide;
    [SerializeField]
    GameObject Clap_After_Guide;

    [Header("----Treant_Power_Golem_Variable---")]
    [SerializeField]
    float Golem_Active_Time;
    [SerializeField]
    GameObject Golem_GuideLine;

    [Header("----Treant_Power_HulkBurst1_Variable---")]
    [SerializeField]
    GameObject Stone;
    [SerializeField]
    Transform Stone_seize;
    [SerializeField]
    Transform Stone_Throw_Parent;
    [SerializeField]
    Transform R_VFX_Pos;
    [SerializeField]
    Transform L_VFX_Pos;

  [Header("----Treant_Power_HulkBurst2_Variable---")]
    [SerializeField]
    GameObject[] Combo_Atk_VFX;
    [SerializeField]
    GameObject[] Combo_Atk_GuideLines;
    [SerializeField]
    int Combo_Index;
    [SerializeField]
    float Crack_Forward;

#endregion

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
        else if (TargetDistance < ChaseDistance + 0.5f && !isAttacking) // �������� �ƴϰ� �����Ÿ��ȿ� ��� ���� ��
        {
            NotMove();
        }
    }
    #endregion

    #region Init
    void Treant_Init()
    {
        // ������ �� 80�� ���ϰ� �Ǹ� �ٷ� �۵��� �� �ֵ��� ���� �ʱ�ȭ ���ش�.
        FormChange_Count = ChangeForm_Skill_Max_Count;
        isLock = true;
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

        TreantSpeedState = Treant_Speed_State.NONE;
        TreantPowerState = Treant_Power_State.NONE;

        // �̵��ӵ� ����
        Treant_MoveSpeed = Treant_Normal_MoveSpeed;
        // ���� ��ȯ
        Treant_Type_Shape.StartTransitionToPreset("Reset");
        animator.SetTrigger("FormChange");
        animator.SetFloat("AnimSpeed", 1.0f);
    }

    public void Change_Speed_Form()
    {
        // TODO ## 3�� ���� ���ǵ� �� ü����
        Treant_Type = TreantType.SPEED;

        TreantNormalState = Treant_Normal_State.NONE;
        TreantPowerState = Treant_Power_State.NONE;


        // �̵��ӵ� ����
        Treant_MoveSpeed = Treant_Speed_MoveSpeed;
        // ���� ��ȯ
        Treant_Type_Shape.StartTransitionToPreset("Skinny");
        animator.SetTrigger("FormChange");
        animator.SetFloat("AnimSpeed", 1.5f);
    }
    public void Change_Power_Form()
    {
        // TODO ## 3�� ���� �Ŀ� �� ü����
        Treant_Type = TreantType.POWER;

        TreantNormalState = Treant_Normal_State.NONE;
        TreantSpeedState = Treant_Speed_State.NONE;

        // �̵��ӵ� ����
        Treant_MoveSpeed = Treant_Power_MoveSpeed;
        // ���� ��ȯ
        Treant_Type_Shape.StartTransitionToPreset("Fat");
        animator.SetTrigger("FormChange");
        animator.SetFloat("AnimSpeed", 1.0f);
    }

    // �븻 ���� �� ����
    public void CurrentType_Normal_Form_Change()
    {
        isLock = true;
        // �� ���� �Ұ���
        CanFormChange = false;

        int changePercent = Random.Range(0, 10);

        FormChange_VFX.SetActive(true);
        StartCoroutine(FormChange_VFX_Off());

        // 50 / 50 Ȯ���� ����
        if (changePercent < 5)
        {
            Change_Speed_Form();
        }
        else
        {
            Change_Power_Form();
        }
    }

    // ���ǵ� ���� �� ����
    public void CurrentType_Speed_Form_Change()
    {
        isLock = true;
        // �� ���� �Ұ���
        CanFormChange = false;

        int changePercent = Random.Range(0, 10);

        FormChange_VFX.SetActive(true);
        StartCoroutine(FormChange_VFX_Off());

        // 30 / 70 Ȯ���� ����
        if (changePercent < 3)
        {
            Change_Normal_Form();
        }
        else
        {
            Change_Power_Form();
        }
    }

    // �Ŀ� ���� �� ����
    public void CurrentType_Power_Form_Change()
    {
        isLock = true;
        // �� ���� �Ұ���
        CanFormChange = false;

        int changePercent = Random.Range(0, 10);

        FormChange_VFX.SetActive(true);
        StartCoroutine(FormChange_VFX_Off());

        // 30 / 70 Ȯ���� ����
        if (changePercent < 3)
        {
            Change_Normal_Form();
        }
        else
        {
            Change_Speed_Form();
        }
    }

    public void UnLock()
    {
        isLock = false;
    }

    IEnumerator FormChange_VFX_Off()
    {
        yield return new WaitForSeconds(10.0f);
        FormChange_VFX.SetActive(false);
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
            Treant_Normal_State randomNormalState = (Treant_Normal_State)Random.Range(2, (int)Treant_Normal_State.END - 1);
            // Treant_Normal_State randomNormalState = (Treant_Normal_State)4;
            TreantNormalState = randomNormalState;

            //Debug.Log(randomNormalState);
        }
        else if (Treant_Type == TreantType.POWER)
        {
            // �������� ���� ���� ����
            Treant_Power_State randomPowerState = (Treant_Power_State)Random.Range(2, (int)Treant_Power_State.END - 1);
            // Treant_Power_State randomPowerState = (Treant_Power_State)4;
            TreantPowerState = randomPowerState;

            //Debug.Log(randomPowerState);

        }
        else if (Treant_Type == TreantType.SPEED)
        {
            // �������� ���� ���� ����
            Treant_Speed_State randomSpeedState = (Treant_Speed_State)Random.Range(2, (int)Treant_Speed_State.END - 1);
            // Treant_Speed_State randomSpeedState = (Treant_Speed_State)7;
            TreantSpeedState = randomSpeedState;

            //Debug.Log(randomSpeedState);
        }

        // �⺻ ���°� �ƴ� �� 2�ʰ��� �����̸� �ش�
        //yield return new WaitForSeconds(0.0f);

        // ����� ���� �Ÿ����� �ָ� �̵� ����
        if (TargetDistance >= ChaseDistance + 1.0f)
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

        // �� ü���� �����Ҷ�
        if (isStartFormChange && FormChange_Count == ChangeForm_Skill_Max_Count)
        {
            if (Treant_Type == TreantType.NORMAL)
            {
                TreantNormalState = Treant_Normal_State.FORMCHANGE;
            }
            else if (Treant_Type == TreantType.POWER)
            {
                TreantPowerState = Treant_Power_State.FORMCHANGE;
            }
            else
            {
                TreantSpeedState = Treant_Speed_State.FORMCHANGE;
            }

            // ���� ������ 0���� �ʱ�ȭ
            FormChange_Count = 0;
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
                        Skill_Count_Up();
                        break;
                    case Treant_Normal_State.BARRIER:
                        Treant_Barrier();
                        Skill_Count_Up();
                        break;
                    case Treant_Normal_State.LEAFTURN:
                        Treant_LeafTurn();
                        Skill_Count_Up();
                        break;
                    case Treant_Normal_State.LEAFBREATH:
                        Treant_LeafBreath();
                        Skill_Count_Up();
                        break;
                    case Treant_Normal_State.LEAFPLACE:
                        Treant_LeafPlace();
                        Skill_Count_Up();
                        break;
                    case Treant_Normal_State.LEAFMISSALE:
                        Treant_LeafMissale();
                        Skill_Count_Up();
                        break;
                    case Treant_Normal_State.FORMCHANGE:
                        CurrentType_Normal_Form_Change();
                        break;
                    default:
                        break;
                }
                break;
                // ���ǵ� ���϶�
            case TreantType.SPEED:
                switch (TreantSpeedState)
                {
                    case Treant_Speed_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Speed_State.NORMALATTACK:
                        Normal_Attack();
                        Skill_Count_Up();
                        break;
                    case Treant_Speed_State.BARRIER:
                        Treant_Barrier();
                        Skill_Count_Up();
                        break;
                    case Treant_Speed_State.DASH:
                        Treant_Dash();
                        Skill_Count_Up();
                        break;
                    case Treant_Speed_State.TRUNWHEEL:
                        Treant_TurnWheel();
                        Skill_Count_Up();
                        break;
                    case Treant_Speed_State.EXPLOSION:
                        Treant_Explosion();
                        Skill_Count_Up();
                        break;
                    case Treant_Speed_State.CLAP:
                        Treant_Clap();
                        Skill_Count_Up();
                        break;
                    case Treant_Speed_State.FORMCHANGE:
                        CurrentType_Speed_Form_Change();
                        break;
                    default:
                        break;
                }
                break;
                // �Ŀ� ���϶�
            case TreantType.POWER:
                switch (TreantPowerState)
                {
                    case Treant_Power_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Power_State.NORMALATTACK:
                        Normal_Attack();
                        Skill_Count_Up();
                        break;
                    case Treant_Power_State.BARRIER:
                        Treant_Barrier();
                        Skill_Count_Up();
                        break;
                    case Treant_Power_State.GOLEM_RECALL:
                        Treant_Golem_Recall();
                        Skill_Count_Up();
                        break;
                    case Treant_Power_State.THROW_STONE:
                        Treant_Throw_Stone();
                        Skill_Count_Up();
                        break;
                    case Treant_Power_State.HULK_BURST_1:
                        Treant_Hulk_Burst_1();
                        Skill_Count_Up();
                        break;
                    case Treant_Power_State.HULK_BURST_2:
                        Treant_Hulk_Burst_2();
                        Skill_Count_Up();
                        break;
                    case Treant_Power_State.FORMCHANGE:
                        CurrentType_Power_Form_Change();
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }

    public void Skill_Count_Up()
    {
        // ���� ���� ���� ���� �������� ����
        if (isStartFormChange)
        {
            FormChange_Count++;
        }
    }
    #endregion

    #region Treant_Idle
    public void Treant_Idle()
    {
        isMove = false;
        animator.SetFloat("Locomotion", 0.5f);
        //�ٽ� Ȱ��ȭ
        // TreantAtkRange.GetComponent<SphereCollider>().enabled = false;
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

    #region Treant_FirstSee
    public void RaidStart_On()
    {
        isStartRaid = true;
        isLock = false;
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

    public void Normal_Attack_R_VFX_On()
    {
        Normal_Atk_R_VFX.SetActive(true);
        StartCoroutine(Normal_Attack_VFX_Off(Normal_Atk_R_VFX));
    }

    public void Normal_Attack_L_VFX_On()
    {
        Normal_Atk_L_VFX.SetActive(true);
        StartCoroutine(Normal_Attack_VFX_Off(Normal_Atk_L_VFX));
    }

    public void Normal_ATK_Guide_On()
    {
        NormalAtk_GuideLine.SetActive(true);
    }

    IEnumerator Normal_Attack_VFX_Off(GameObject _obj)
    {
        yield return new WaitForSeconds(0.5f);
        _obj.SetActive(false);
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
        isBarrier = true;
        Shield_VFX.SetActive(true);
    }

    public void Treant_Barrier_End()
    {
        StartCoroutine(Treant_Barrier_Start_Event());
    }

    IEnumerator Treant_Barrier_Start_Event()
    {
        yield return new WaitForSeconds(5.0f);
        isAttacking = false;
        Shield_VFX.SetActive(false);
        isBarrier = false;
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

    public void Treant_LeafTurn_Start()
    {
        LeafTurn_VFX.SetActive(true);
    }

    public void Treant_LeafTurn_GuideLine()
    {
        LeafTurn_GuideLine.SetActive(true);
    }

    public void Treant_LeafTurn_End()
    {
        LeafTurn_VFX.SetActive(false);
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

    public void Treant_LeafBreath_GuideLine()
    {
        // ���̵����
        LeafBreath_GuideLine.SetActive(true);
    }

    public void Treant_LeafBreath_Start()
    {
        // ȸ�� �ӵ� ����
        Treant_Slow_RotSpeed = 4.0f;
    }

    public void Treant_LeafBreath_VFX_Start()
    {
        LeafBreath_VFX.SetActive(true);
    }

    public void Treant_LeafBreath_End()
    {
        LeafBreath_VFX.SetActive(false);
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
        isLock = true;
    }

    public void Treant_LeafPlace_VFX()
    {
        // ������Ʈ Ǯ���� ���̵���� ������Ʈ ��� �´�
        GameObject LeafPlace_GuideLine = Skill_Obj_Pool.GetLeafPlace_Guide_FromPool();
        // ���̵���� ��ġ ����
        LeafPlace_GuideLine.transform.position = new Vector3 (Target.transform.position.x, 1.0f, Target.transform.position.z);

        // ����Ʈ �߻� �ڷ�ƾ ����
        StartCoroutine(LeafPlace_VFX(LeafPlace_GuideLine.transform.position, LeafPlace_GuideLine));
    }

    IEnumerator LeafPlace_VFX(Vector3 _position, GameObject _guideline)
    {
        yield return new WaitForSeconds(1.0f);
        // �Ű������� ���� ���̵���� ���ֱ�
        _guideline.SetActive(false);

        // ����Ʈ ��ġ
        GameObject leafPlace = Skill_Obj_Pool.GetLeafPlaceFromPool();
        leafPlace.transform.position = _position;
    }

    public void Treant_LeafPlace_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region Treant_LeafMissale
    // TODO ## Treant_LeafMissale
    public void Treant_LeafMissale()
    {
        isAttacking = true;
        isLock = true;
        animator.SetTrigger("LeafMissale");
    }

    public void Treant_LeafMissale_Start()
    {
        // �ڷ�ƾ ����
        StartCoroutine(Treant_LeafMissale_Start_Event());
        // �ڷ�ƾ ����� ����
        isEnterCoroutine = true;
    }
    
    public void Treant_LeafMissale_GuideLine()
    {
        LeafMissale_GuideLine.SetActive(true);
    }

    IEnumerator Treant_LeafMissale_Start_Event()
    {
        if (isEnterCoroutine == true)
            yield break;

        LeafMissale_VFX.SetActive(true);

        float time = 0.0f;

        while (time < LeafMissale_Time)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.back * LeafMissale_Speed * Time.deltaTime);
            yield return null;
        }

        LeafMissale_VFX.SetActive(false);
        animator.SetTrigger("LeafMissale_Out");
      
    }

    public void Treant_LeafMissale_End()
    {
        isEnterCoroutine = false;
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    // Speed Form
    #region Treant_Dash
    // TODO ## Treant_Dash
    public void Treant_Dash()
    {
        DashCount++; // �뽬 ī��Ʈ ����
        animator.SetTrigger("Dash");
    }

    public void Treant_Dash_Start()
    {
        // �ڷ�ƾ ����
        StartCoroutine(Treant_Dash_Start_Event());
        // �ڷ�ƾ ����� ����
        isEnterCoroutine = true;
    }

    IEnumerator Treant_Dash_Start_Event()
    {
        if (isEnterCoroutine == true)
            yield break;

        Dash_VFX.SetActive(true);

        isAttacking = true;
        // isLock = true;
        float time = 0.0f;

        while (time < DashTime)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.forward * DashSpeed * Time.deltaTime);
            yield return null;
        }

        Dash_VFX.SetActive(false);

        // ù��°�� �ι�° ���� �� ���
        if (DashCount != 0 && DashCount < 3)
        {
            // Ȯ��
            int re_Dash = Random.Range(0, 10);

            // ���� Ȯ��
            if (re_Dash < ReDash_Percent)
            {
                // ���� �Ѵٸ� true
                isReDash = true;
                // �ִϸ��̼� ���
                animator.SetTrigger("KeepDash");
            }
            else
            {
                isReDash = false;  
            }
        }

        // �뽬ī��Ʈ�� 3�̰ų� ���ӵ����� ���ٸ� ����������
        if (DashCount == 3 || !isReDash)
        {
            animator.SetTrigger("OutDash");
        }

        isEnterCoroutine = false;
    }

    public void Treant_Dash_GuideLine_On()
    {
        isLock = true;
        DashGuideLine.SetActive(true);
    }

    public void Treant_Keep_Dash_End()
    {
        isLock = false;
        Treant_Dash();
    }

    public void Treant_Out_Dash_End()
    {
        isLock = false;
        isAttacking = false;
        DashCount = 0;
        isThink = false;
    }
    #endregion

    #region Treant_TurnWheel
    // TODO ## Treant_TurnWheel
    public void Treant_TurnWheel()
    {
        isAttacking = true;
        animator.SetTrigger("TurnWheel");
    }

    public void Treant_TurnWheel_Start()
    {
        // �ڷ�ƾ ����
        StartCoroutine(Treant_TurnWheel_Start_Event());
        // ����Ʈ Ȱ��ȭ
        TurnWheel_VFX.SetActive(true);
        // �ڷ�ƾ ����� ����
        isEnterCoroutine = true;
    }

    IEnumerator Treant_TurnWheel_Start_Event()
    {
        if (isEnterCoroutine == true)
            yield break;

        float time = 0.0f;
        Treant_Slow_RotSpeed = 4.0f;

        while (time < TurnWheel_Time)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.forward * TurnWheel_Speed * Time.deltaTime);
            yield return null;
        }

        TurnWheel_VFX.SetActive(false);
        animator.SetTrigger("OutTurnWheel");
    }

    public void TurnWheel_Guide_On()
    {
        TurnWheel_GuideLine.SetActive(true);
        TurnWheel_GuideLine.transform.position = this.transform.position;
    }

    public void TurnWheel_VFX_Off()
    {
        // ����Ʈ ��Ȱ��ȭ
        TurnWheel_VFX.SetActive(false);
    }

    public void Treant_TurnWheel_End()
    {
        // ȸ���� ����ġ
        Treant_Slow_RotSpeed = 0.0f;
        // ���� �� �ƴ�
        isAttacking = false;
        // ���� �� �� ����
        isThink = false;
        // �ڷ�ƾ �ٳ���
        isEnterCoroutine = false;
    }
    #endregion

    #region Treant_Explosion
    // TODO ## Treant_Explosion
    public void Treant_Explosion()
    {
        isAttacking = true;
        animator.SetTrigger("Explosion");
    }

    public void Treant_Explosion_Start()
    {
        isLock = true;
        Explosion_GuideLine.SetActive(true);
    }

    public void Treant_Explosion_Energy()
    {
        // ������ Ȱ��ȭ
        Energy_VFX.SetActive(true);
    }

    public void Treant_Explosion_Shoot()
    {
        // ������ ����Ʈ ��Ȱ��ȭ
        Energy_VFX.SetActive(false);
        // ���� ����Ʈ Ȱ��ȭ
        Shooting_VFX.SetActive(true);
        StartCoroutine(On_Shoot_Collision());
    }

    IEnumerator On_Shoot_Collision()
    {
        yield return new WaitForSeconds(0.4f);
        // 0.3���� �ǰ� ���� �߻�
        Shooting_VFX.GetComponent<SphereCollider>().enabled = true;
    }

    public void Treant_Explosion_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
        Shooting_VFX.GetComponent<SphereCollider>().enabled = false;
        Shooting_VFX.SetActive(false);
    }
    #endregion

    #region Treant_Clap
    // TODO ## Treant_Clap
    public void Treant_Clap()
    {
        isAttacking = true;
        animator.SetTrigger("Clap");
    }

    public void Treant_Clap_Start()
    {
        // isLock = true;       
    }

    public void Treant_Clap_GuideLine()
    {
        isLock = true;

        Clap_Guide.SetActive(true);
    }

    public void Treant_Clap_After_GuideLine()
    {
        Clap_After_Guide.SetActive(true);
    }


    public void Treant_Clap_VFX_On()
    {
        // ����Ʈ Ȱ��ȭ
        Clap_VFX.SetActive(true);
        Clap_After_VFX.SetActive(true);

        StartCoroutine(Treant_Clap_VFX_Off());
    }

    IEnumerator Treant_Clap_VFX_Off()
    {
        yield return new WaitForSeconds(0.2f);
        Clap_VFX.SetActive(false);

        yield return new WaitForSeconds(1.7f);
        Clap_After_VFX.SetActive(false);
        isLock = false;
    }

    public void Treant_Clap_End()
    {
        isAttacking = false;
        isThink = false;
    }
    #endregion

    // Power Form
    #region Treant_Golem_Recall
    // TODO ## Treant_Golem_Recall
    public void Treant_Golem_Recall()
    {
        isAttacking = true;
        //isLock = true;
        animator.SetTrigger("Golem_Recall");
    }

    public void Treant_Golem_Guide_On()
    {
        // ���̵� ���� Ȱ��ȭ
        isLock = true;
        Golem_GuideLine.SetActive(true);
        Golem_GuideLine.transform.position = Skill_StartPos.position + Skill_StartPos.forward * 3.0f;
    }

    public void Treant_Golem_VFX_On()
    {
        // �� ����Ʈ Ȱ��ȭ
        GameObject golem = Skill_Obj_Pool.GetGolem_FromPool();
        golem.transform.position = Skill_StartPos.position;
        golem.transform.rotation = Skill_StartPos.rotation;
        StartCoroutine(Treant_Golem_VFX_Off(golem));
    }

    IEnumerator Treant_Golem_VFX_Off(GameObject _golem)
    {
        yield return new WaitForSeconds(Golem_Active_Time);
        _golem.SetActive(false);
    }
    public void Treant_Golem_Recall_End()
    {
        isAttacking = false;
        isThink = false;
        isLock = false;
    }
    #endregion

    #region Treant_Throw_Stone
    // TODO ## Treant_Throw_Stone
    public void Treant_Throw_Stone()
    {
        isLock = true;
        isAttacking = true;
        animator.SetTrigger("Throw_Stone");
    }

    public void Treant_Throw_Stone_Spawn()
    {
        Stone.SetActive(true);
        // ��ġ �ʱ�ȭ
        Vector3 Pos = Skill_StartPos.position + Skill_StartPos.forward * 5.0f;
        Stone.transform.position = new Vector3(Pos.x, -4.0f, Pos.z);
    }


    //  ���� 4���̶� ���� ���
    public void Treant_Throw_Stone_Seize()
    {
        // ���� �θ������Ʈ �ٲ���
        Stone.transform.parent = Stone_seize.transform;
        // ������ ����
        Stone.GetComponent<Stone_Ctrl>().isSeize = true;
        // ������ ��ġ�� �ʱ�ȭ �����ش�.
        Stone.transform.localPosition = Vector3.zero;
    }


    // ���� 4���̶� ���� ���
    public void Stone_Throw()
    {
        Stone.transform.parent = Stone_Throw_Parent;

        Vector3 direction = Target.transform.position - Stone.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Stone.transform.rotation = rotation;

        // ������
        Stone.GetComponent<Stone_Ctrl>().isThrow = true;
        // ���̵���� Ȱ��ȭ
        Skill_Obj_Pool.GetStone_Guide_FromPool().transform.position = new Vector3(Target.transform.position.x, 1.0f, Target.transform.position.z);
    }


    //  ���� 4���̶� ���� ���
    public void Treant_Throw_Stone_Lock()
    {
        isLock = false;
    }

    public void Treant_Throw_Stone_End()
    {
        isAttacking = false;
        isThink = false;
    }
    #endregion

    #region Treant_Hulk_Burst_1
    // TODO ## Treant_Hulk_Burst_1
    public void Treant_Hulk_Burst_1()
    {
        isAttacking = true;
        isLock = true;
        animator.SetTrigger("Hulk_Burst_1");
    }

    public void Treant_Hulk_Burst_1_Start()
    {

    }

    public void Treant_Hulk_Throw_Stone_Spawn()
    {
        Stone.SetActive(true);
        Vector3 Pos = Skill_StartPos.position + Skill_StartPos.forward * 15.0f;
        Stone.transform.position = new Vector3(Pos.x, -4.0f, Pos.z);
    }

    public void Spawn_Stone_Crash_GuideLine(float _pos)
    {
        if (_pos == 0)
        {
            Skill_Obj_Pool.GetStone_Guide_FromPool().transform.position = R_VFX_Pos.position;
        }
        else
        {
            Skill_Obj_Pool.GetStone_Guide_FromPool().transform.position = L_VFX_Pos.position;
        }
    }

    public void Stone_Crash_R_VFX_On()
    {
        // ����Ʈ ����
        GameObject obj = Skill_Obj_Pool.GetStone_Crash_FromPool();
        obj.transform.position = R_VFX_Pos.position;
    }

    public void Stone_Crash_L_VFX_On()
    {
        // ����Ʈ ����
        GameObject obj = Skill_Obj_Pool.GetStone_Crash_FromPool();
        obj.transform.position = L_VFX_Pos.position;
    }

    public void Treant_Hulk_Burst_1_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region  Treant_Hulk_Burst_2 3Combo
    // TODO ## Treant_Hulk_Burst_2
    public void Treant_Hulk_Burst_2()
    {
        isAttacking = true;
        animator.SetTrigger("Hulk_Burst_2");
    }

    // ȸ�� ����
    public void Treant_Hulk_Burst_2_Start()
    {
        isLock = true;

        // �ܰ迡�´� ���̵���� Ȱ��ȭ
        if (Combo_Index == 0)
        {
            Combo_Atk_GuideLines[0].SetActive(true);
        }
        else if (Combo_Index == 1)
        {
            Combo_Atk_GuideLines[1].SetActive(true);
        }
        else if (Combo_Index == 2)
        {
            Combo_Atk_GuideLines[2].SetActive(true);
        }
    }

    // ȸ�� ���� ����
    public void Treant_Hulk_Burst_2_Lock()
    {
        isLock = false;
    }

    public void Treant_Hulk_Burst2_VFX_On()
    {
        if (Combo_Index == 0)
        {
            // �޺� 1 ����
            Combo_Atk_VFX[0].SetActive(true);
            Combo_Atk_VFX[0].transform.position = Skill_StartPos.position + Skill_StartPos.forward * Crack_Forward;
            Combo_Atk_VFX[0].transform.rotation = Skill_StartPos.rotation;
            Combo_Index++;
        }
        else if (Combo_Index == 1)
        {

            // �޺� 2����
            Combo_Atk_VFX[1].SetActive(true);
            Combo_Atk_VFX[1].transform.position = Skill_StartPos.position + Skill_StartPos.forward * Crack_Forward;
            Combo_Atk_VFX[1].transform.rotation = Skill_StartPos.rotation;
            Combo_Index++;
        }
        else if (Combo_Index == 2)
        {
            // �޺� 3����
            Combo_Atk_VFX[2].SetActive(true);
            Combo_Atk_VFX[2].transform.position = Skill_StartPos.position + Skill_StartPos.forward * Crack_Forward;
            Combo_Atk_VFX[2].transform.rotation = Skill_StartPos.rotation;
            Combo_Index = 0;

        }
    }

    public void Treant_Hulk_Burst_2_End()
    {
        isAttacking = false;
        isThink = false;
        isLock = false;
    }
    #endregion 
}
