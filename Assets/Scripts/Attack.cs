using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //���� Ÿ��, ������, ����, ����, ȿ�� ���� ����
    public enum Type { Melee, Range }

    [Header("���� Ÿ��")]
    public Type type;

    [Header("���� ����")]
    public int damage; //������
    public float rate; //���ݼӵ�
    public bool isAttackable;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
