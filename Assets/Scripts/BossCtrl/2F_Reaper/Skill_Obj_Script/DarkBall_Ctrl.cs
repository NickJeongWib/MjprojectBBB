using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DarkBall_Ctrl : MonoBehaviour
{
    public Reaper_Pattern_Color ballColor;
    public GameObject Target;
    [SerializeField]
    float DarkBall_RotSpeed;
    [SerializeField]
    float DarkBall_Speed;
    [SerializeField]
    float ExplosionTime;

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

    private void OnEnable()
    {
        if (!this.CompareTag("CountAtk"))
        {
            // Ÿ�� �˻�
            Target = GameObject.FindWithTag("Player");
        }
        StartCoroutine(Explosion_Ball());
    }

    public void DarkBall_Move()
    {
        // ������ �̵�
        transform.Translate(Vector3.forward * DarkBall_Speed * Time.deltaTime);
    }

    IEnumerator Explosion_Ball()
    {
        yield return new WaitForSeconds(ExplosionTime);
        // ExplosionTime �� �� ����
        this.gameObject.SetActive(false);
    }
}
