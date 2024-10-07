using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurentElement_State
{
    THUNDER_DRAGON,
    ICE_DRAGON,
    FIRE_DRAGON,
}


public enum IceDragon_State
{
    NONE,                       // 0
    ICE_IDLE,                   // 1
    CHANGE_FORM,                // 2
    ICE_MOVE,                   // 3
    ICE_FLY_NORMAL_ATK,         // 4
    ICE_WIND_ATK,               // 5
    ICE_DASH_ATK,               // 6
    ICE_BREATH_ATK,             // 7
    ICE_TSUNAMI_ATK,            // 8
    ICE_DROPDOWN_ATK,           // 9
    ICE_WAVE_ATK,               // 10
    ICE_ARROW_ATK,              // 11
    ICE_CLOSE_NORMAL_ATK,       // 12
    END
}

public enum ThunderDragon_State
{
    NONE,                       // 0
    THUNDER_IDLE,               // 1
    CHANGE_FORM,                // 2
    THUNDER_MOVE,               // 3
    THUNDER_FLY_NORMAL_ATK,     // 4
    THUNDER_WIND_ATK,           // 5
    THUNDER_DASH_ATK,           // 6
    THUNDER_BREATH_ATK,         // 7
    THUNDER_THUNDER_ATK,        // 8
    THUNDER_LASER_ATK,          // 9
    THUNDER_ROPE_ATK,           // 10
    THUNDER_CLOSE_NORMAL_ATK,   // 11
    END
}

public enum FireDragon_State
{
    NONE,                       // 0
    FIRE_IDLE,                  // 1
    FIRE_MOVE,                  // 2
    FIRE_FLY_NORMAL_ATK,        // 3
    FIRE_WIND_ATK,              // 4
    FIRE_DASH_ATK,              // 5
    FIRE_BREATH_ATK,            // 6
    FIRE_DRAGON_DROP,           // 7
    FIRE_DRAGON_FURY,           // 8
    FIRE_BALL_ATK,              // 9
    FIRE_HEAL,                  // 10
    FIRE_CLOSE_NORMAL_ATK,      // 11
    END
}

public class Dragon_Controller : Boss_BehaviorCtrl_Base
{
    #region Variable
    [Header("-----Dragon State-----")]
    public bool isLock;               // ���� ���� ����
    public bool isAttacking;          // ���� �� ���� ����
    [SerializeField]
    bool isThink;

    public CurentElement_State CurrentElement;
    public IceDragon_State IceDragonState;
    public ThunderDragon_State ThunderDragonState;
    public FireDragon_State FireDragonState;

    [Header("-----Dragon Reference-----")]
    public Boss_HP_Controller boss_hp_ctrl;  // HP ��Ʈ�ѷ�
    [SerializeField]
    Dragon_ObjPool Dragon_ObjPoolRef;

    [Header("-----Dragon Variable-----")]
    [SerializeField]
    bool isEnterCoroutine;
    public GameObject Target;       // �÷��̾�
    public float TargetDistance;    // �÷��̾���� �Ÿ�
    [SerializeField]
    int NextSkillNum;
    [SerializeField]
    float Slow_RotSpeed;
    [SerializeField]
    Transform DragonPos;
    [SerializeField]
    Transform DragonStartPos;

    [Header("-----Dragon State Variable-----")]
    public int MaxHP;   // �巡�� ü��

    [SerializeField]
    float Boss_RotSpeed;    //  ȸ�� �ӵ�
    [SerializeField]
    float moveSpeed;        // ������ �ӵ�
    public float ChaseDistance; // ��ų ���� ���� ����
    [SerializeField]
    GameObject Skill_Pos; // ��ų ���� ��ġ
    [SerializeField]
    GameObject Skill_Look; // ��ų�� �ٶ󺸴� ����
    Vector3 dir; // ����

    [Header("-----Animation Var-----")]
    public Animator Dragon_animator;   // �ִϸ�����
    public bool isMove;         // �̵� ����


    [Header("-----Dragon_Normal_ATK-----")]
    [SerializeField]
    bool isEnterDown;
    [SerializeField]
    float DownValue_Min;
    [SerializeField]
    float NormalAtk_Fly_Speed;
    [SerializeField]
    float NormalAtk_Down_Speed;

    [Header("-----Dragon_Wind_ATK-----")]
    [SerializeField]
    bool isEnterWindAtk;
    [SerializeField]
    bool isEnterLeftMove;
    [SerializeField]
    float Left_MaxTime;
    [SerializeField]
    float Left_Time;
    [SerializeField]
    float LeftSpeed;
    [SerializeField]
    float WindAtk_MaxTime;
    [SerializeField]
    float WindAtk_Time;

    [Header("-----Dragon_Dash_ATK-----")]
    [SerializeField]
    Transform[] Dash_StartPos;
    [SerializeField]
    bool isEnterDashAtk;
    [SerializeField]
    float Dash_MaxTime;
    [SerializeField]
    float Dash_Time;
    [SerializeField]
    float DashSpeed;

    [Header("-----Dragon_Breath-----")]
    [SerializeField]
    float BreathTime;

    [Header("-----Dragon_ICE_DropDown-----")]
    [SerializeField]
    bool isEnterDropDown;
    [SerializeField]
    float Drop_Time;
    [SerializeField]
    float Drop_MaxTime;

    [Header("-----Dragon_ICE_Arrow-----")]
    [SerializeField]
    bool isEnterIceArrow;
    [SerializeField]
    float IceArrow_Time;
    [SerializeField]
    float IceArrow_MaxTime;

    [Header("-----Dragon_Thunder_Thunder_Atk-----")]
    [SerializeField]
    bool isEnterThunderAtk;
    [SerializeField]
    float Thunder_Time;
    [SerializeField]
    float Thunder_MaxTime;

    [Header("-----Dragon_Thunder_Rope_Atk-----")]
    [SerializeField]
    bool isEnterRopeAtk;
    [SerializeField]
    float Rope_Time;
    [SerializeField]
    float Rope_MaxTime;
    #endregion

    #region Dragon_Rotate
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

    public void Reaper_Lock()
    {
        isLock = true;
    }

    public void Reaper_UnLock()
    {
        isLock = false;
    }
    #endregion

    #region Dragon_Move
    public override void Move()
    {
        // �÷��̾ ã�� �� ���ٸ� ���� ����
        if (Target == null)
            return;

        if (isMove && isAttacking == false)
        {
            Dragon_animator.SetBool("isMove", isMove);
            // ������ �̵�
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        //else if (!isMove && isAttacking == false)
        //{
        //    reaperState = ReaperState.Idle;
        //    Reaper_animator.SetBool("isMove", isMove);
        //}
    }

    public void NotMove()
    {
        if (Target == null)
            return;

        isMove = false;

        if (!isMove && isAttacking == false)
        {
            transform.Translate(Vector3.forward * 0.0f * Time.deltaTime);
            //animator.SetFloat("Locomotion", 0.5f);
            Dragon_animator.SetBool("isMove", false);
        }
    }
    #endregion

    #region Start/Update
    // Start is called before the first frame update
    void Start()
    {
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        Dragon_ObjPoolRef = GetComponent<Dragon_ObjPool>();
        Dragon_animator = GetComponent<Animator>();

        // CurrentElement = CurentElement_State.ICE_DRAGON;
        // CurrentElement = CurentElement_State.FIRE_DRAGON;
        CurrentElement = CurentElement_State.THUNDER_DRAGON;

        // IceDragonState = IceDragon_State.ICE_IDLE;
        //FireDragonState = FireDragon_State.FIRE_IDLE;
        ThunderDragonState = ThunderDragon_State.THUNDER_IDLE;

        IceDragonState = IceDragon_State.NONE;
        //ThunderDragonState = ThunderDragon_State.NONE;
        FireDragonState = FireDragon_State.NONE;
       

        isMove = false;

        // �ִ� ü�� ���� ���� ü�� 
        boss_hp_ctrl.BossMaxHP = MaxHP;
    }

    private void FixedUpdate()
    {
        // ���� ���°� �����̴� Move�� ����� �ָ�
        if (TargetDistance >= ChaseDistance)
        {
            Move();
        }
        else if (TargetDistance < ChaseDistance + 1.0f && !isAttacking) // �������� �ƴϰ� �����Ÿ��ȿ� ��� ���� ��
        {
            NotMove();
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

    #region Dragon_NextAct

    public void Dragon_NextAct()
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

        if (CurrentElement == CurentElement_State.ICE_DRAGON)
        {
            // �������� ���� ���� ����
            IceDragon_State randomIceState = (IceDragon_State)Random.Range(8, (int)IceDragon_State.END - 1);
            //IceDragon_State randomIceState = (IceDragon_State)8;
            IceDragonState = randomIceState;

            //Debug.Log(randomPowerState);

        }
        // ���� �ൿ ����
        else if (CurrentElement == CurentElement_State.THUNDER_DRAGON)
        {
            // �������� ���� ���� ����
            ThunderDragon_State randomThunderState = (ThunderDragon_State)Random.Range(3, (int)ThunderDragon_State.END - 1);
            //ThunderDragon_State randomThunderState = (ThunderDragon_State)4;
            ThunderDragonState = randomThunderState;

            //Debug.Log(randomNormalState);
        }

        else if (CurrentElement == CurentElement_State.FIRE_DRAGON)
        {
            // �������� ���� ���� ����
            FireDragon_State randomFireState = (FireDragon_State)Random.Range(2, (int)FireDragon_State.END - 1);
            // Treant_Speed_State randomSpeedState = (Treant_Speed_State)2;
            FireDragonState = randomFireState;

            //Debug.Log(randomSpeedState);
        }

        // �⺻ ���°� �ƴ� �� 2�ʰ��� �����̸� �ش�
        //yield return new WaitForSeconds(0.0f);

        // ����� ���� �Ÿ����� �ָ� �̵� ����
        if (TargetDistance >= ChaseDistance + 1.0f)
        {
            if (CurrentElement == CurentElement_State.ICE_DRAGON)
            {
                //IceDragon_State randomIceState = (IceDragon_State)Random.Range(3, 8);
                IceDragon_State randomIceState = (IceDragon_State)11;
                IceDragonState = randomIceState;
            }
            else if(CurrentElement == CurentElement_State.THUNDER_DRAGON)
            {
                //ThunderDragon_State randomThunderState = (FireDragon_State)Random.Range(3, 8);
                ThunderDragon_State randomThunderState = (ThunderDragon_State)10;
                ThunderDragonState = randomThunderState;
            }
           
            else
            {
                //FireDragon_State randomFireState = (FireDragon_State)Random.Range(3, 8);
                FireDragon_State randomFireState = (FireDragon_State)10;
                FireDragonState = randomFireState;
            }
        }

        #region Dragon_FormChange_Function
        Change_Thuunder();
        Change_Fire();
        #endregion

        switch (CurrentElement)
        {
            // ���� ���϶�
            case CurentElement_State.ICE_DRAGON:
                switch (IceDragonState)
                {
                    case IceDragon_State.ICE_MOVE:
                        Dragon_Move();
                        break;
                    case IceDragon_State.CHANGE_FORM:
                        Change_Thunder_Element();
                        break;
                    case IceDragon_State.ICE_FLY_NORMAL_ATK:
                        Fly_NormalAtk();
                        break;
                    case IceDragon_State.ICE_CLOSE_NORMAL_ATK:
                        Close_NormalAtk();
                        break;
                    case IceDragon_State.ICE_WIND_ATK:
                        Wind_Atk();
                        break;
                    case IceDragon_State.ICE_DASH_ATK:
                        Dash_Atk();
                        break;
                    case IceDragon_State.ICE_BREATH_ATK:
                        Breath_Atk();
                        break;
                    case IceDragon_State.ICE_TSUNAMI_ATK:
                        Tsunami_Atk();
                        break;
                    case IceDragon_State.ICE_DROPDOWN_ATK:
                        DropDown_Atk();
                        break;
                    case IceDragon_State.ICE_WAVE_ATK:
                        IceWave_Atk();
                        break;
                    case IceDragon_State.ICE_ARROW_ATK:
                        IceArrow_Atk();
                        break;
                    default:
                        break;
                }
                break;
            // ���� ���� �� ��ų
            case CurentElement_State.THUNDER_DRAGON:
                switch (ThunderDragonState)
                {
                    case ThunderDragon_State.THUNDER_MOVE:
                        Dragon_Move();
                        break;
                    case ThunderDragon_State.CHANGE_FORM:
                        Change_Fire_Element();
                        break;
                    case ThunderDragon_State.THUNDER_FLY_NORMAL_ATK:
                        Fly_NormalAtk();
                        break;
                    case ThunderDragon_State.THUNDER_CLOSE_NORMAL_ATK:
                        Close_NormalAtk();
                        break;
                    case ThunderDragon_State.THUNDER_WIND_ATK:
                        Wind_Atk();
                        break;
                    case ThunderDragon_State.THUNDER_DASH_ATK:
                        Dash_Atk();
                        break;
                    case ThunderDragon_State.THUNDER_BREATH_ATK:
                        Breath_Atk();
                        break;
                    case ThunderDragon_State.THUNDER_THUNDER_ATK:
                        Thunder_Atk();
                        break;
                    case ThunderDragon_State.THUNDER_LASER_ATK:
                        Laser_Atk();
                        break;
                    case ThunderDragon_State.THUNDER_ROPE_ATK:
                        Rope_Atk();
                        break;
                    default:
                        break;
                }
                break;
            // �� ���϶�
            case CurentElement_State.FIRE_DRAGON:
                switch (FireDragonState)
                {
                    case FireDragon_State.FIRE_MOVE:
                        Dragon_Move();
                        break;
                    case FireDragon_State.FIRE_FLY_NORMAL_ATK:
                        Fly_NormalAtk();
                        break;
                    case FireDragon_State.FIRE_CLOSE_NORMAL_ATK:
                        Close_NormalAtk();
                        break;
                    case FireDragon_State.FIRE_WIND_ATK:
                        Wind_Atk();
                        break;
                    case FireDragon_State.FIRE_DASH_ATK:
                        Dash_Atk();
                        break;
                    case FireDragon_State.FIRE_BREATH_ATK:
                        Breath_Atk();
                        break;
                    case FireDragon_State.FIRE_DRAGON_FURY:
                        Fire_Fury();
                        break;
                    case FireDragon_State.FIRE_DRAGON_DROP:
                        Fire_Drop();
                        break;
                    case FireDragon_State.FIRE_BALL_ATK:
                        FireBall_Atk();
                        break;
                    case FireDragon_State.FIRE_HEAL:
                        Fire_Heal();
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

    #region Dragon_Atk
    public void Atk_Finish()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }

    public void Atk_Start()
    {
        isAttacking = true;
        isLock = true;
    }
    #endregion

    #region Dragon_Idle
    public void Dragon_Idle()
    {
        // �������̸�
        if (isThink)
        {
            isThink = false;
            // return;
        }

        if (!isThink && GameManager.GMInstance.Get_PlaySceneManager().isRaidStart == true)
        {
            Dragon_NextAct();
        }
    }
    #endregion

    #region Dragon_Move
    public void Dragon_Move()
    {
        isMove = true;
        isThink = false;
    }
    #endregion

    #region Dragon_Change
    public void Change_Thunder_Element()
    {
        Debug.Log(1);

        // �����巡�� ����
        Dragon_animator.SetTrigger("ChangeThunderForm");

        // ���°� �ʱ�ȭ
        CurrentElement = CurentElement_State.THUNDER_DRAGON;
        IceDragonState = IceDragon_State.NONE;
        ThunderDragonState = ThunderDragon_State.THUNDER_IDLE;
        FireDragonState = FireDragon_State.NONE;
    }

    public void Change_Fire_Element()
    {
        // �� �巡�� ����
        Dragon_animator.SetTrigger("ChangeFireForm");

        // ���°� �ʱ�ȭ
        CurrentElement = CurentElement_State.FIRE_DRAGON;
        IceDragonState = IceDragon_State.NONE;
        ThunderDragonState = ThunderDragon_State.NONE;
        FireDragonState = FireDragon_State.FIRE_IDLE;
    }

    public void Change_Thuunder()
    {
        // ���� ������ �����Ҷ�
        if (boss_hp_ctrl.isChange_Thunder == true && CurrentElement == CurentElement_State.ICE_DRAGON)
        {
            IceDragonState = IceDragon_State.CHANGE_FORM;
        }
    }

    public void Change_Fire()
    {
        // �� ������ �����Ҷ�
        if (boss_hp_ctrl.isChange_Fire == true && CurrentElement == CurentElement_State.THUNDER_DRAGON)
        {
            ThunderDragonState = ThunderDragon_State.CHANGE_FORM;
        }
    }

    #endregion

    #region Dragon_NormalAtk
    public void Fly_NormalAtk()
    {
        Dragon_animator.SetTrigger("Fly_NormalAtk_1");
    }

    // ĳ���͸� ���� �ٰ������� ȣ��
    public void Fly_Forward()
    {
        StartCoroutine(Dragon_Fly_Forward_Start_Event());
        // �ߺ� ����
        isEnterCoroutine = true;
    }

    // ĳ���͸� ���� �ٰ���
    IEnumerator Dragon_Fly_Forward_Start_Event()
    {
        if (isEnterCoroutine == true)
            yield break;

        isAttacking = true;
        // isLock = true;

        while (TargetDistance >= ChaseDistance + 1.0f)
        {
            transform.Translate(Vector3.forward * NormalAtk_Fly_Speed * Time.deltaTime);
            yield return null;
        }

        Dragon_animator.SetTrigger("Fly_NormalAtk_2");       
    }

    public void Fly_Down()
    {
        StartCoroutine(Dragon_Fly_Down_Event());
        // �ߺ� ����
        isEnterDown = true;
    }

    // ������ ��������
    IEnumerator Dragon_Fly_Down_Event()
    {
        if (isEnterDown == true)
            yield break;


        // �巡���� �ϰ���
        while (this.DragonPos.localPosition.z < DownValue_Min)
        {
            transform.Translate(Vector3.down * NormalAtk_Down_Speed * Time.deltaTime);
            yield return null;
        }
    }

    // �ϰ� ����� �������� ��ġ �ٽ� ����
    public void Down_Finish()
    {
        isEnterDown = false;

        // ������Ʈ�� ���� ��ġ�� ������
        Vector3 currentPosition = DragonPos.localPosition;
        currentPosition.z = 0.0f;
        // ����� ��ġ�� �ٽ� �Ҵ�
        DragonPos.localPosition = currentPosition;
    }

    public void Close_NormalAtk()
    {
        Dragon_animator.SetTrigger("Close_NormalAtk");
    }

    public void Fly_Atk_Finish()
    {
        isEnterCoroutine = false;
        isAttacking = false;
        isLock = false;
        isThink = false;
    }

    #endregion

    #region Dragon_WindAtk
    public void Wind_Atk()
    {
        Dragon_animator.SetTrigger("WindAtk");
    }

    // �ٶ� ���� ����
    public void Wind_SideMove()
    {
        StartCoroutine(Wind_LeftMove());
        isEnterLeftMove = true;
    }

    // �ٶ� ���� ����
    IEnumerator Wind_LeftMove()
    {
        if (isEnterLeftMove == true)
            yield break;

        while (Left_Time <= Left_MaxTime)
        {
            Left_Time += Time.deltaTime;
            transform.Translate(Vector3.left * LeftSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // �ٶ� ���� ����
    public void Wind_Attaking()
    {
        StartCoroutine(WindAttack());
        isEnterWindAtk = true;
    }

    // �ٶ� ���� ����
    IEnumerator WindAttack()
    {
        if (isEnterWindAtk == true)
            yield break;

        while (WindAtk_Time <= WindAtk_MaxTime)
        {
            WindAtk_Time += Time.deltaTime;
            yield return null;
        }

        Wind_Atk_Finish_Anim();

    }

    // ���� �� �ִ�
    public void Wind_Atk_Finish_Anim()
    {
        Dragon_animator.SetTrigger("WindAtk_End"); 
    }

    // ���� �� ����
    public void WindAtk_Finish()
    {
        isEnterLeftMove = false;
        isEnterWindAtk = false;
        WindAtk_Time = 0.0f;
        Left_Time = 0.0f;
    }
    #endregion

    #region Dragon_Dash
    public void Dash_Atk()
    {
        Dragon_animator.SetTrigger("DashAtk");
    }

    public void Dash_Atk_MoveStart()
    {
        StartCoroutine(Dash_Atk_Move());
        isEnterDashAtk = true;
    }

    IEnumerator Dash_Atk_Move()
    {
        if (isEnterDashAtk == true)
            yield break;

        while (Dash_Time <= Dash_MaxTime)
        {
            Dash_Time += Time.deltaTime;
            transform.Translate(Vector3.forward * DashSpeed * Time.deltaTime);
            yield return null;
        }

        Dash_Atk_End();
    }

    public void Dash_Atk_End()
    {
        Dragon_animator.SetTrigger("DashAtk_End");
    }

    public void Dash_Atk_End_Pos()
    {
        DragonPos.localPosition = Vector3.zero;

        Dash_Time = 0.0f;
        isAttacking = false;
        isThink = false;
        isLock = false;
        isEnterDashAtk = false;
    }

    public void Dash_Atk_Pos()
    {
        DragonPos.localPosition = Dash_StartPos[Random.Range(0, Dash_StartPos.Length)].transform.localPosition;
    }
    #endregion

    #region Dragon_Breath
    public void Breath_Atk()
    {
        Dragon_animator.SetTrigger("BreathAtk");
    }

    public void Breath_AnimTime()
    {
        Dragon_animator.SetFloat("BreathAnimSpeed", BreathTime);
    }

    public void Breath_Atk_End()
    {
        Dragon_animator.SetFloat("BreathAnimSpeed", 1.0f);

        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region Ice_Dragon_Tsunami
    public void Tsunami_Atk()
    {
        Dragon_animator.SetTrigger("Tsunami_Atk");
    }

    public void Set_Tsunmai_Pos()
    {
        DragonPos.localPosition = Vector3.zero;
        isAttacking = true; 
    }

    public void Tsunami_Start()
    {
        isLock = true;
    }

    public void Tsunami_End()
    {
        isThink = false;
        isAttacking = false;
        isLock = false;
    }
    #endregion

    #region Ice_Dragon_DropDown
    public void DropDown_Atk()
    {
        Dragon_animator.SetTrigger("DropDownStart");
        isAttacking = true;
    }

    public void DropDown_Start()
    {
        StartCoroutine(DropDown_Time());
        DragonPos.position = Vector3.zero;
        isEnterDropDown = true;
    }

    IEnumerator DropDown_Time()
    {
        if (isEnterDropDown == true)
            yield break;

        while (Drop_Time <= Drop_MaxTime)
        {
            Drop_Time += Time.deltaTime;
            yield return null;
        }

        DropDown_Atk_Anim();
    }

    public void DropDown_Atk_Anim()
    {
        DragonPos.localPosition = Vector3.zero;
        Dragon_animator.SetTrigger("DropDownAtk");
    }

    public void DropDown_End()
    {
        isEnterDropDown = false;
        Drop_Time = 0.0f;
        isThink = false;
        isAttacking = false;
    }

    #endregion

    #region Ice_Dragon_Wave
    public void IceWave_Atk()
    {
        Dragon_animator.SetTrigger("IceWaveAtk");
    }

    public void IceWave_Center_Land()
    {
        DragonPos.localPosition = Vector3.zero;
        isAttacking = true;
        isLock = true;
    }

    public void IceWave_Center_Jump()
    {
        DragonPos.localPosition = new Vector3(100.0f, 100.0f, 100.0f);
    }

    public void IceWave_Atk_End()
    {
        isAttacking = false;
        isThink = false;
        isLock = false;
    }
    #endregion

    #region Ice_Dragon_IceArrow
    public void IceArrow_Atk()
    {
        Dragon_animator.SetTrigger("IceArrowAtk");
    }

    public void IceArrow_Start()
    {
        StartCoroutine(IceArrowTime());
        isEnterIceArrow = true;
        isAttacking = true;
    }

    IEnumerator IceArrowTime()
    {
        if (isEnterIceArrow == true)
            yield break;

        while (IceArrow_Time <= IceArrow_MaxTime)
        {
            IceArrow_Time += Time.deltaTime;
            yield return null;
        }

        IceArrow_End();
    }

    public void IceArrow_End()
    {
        Dragon_animator.SetTrigger("IceArrowAtk_End");
        IceArrow_Time = 0.0f;
        isEnterIceArrow = false;
        isAttacking = false;
        isThink = false;
    }
    #endregion

    #region Fire_Dragon_Drop
    public void Fire_Drop()
    {
        Dragon_animator.SetTrigger("Fire_Drop");
        isLock = true;
        isAttacking = true;
    }

    public void Fire_Drop_Jump()
    {
        DragonPos.localPosition = new Vector3(100.0f, 100.0f, 100.0f);
    }

    public void Fire_Drop_Land()
    {
        DragonPos.localPosition = Vector3.zero;
    }

    public void Fire_Drop_Down()
    {
        DragonPos.localPosition = Vector3.zero;
    }

    public void Fire_Drop_Stump()
    {
        
    }

    public void Fire_Drop_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;

    }
    #endregion

    #region Fire_Dragon_Fury
    public void Fire_Fury()
    {
        Dragon_animator.SetTrigger("Fire_Fury");
        isAttacking = true;
    }

    public void Fire_Fury_Start()
    {
        isLock = true;
    }
    #endregion

    #region Fire_Dragon_FireBall
    public void FireBall_Atk()
    {
        Dragon_animator.SetTrigger("FireBall");
        isAttacking = true;
    }

    public void FireBall_End()
    {
        isAttacking = false;
        isThink = false;
    }

    #endregion

    #region Fire_Dragon_Heal
    public void Fire_Heal()
    {
        Dragon_animator.SetTrigger("Fire_Heal");
    }
    #endregion

    #region Thunder_Dragon_ThunderAtk
    public void Thunder_Atk()
    {
        Dragon_animator.SetTrigger("Thunder_Atk");
    }

    public void Thunder_Flying()
    {
        StartCoroutine(Thunder_Atk_Fly());
        isEnterThunderAtk = true;
    }

    IEnumerator Thunder_Atk_Fly()
    {
        if (isEnterThunderAtk == true)
            yield break;

        while (Thunder_Time <= Thunder_MaxTime)
        {
            Thunder_Time += Time.deltaTime;
            yield return null;
        }

        Thunder_Atk_Fly_End();
    }

    public void Thunder_Atk_Fly_End()
    {
        Dragon_animator.SetTrigger("Thunder_Atk_End");
    }

    public void Thunder_Atk_End()
    {
        Thunder_Time = 0.0f;
        isEnterThunderAtk = false;
        isAttacking = false;
        isThink = false;
    }
    #endregion

    #region Thunder_Dragon_Laser
    public void Laser_Atk()
    {
        Dragon_animator.SetTrigger("Laser_Atk");
    }
    #endregion

    #region Thunder_Dragon_Rope_Atk
    public void Rope_Atk()
    {
        Dragon_animator.SetTrigger("Rope_Atk_Start");
    }

    public void Rope_Jump()
    {
        DragonPos.localPosition = new Vector3(100.0f, 100.0f, 100.0f);
    }

    public void Rope_Center()
    {
        StartCoroutine(Rope_Atk_Fly());
        DragonPos.localPosition = Vector3.zero;
        isAttacking = true;
        isLock = true;
    }

    IEnumerator Rope_Atk_Fly()
    {
        if (isEnterRopeAtk == true)
            yield break;

        while (Rope_Time <= Rope_MaxTime)
        {
            Rope_Time += Time.deltaTime;
            yield return null;
        }

        Rope_Atk_Fly_End();
    }

    public void Rope_Atk_Fly_End()
    {
        Dragon_animator.SetTrigger("Rope_Atk_End");
    }

    public void Rope_Atk_End()
    {
        Rope_Time = 0.0f;
        isAttacking = false;
        isEnterRopeAtk = false;
        isLock = false;
    }
    #endregion

    #region Dragon_FindTarget
    public void FindTarget()
    {

    }

    #endregion
}
    