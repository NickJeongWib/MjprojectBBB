using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_Controller : Character_BehaviorCtrl_Base
{
    [SerializeField]
    Assassin_ObjPool assassin_ObjPoolRef;
    public Transform firePoint; // �߻� ����
    public float projectileSpeed = 10f; // �߻�ü �ӵ�
    public GameObject skill_Look;
    [SerializeField]
    LayerMask groundLayer; // ������ ���̾�
    public GameObject mouseMoveEffect; // �̵��� ���콺 Ŭ�� ����Ʈ
    public Camera mainCam;
    private Animator animator;
    private Vector3 destination;
    public float moveSpeed;

    private bool isMove;
    private bool isSkill1;

    void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        assassin_ObjPoolRef = GetComponent<Assassin_ObjPool>();
    }

    // Update is called once per frame
    public void Update()
    {
        firePoint.transform.localRotation = Quaternion.identity;

        if (Input.GetMouseButtonDown(0)) //|| skill1 || skill2 || skill3 || skill4
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100)) // ray�� 100�� ���̱��� ������� �浹�� ������ rayHit�� ����
            {
                Vector3 nextVec = rayHit.point - transform.position; // rayHit.point : ray�� �浹�� ����, transform.position : ĳ������ ���� ��ġ
                nextVec.y = 0;                                       // y���� 0���� �Ͽ� ���򼱿��� �ٶ� �� �ֵ��� ��
                transform.LookAt(transform.position + nextVec); // ĳ���Ͱ� ���� ���� ���͸� �ٶ󺸵��� ȸ���մϴ�.
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && hit.collider.CompareTag("Ground"))
            {
                Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
                // �������� �����ϰ� 1�� �Ŀ� �ı�
                GameObject newPrefab = Instantiate(mouseMoveEffect, spawnPosition, Quaternion.identity);

                // ȸ�� ���� �����մϴ�. x ���� 90���� �����մϴ�.
                newPrefab.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

                Destroy(newPrefab, 1.0f);
            }
        }

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit) && hit.collider.CompareTag("Ground"))
            {
                SetDestination(hit.point);
            }
        }

        Move();
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Skill_1();
        }
            
        
    }

    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    public override void Move()
    {
        if(isMove)
        {
            Vector3 dir = destination - transform.position;
            transform.forward = dir;
            transform.position += dir.normalized * Time.deltaTime * moveSpeed;
        }

        if(Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }

    public override void Skill_1()
    {
        if (!isSkill1)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill1 = true;

            animator.SetTrigger("doSkill1");

            //attack_Collider.Q_Use();

            Invoke("SkillOut", 2f);
        }
    }

    public void SkillOut()
    {
        isSkill1 = false;
        
    }

    public void Skill01_Event01()
    {
        StartCoroutine("ShurikenShot");
    }

    public void Skill01_Event02()
    {
        StartCoroutine("ShurikenShot");
    }

    public void Skill01_Event03()
    {
        StartCoroutine("ShurikenShot");
    }
    IEnumerator ShurikenShot()
    {
        GameObject shuriken = assassin_ObjPoolRef.ShurikenFromPool();
        shuriken.transform.position = firePoint.transform.position; // �߻� ��ġ ����
        shuriken.transform.Rotate(0, 0, 90);
        shuriken.SetActive(true); // �߻�ü Ȱ��ȭ


        Vector3 d2 = shuriken.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        shuriken.transform.rotation = q2 * Quaternion.Euler(90f, 180f, 0f);
        Debug.Log("3");
        yield return new WaitForSeconds(0f);
    }

}
 