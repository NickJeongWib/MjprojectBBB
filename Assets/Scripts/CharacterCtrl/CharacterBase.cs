using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    // ĳ���� �̵�
    public abstract void Move();

    // ĳ���� ����
    public abstract void Dodge();

    public abstract void DodgeOut();

    // ĳ���� ����
    public abstract void Attack();

    // ĳ���� ��ų_1
    public abstract void Skill_1();
    // ĳ���� ��ų_2
    public abstract void Skill_2();
    // ĳ���� ��ų_3
    public abstract void Skill_3();
    // ĳ���� ��ų_4
    public abstract void Skill_4();

    // �Է°�
    public abstract void GetInput();

    public abstract void SetMove(Vector3 move);

    public abstract void NotMove();

    public abstract void SkillOut();

    public abstract void AttackOut();

    public abstract void AAA_Attack();

    public abstract void ResetCollision();
}
