using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D };
    

    public Attack attack;

    [Header("�� ����")]
    public Type enemyType;

    [Header("�� ü��")]
    public int maxHealth;
    public int curHealth;

    [Header("�� ����")]
    public Transform target;

    [Header("���� ����")]
    public BoxCollider meleeArea;
    public GameObject bullet;

    public bool isChase;

    public bool isAttack;

    public bool isDead;

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;

    public Animator anim;

    //NavMesh: NavAgent�� ��θ� �׸��� ���� ����(Mesh)
    //NavMeshsh�� Static ������Ʈ�� Bake ����
    public NavMeshAgent nav;

    public float attackInterval = 2f; // ���� ����
    private Coroutine attackCoroutine; // ���� �ڷ�ƾ ����

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position);

            nav.isStopped = !isChase;
        }

        if (Input.GetMouseButtonDown(0) && attack.isAttackable)
        {
            attackCoroutine = StartCoroutine(AttackWithDelay());
        }
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        if (!isDead && enemyType != Type.C)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;

                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;

                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        if (attack.isAttackable)
        {
            isChase = false;
            isAttack = true;
            anim.SetBool("isAttack", true);

            switch (enemyType)
            {
                case Type.A:
                    yield return new WaitForSeconds(0.2f);

                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f);

                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(1f);
                    break;

                case Type.B:
                    yield return new WaitForSeconds(0.1f);

                    rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.5f);

                    rigid.velocity = Vector3.zero;
                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(2f);
                    break;

                case Type.C:
                    yield return new WaitForSeconds(0.5f);

                    GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                    Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                    rigidBullet.velocity = transform.forward * 20;

                    yield return new WaitForSeconds(2f);
                    break;
            }

            isChase = true;
            isAttack = false;
            anim.SetBool("isAttack", false);
        }
    }
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

            gameObject.layer = 14;

            isDead = true;

            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            Destroy(gameObject, 4);
        }
    }

    IEnumerator AttackWithDelay()
    {
        attack.isAttackable = false; // ���� �Ұ��� ���·� ����

        yield return new WaitForSeconds(attackInterval); // ���� ���ݸ�ŭ ���

        // ���� �ִϸ��̼� ��� �� ���� ���� ����

        attack.isAttackable = true; // ���� ���� ���·� �ٽ� ����
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee") && other.gameObject.CompareTag("Enemy") && attack.isAttackable)
        {
            Attack weapon = other.GetComponent<Attack>();
            if (weapon != null)
            {
                int weaponDamage = weapon.damage; // ������ ���ݷ� ���� �����ɴϴ�.
                curHealth -= weaponDamage; // ü���� ������ ���ݷ¸�ŭ ���ҽ�ŵ�ϴ�.
                curHealth = Mathf.Max(curHealth, 0); // ü���� 0 �̸����� �������� �ʵ��� �����մϴ�.

                Vector3 reactVec = transform.position - other.transform.position;

                StartCoroutine(OnDamage(reactVec, false));

                attack.isAttackable = false; // ���� �ð� ���� ���� �Ұ��� ���·� ����
                StartCoroutine(ResetAttackableState());
            }
        }
    }

    IEnumerator ResetAttackableState()
    {
        yield return new WaitForSeconds(attackInterval); // ���� ���ݸ�ŭ ���

        attack.isAttackable = true; // ���� ���� ���·� �ٽ� ����
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }
}
