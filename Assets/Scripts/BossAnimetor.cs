using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    public bool AttRadyState;

    // ������ ���¸� ��Ÿ���� Enum
    public enum BossState
    {
        Idle,  // ������
        Idle1_2 // �߰��� ����
    }

    public Animator animator;   // �ִϸ����� ������Ʈ

    private BossState currentState; // ���� ������ ����

    private void Awake()
    {
        currentState = BossState.Idle;
        animator.Play("Idle");
    }
    // ������ �� �ʱ� ���¸� �����մϴ�.
    private void Start()
    {

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

    // AttRadyState�� true�� �� ȣ��Ǵ� �Լ�
    public void ChangeToIdle1_2State()
    {
        if (currentState == BossState.Idle1_2)
            return;

        currentState = BossState.Idle1_2;

        // �ִϸ����� ������Ʈ�� �Ķ���͸� �����ϰ� �ִϸ��̼� ���� ��ȯ�� �մϴ�.
        animator.SetBool("IsAttReady", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (AttRadyState)
            ChangeToIdle1_2State();
        else
            ChangeToIdleState();
    }
}