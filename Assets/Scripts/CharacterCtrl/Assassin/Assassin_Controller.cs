using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_Controller : Character_BehaviorCtrl_Base
{
    [SerializeField]
    LayerMask groundLayer; // ������ ���̾�
    public GameObject mouseMoveEffect; // �̵��� ���콺 Ŭ�� ����Ʈ
    public Camera mainCam;
    private Animator animator;
    private Vector3 destination;
    public float moveSpeed;

    private bool isMove;

    void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
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
}
 