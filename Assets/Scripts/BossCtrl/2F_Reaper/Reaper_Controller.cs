using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    public GameObject Target;   // �÷��̾�
    Vector3 dir; // ����

    Rigidbody rigid;

    [SerializeField]
    float Boss_RotSpeed;
    [SerializeField]
    float moveSpeed;
    public bool isLock;               // ���� ���� ����
    public bool isAttacking;          // ���� �� ���� ����

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // �ִϸ�����

    public bool isChase;
    public bool isMove;         // �̵� ����
    public bool isTargetFind;   // ù ���� ����

    public override void LookAtPlayer()
    {
        // �÷��̾ ã�� �� ���ٸ� ���� ����
        if (Target == null)
            return;

        // �÷��̾ �ٶ󺸵���
        // this.transform.LookAt(Target.transform);

        dir = Target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Boss_RotSpeed);
    }

    public override void Move()
    {
        // �÷��̾ ã�� �� ���ٸ� ���� ����
        if (Target == null)
            return;

        Reaper_animator.SetBool("isMove", isMove);

        if (isMove)
        {
            // ������ �̵�
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reaper_animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        // Rigidbody�� ȸ���� ����
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLock == false)
        {
            LookAtPlayer();
        }
    }

    #region Animation Event
    public void FindAnim_To_Idle()
    {
        // Ÿ�� ã������ false
        isTargetFind = false;
        // Idle ������� ����
        Reaper_animator.SetBool("isFind", isTargetFind);

        isMove = true;
    }
    
    

    // �߰� ����
    public void Target_Chase()
    {
        if (isChase)
        {
            isMove = true;
            Reaper_animator.SetBool("isMove", isMove);
        }
    }
    #endregion

    #region Boss_BaseAtk
    // ���� ȸ�� ����
    public void AnimRotate_Lock()
    {
        isLock = true;
        isAttacking = true;

        StartCoroutine(AnimRotate_UnLock());
    }

    // ���� ȸ��
    IEnumerator AnimRotate_UnLock()
    {
        yield return new WaitForSeconds(2.5f);
        isLock = false;
        isAttacking = false;
    }
    #endregion
}
