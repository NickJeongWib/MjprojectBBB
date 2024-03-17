using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    public float moveSpeed = 10f; // �̵� �ӵ�
    public float maxDistance = 30f; // �ִ� �̵� �Ÿ�

    public bool wSkill;
    public bool wSkillDis;

    [SerializeField]
    Assassin_ObjPool assassin_ObjPoolRef;

    void Start()
    {
        
    }

    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!wSkill)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // �߻�ü �̵� �Ÿ� üũ
            if (Vector3.Distance(transform.position, transform.parent.parent.parent.GetChild(1).position) > maxDistance)
            {
                gameObject.SetActive(false); // ���� �Ÿ� �̵� �� ��Ȱ��ȭ
            }
        }
        else
        {
            if (!wSkillDis)
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

                // �߻�ü �̵� �Ÿ� üũ
                if (Vector3.Distance(transform.position, transform.parent.parent.parent.parent.GetChild(1).position) > maxDistance)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

                // �߻�ü �̵� �Ÿ� üũ
                if (Vector3.Distance(transform.position, transform.parent.parent.parent.parent.GetChild(1).position) > maxDistance)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
