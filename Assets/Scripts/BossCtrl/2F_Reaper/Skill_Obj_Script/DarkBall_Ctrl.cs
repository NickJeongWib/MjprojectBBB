using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall_Ctrl : MonoBehaviour
{
    public GameObject Target;
    [SerializeField]
    float DarkBall_RotSpeed;
    [SerializeField]
    float DarkBall_Speed;

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        DarkBall_Move();
    }

    public void LookAtPlayer()
    {
        // �÷��̾ �ٶ󺸵���
        // this.transform.LookAt(Target.transform);

        Vector3 dir;

        dir = Target.transform.position - transform.position;
        // y�� ���� ����

        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DarkBall_RotSpeed);
    }

    public void DarkBall_Move()
    {
        // ������ �̵�
        transform.Translate(Vector3.forward * DarkBall_Speed * Time.deltaTime);
    }
}
