using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ninja_CharacterController : CharacterBase
{
    [Header("Player �̵��ӵ�")]
    public float moveSpeed;
    [Header("Player ����Ŷ�")]
    public float DodgeDistance;

    Rigidbody rigid;
    public Camera cam;
    private Animator animator;
    NavMeshAgent navMeshAgent;

    public bool isMove;
    public bool isDodge;
    public bool isAttack;
    public bool isSkill1;
    public bool isSkill2;
    public bool isSkill3;
    public bool isSkill4;

    public bool spaceDown;

    public bool skill1;
    public bool skill2;
    public bool skill3;
    public bool skill4;

    bool isBorder;

    bool isHit = false;

    private Vector3 movement;
    Vector3 ClickPoint;

    public Skill_Test skillManager1;
    public Skill_Test skillManager2;
    public Skill_Test skillManager3;
    public Skill_Test skillManager4;
    public Skill_Test skillManager5;

    public GameObject MouseMoveEffect;

    public AttackCollider attack_Collider;
    public PlaySceneManager playscenemanager;
    [SerializeField]
    LayerMask obstacleMask;

    public float rayDistance = 5.0f;
    private RaycastHit Wallhit;
    private RaycastHit SkillWallhit;
    [SerializeField]
    Transform WallCheckTrans;
    [SerializeField]
    bool isWall;

    public float MoveSkill_rayDistance = 20.0f;


    void Awake()
    {
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {
        if (!isDodge && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4)
        {
            Move();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        Dodge();

        Attack();

        Skill_1();
        Skill_2();
        Skill_3();
        Skill_4();

        if (isDodge)
        {
            isMove = false;
            isAttack = false;
            isSkill1 = false;
            isSkill2 = false;
            isSkill3 = false;
            isSkill4 = false;
        }



        if (Input.GetMouseButtonDown(0) || skill1 || skill2 || skill3 || skill4)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }

            // Enemy ��ũ��Ʈ�� isAttackable ���� ����
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = true;
            }
        }

        if (isBorder)
        {
            animator.SetBool("isMove", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
                // �������� �����ϰ� 1�� �Ŀ� �ı�
                GameObject newPrefab = Instantiate(MouseMoveEffect, spawnPosition, Quaternion.identity);

                // ȸ�� ���� �����մϴ�. x ���� 90���� �����մϴ�.
                newPrefab.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

                Destroy(newPrefab, 1.0f);
            }
        }
    }

    public override void GetInput()
    {
        spaceDown = Input.GetButtonDown("Dodge");

        skill1 = Input.GetButtonDown("Skill1");
        skill2 = Input.GetButtonDown("Skill2");
        skill3 = Input.GetButtonDown("Skill3");
        skill4 = Input.GetButtonDown("Skill4");
    }


    #region Move
    // TODO ## ���� ĳ���� �̵� �����Լ�
    public override void Move()
    {
        // TODO ## ���� �� ��ֹ� üũ
        // ĳ������ ��ġ�� �������� Ray�� ���ϴ�.
        if (Physics.Raycast(WallCheckTrans.position, WallCheckTrans.forward, out Wallhit, rayDistance) && Wallhit.collider.CompareTag("Wall"))
        {
            // ���̰� �浹�� �������� ���������� �ð�ȭ�մϴ�.
            Debug.DrawRay(WallCheckTrans.position, WallCheckTrans.forward * Wallhit.distance, Color.red);

            isWall = true;

            if (isWall == true)
            {
                moveSpeed = 0.0f;
                DodgeDistance = 0.0f;
                isMove = false;
                animator.SetBool("isMove", false);
            }
        }
        else
        {
            // ���̰� �浹���� �ʰ� �ִ� �Ÿ����� ������ ��� �ʷϻ����� �ð�ȭ�մϴ�.
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);

            isWall = false;

            if (isWall == false)
            {
                moveSpeed = 15.0f;
                DodgeDistance = 20.0f;
            }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SetMove(hit.point);
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Wall"))
            {
                NotMove();
            }

            if (hit.collider.CompareTag("Wall"))
            {
                // ���� ������ ���⿡ ���ϴ� ������ �߰��� �� �ֽ��ϴ�.
                Debug.Log("�̵��� �� ���� �����Դϴ�!");
                return; // ���� ������ �̵����� �ʵ��� ����
            }

            //    Vector3 mousePosition = hit.point;
            //    Vector3 direction = (mousePosition - transform.position).normalized;

            //    ���� ������ ���⼭ ����ڰ� ���ϴ� ��� �����ؾ� �մϴ�.
            //    direction.y = 0;

            //    rigid.MovePosition(rigid.position + direction * moveSpeed * Time.deltaTime);
        }



        if (isMove)
        {
            Vector3 dir = movement - transform.position;
            ClickPoint = dir;

            animator.transform.forward = dir;


            if (!isBorder)
            {
                transform.position += dir.normalized * Time.deltaTime * moveSpeed;
            }
        }

        if (Vector3.Distance(transform.position, movement) <= 2f)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }


    public override void NotMove()
    {
        isMove = false;
        animator.SetBool("isMove", false);
    }

    public override void SetMove(Vector3 _move)
    {
        movement = _move;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    #endregion

    #region Normal_Attack
    // TODO ## ���� �⺻ ����
    public override void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isAttack = true;

            animator.SetTrigger("Attack");

            attack_Collider.A_NoUse();

            Invoke("AAA_Attack", 0.000001f);

            Invoke("AttackOut", 0.8f);

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = false;
            }
        }

    }

    public override void AttackOut()
    {
        isAttack = false;
    }


    public override void AAA_Attack()
    {
        attack_Collider.A_Use();
    }
    #endregion

    #region Dodge
    // TODO ## ���� ����
    public override void Dodge()
    {
        if (spaceDown && !isDodge && skillManager5.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);
            animator.SetTrigger("doDodge");
            isDodge = true;

            attack_Collider.D_Use();

            attack_Collider.FX_Slash_R.enabled = false;
            attack_Collider.R_Shash_FX.enabled = false;
            attack_Collider.R_Shash_FX001.enabled = false;
            attack_Collider.Treak_Weapon.enabled = false;

            SkillOut();

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // ���͸� ����ȭ�մϴ�.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * DodgeDistance;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.7f));
                Invoke("DodgeOut", 0.7f);
            }
        }
    }

    public override void DodgeOut()
    {
        isDodge = false;
    }

    IEnumerator MoveDuring(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            // Dodge ���� �߿� �÷��̾ �����Դϴ�.
            float t = (Time.time - startTime) / duration; // �ð� ���
            transform.position = Vector3.Lerp(startPosition, endPosition, t); // ���� �������� ���� �������� t��ŭ �ð� �ҿ�
            yield return null;
        }

        transform.position = endPosition;
    }
    #endregion

    #region Skill
    // TODO ## Ninja_Q_Skill
    public override void Skill_1()
    {
        if (skill1 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager1.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill1 = true;

            animator.SetTrigger("doSkill1");

            attack_Collider.Q_Use();

            Invoke("SkillOut", 2f);

            // Enemy ��ũ��Ʈ�� isAttackable ���� ����
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = false;
            }
        }
    }

    public void Skill_Q()
    {
        Debug.Log("Q_Skill_1");
    }
    public void Skill_Q_2()
    {
        Debug.Log("Q_Skill_2");
    }

    // TODO ## Ninja_W_Skill
    public override void Skill_2()
    {
        if (skill2 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager2.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill2 = true;

            animator.SetTrigger("doSkill2");

            attack_Collider.W_Use();

            Invoke("SkillOut", 2.3f);

            // Enemy ��ũ��Ʈ�� isAttackable ���� ����
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = false;
            }
        }
    }

    // TODO ## Ninja_E_Skill
    public override void Skill_3()
    {
        if (skill3 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager3.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill3 = true;

            attack_Collider.E_Use();

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // ���͸� ����ȭ�մϴ�.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * 25f;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.6f));
                Invoke("SkillOut", 0.7f);
                animator.SetTrigger("doSkill3");

                // Enemy ��ũ��Ʈ�� isAttackable ���� ����
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    enemy.attack.isAttackable = false;
                }
            }
            /*
            foreach (Transform child in transform)
            {
                child.localRotation = Quaternion.identity;
            }*/
        }
    }

    // TODO ## Ninja_R_Skill
    public override void Skill_4()
    {
        if (skill4 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager4.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill4 = true;

            attack_Collider.R_Use();

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // ���͸� ����ȭ�մϴ�.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * 25f;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.1f));
                animator.SetTrigger("doSkill4");
                Invoke("SkillOut", 1f);

                // Enemy ��ũ��Ʈ�� isAttackable ���� ����
                // Enemy ��ũ��Ʈ�� isAttackable ���� ����
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    enemy.attack.isAttackable = false;
                }
            }
            /*
            foreach (Transform child in transform)
            {
                child.localRotation = Quaternion.identity;
            }*/
        }
    }

    public override void SkillOut()
    {
        isSkill1 = false;
        isSkill2 = false;
        isSkill3 = false;
        isSkill4 = false;
    }

    #endregion

    public override void ResetCollision()
    {
        isHit = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyAttack" && !isHit)
        {
            playscenemanager.HealthDown();
            Debug.Log(other.gameObject.name);

            isHit = true;

            Invoke("ResetCollision", 1f);
        }
    }

}