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
    NONE,
    ICE_IDLE,
    CHANGE_FORM,
    ICE_MOVE,

    END
}

public enum ThunderDragon_State
{
    NONE,
    THUNDER_IDLE,
    CHANGE_FORM,
    THUNDER_MOVE,
    END
}

public enum FireDragon_State
{
    NONE,
    FIRE_IDLE,
    FIRE_MOVE,
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
    public GameObject Target;       // �÷��̾�
    public float TargetDistance;    // �÷��̾���� �Ÿ�
    [SerializeField]
    int NextSkillNum;
    [SerializeField]
    float Slow_RotSpeed;

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

        // ���� �ൿ ����
        if (CurrentElement == CurentElement_State.THUNDER_DRAGON)
        {
            // �������� ���� ���� ����
            ThunderDragon_State randomThunderState = (ThunderDragon_State)Random.Range(3, (int)ThunderDragon_State.END - 1);
            // Treant_Normal_State randomNormalState = (Treant_Normal_State)6;
            ThunderDragonState = randomThunderState;

            //Debug.Log(randomNormalState);
        }
        else if (CurrentElement == CurentElement_State.ICE_DRAGON)
        {
            // �������� ���� ���� ����
            IceDragon_State randomIceState = (IceDragon_State)Random.Range(3, (int)IceDragon_State.END - 1);
            // Treant_Power_State randomPowerState = (Treant_Power_State)5;
            IceDragonState = randomIceState;

            //Debug.Log(randomPowerState);

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
            if (CurrentElement == CurentElement_State.THUNDER_DRAGON)
            {
                ThunderDragonState = ThunderDragon_State.THUNDER_MOVE;
            }
            else if (CurrentElement == CurentElement_State.ICE_DRAGON)
            {
                IceDragonState = IceDragon_State.ICE_MOVE;
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
                    default:
                        break;
                }
                break;

            default:
                break;
        }
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

    #region Dragon_FindTarget
    public void FindTarget()
    {

    }

    #endregion
}
