using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Collider[] warpPortals; // ���� ��Ż���� �迭
    public Transform[] warpDestinations; // ���� ���� �������� �迭

    public float warpDelay = 0.5f; // ���������� ������ �ð�

    private bool isWarping = false; // ���� ���� ������ ���θ� ��Ÿ���� ����

    private void OnTriggerEnter(Collider other)
    {
        if (!isWarping && CanWarp(other))
        {
            StartCoroutine(WarpPlayer(other.gameObject));
        }
    }

    private bool CanWarp(Collider other)
    {
        // �÷��̾�� �浹�� ��쿡�� ���� �����ϵ��� ����
        return other.CompareTag("Player");
    }

    private IEnumerator WarpPlayer(GameObject player)
    {
        isWarping = true;

        // ���� �ִϸ��̼�, ����Ʈ, ���� ���� �߰��Ͽ� �ð����� �ǵ�� ���� ����

        yield return new WaitForSeconds(warpDelay);

        // ���� ��Ż�� ��ġ�ϴ� �������� ã��
        for (int i = 0; i < warpPortals.Length; i++)
        {
            if (warpPortals[i] == GetComponent<Collider>())
            {
                if (i < warpDestinations.Length)
                {
                    // �÷��̾ ���õ� ���� �������� �̵�
                    player.transform.position = warpDestinations[i].position;
                }
                break;
            }
        }

        isWarping = false;
    }
}