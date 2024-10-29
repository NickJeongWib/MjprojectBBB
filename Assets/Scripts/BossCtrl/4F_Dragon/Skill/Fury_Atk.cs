using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fury_Atk : MonoBehaviour
{
    [SerializeField]
    GameObject Target;

    Vector3 dir;

    [SerializeField]
    float Fury_Rot_Speed;
    [SerializeField]
    float Fury_Move_Speed;

    [SerializeField]
    float Spawn_MaxTime;
    [SerializeField]
    float Current_SpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindWithTag("Player");
        this.gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        LookAtPlayer();
        Move();
    }

    private void OnEnable()
    {
        StartCoroutine(VisibleTime());
    }

    private void OnDisable()
    {
        Current_SpawnTime = 0.0f;
    }

    // ��ų ����Ʈ ���� �ð�
    IEnumerator VisibleTime()
    {
        while (Current_SpawnTime <= Spawn_MaxTime)
        {
            Current_SpawnTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.SetActive(false);
    }

    #region Rotation
    public void LookAtPlayer()
    {
        dir = Target.transform.position - transform.position;
        // y�� ���� ����
        dir.y = 0.0f;

        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Fury_Rot_Speed);
    }
    #endregion

    #region Move
    public void Move()
    {
        // �÷��̾ ã�� �� ���ٸ� ���� ����
        if (Target == null)
            return;

        transform.Translate(Vector3.forward * Fury_Move_Speed * Time.deltaTime);
    }
    #endregion




}
