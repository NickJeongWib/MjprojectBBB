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

    public bool wSkillHit;

    //private void Update()
    //{
    //    if(wSkillHit)
    //    {
    //        for(int i = 0; i < transform.childCount; i++)
    //        {
    //            transform.GetChild(i).GetComponent<Collider>().enabled = false;
    //        }
    //    }
    //}


}
