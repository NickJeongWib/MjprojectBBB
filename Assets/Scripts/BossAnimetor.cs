using UnityEngine;
using System.Collections;

public class BossAnimator : MonoBehaviour
{
    public Animator animator;   // �ִϸ����� ������Ʈ
    public bool AttRadyState;   // AttRadyState ����

    private void Start()
    {
        // ���� �ð� �ڿ� ���� ������ ���� �ڷ�ƾ ����
        StartCoroutine(ChangeStateAfterDelay(5f));
    }


    IEnumerator ChangeStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���� ���� �ڵ� �ۼ�
        animator.SetBool("doLookPlayer", AttRadyState);
    }

    // �ִϸ��̼��� ó�� ���·� �ǵ����ϴ�.
    public void ResetAnimation()
    {
        // �ִϸ��̼� �Ķ���� �ʱ�ȭ
        animator.SetBool("doLookPlayer", false);

    }

    // 'doLookPlayer' �ִϸ��̼��� ó������ �����մϴ�.
    public void StartAnimation()
    {
        // �ִϸ��̼� �Ķ���� ����
        animator.SetBool("doLookPlayer", true);

        // 'doLookPlayer' �ִϸ��̼� Ŭ���� ó������ ���
        animator.Play("doLookPlayer", -1, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // AttRadyState ���� ���� �ִϸ��̼� ���¸� �����մϴ�.
        if (AttRadyState)
        {
            StartAnimation();
        }
        else
        {
            ResetAnimation();
        }
    }
}