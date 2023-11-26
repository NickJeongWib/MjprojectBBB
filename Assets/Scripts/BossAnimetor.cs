using UnityEngine;

public class BossAnimator : MonoBehaviour
{

    public bool AttRadyState;

    // ������ ���¸� ��Ÿ���� Enum
    public enum BossState
    {
        Idle  // ������
    }

    public Animator animator;   // �ִϸ����� ������Ʈ

    private BossState currentState; // ���� ������ ����

    // ������ �� �ʱ� ���¸� �����մϴ�.
    private void Start()
    {
        currentState = BossState.Idle;
        animator.Play("Idle");
    }

    // ��� ���·� �����ϰ� �ش� ���¿� �´� �ִϸ��̼��� ����մϴ�.
    public void ChangeToIdleState()
    {
        if (currentState == BossState.Idle)
            return;

        currentState = BossState.Idle;

        // �ִϸ����� ������Ʈ�� �ִϸ��̼� Ŭ���� �����ϰ� ����մϴ�.
        animator.Play("Idle");
    }
}