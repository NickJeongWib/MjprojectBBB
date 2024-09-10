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
    ICE_CLOSE_NORMAL_ATK,       // 7
    END
}

public enum ThunderDragon_State
{
    NONE,
    THUNDER_IDLE,
    CHANGE_FORM,
    THUNDER_MOVE,
    THUNDER_FLY_NORMAL_ATK,
    THUNDER_WIND_ATK,
    THUNDER_DASH_ATK,
    THUNDER_CLOSE_NORMAL_ATK,
    END
}

public enum FireDragon_State
{
    NONE,
    FIRE_IDLE,
    FIRE_MOVE,
    FIRE_FLY_NORMAL_ATK,
    FIRE_WIND_ATK,
    FIRE_DASH_ATK,
    FIRE_CLOSE_NORMAL_ATK,
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

        CurrentElement = CurentElement_State.ICE_DRAGON;

        IceDragonState = IceDragon_State.ICE_IDLE;
        ThunderDragonState = ThunderDragon_State.NONE;
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
            IceDragon_State randomIceState = (IceDragon_State)Random.Range(5, (int)IceDragon_State.END - 1);
            // IceDragon_State randomIceState = (IceDragon_State)4;
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
                // IceDragon_State randomIceState = (IceDragon_State)Random.Range(3, 6);
                IceDragon_State randomIceState = (IceDragon_State)6;
                IceDragonState = randomIceState;
            }
            else if(CurrentElement == CurentElement_State.THUNDER_DRAGON)
            {
                
            }
           
            else
            {
                FireDragonState = FireDragon_State.FIRE_MOVE;
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
        DragonPos.localPosition = new Vector3(Target.transform.position.x, Target.transform.position.y, 0.0f);
    }

    public void Dash_Atk_Pos()
    {
        DragonPos.localPosition = Dash_StartPos[Random.Range(0, Dash_StartPos.Length)].transform.localPosition;
    }
    #endregion

    #region Dragon_FindTarget
    public void FindTarget()
    {

    }

    #endregion
}
