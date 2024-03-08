using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WarpDestination
{
    public Collider warpPortal; // ���� ��Ż
    public Transform warpDestination; // ���� ���� ����
}

public class Warp : MonoBehaviour
{
    public WarpDestination[] warpDestinations; // ���� ���� �������� �迭
    public float warpDelay = 0.5f; // ���������� ������ �ð�
    public bool isWarping = false; // ���� ���� ������ ���θ� ��Ÿ���� ����

    public void OnTriggerEnter(Collider other)
    {
        if (!isWarping && CanWarp(other))
        {
            StartCoroutine(WarpPlayer(other.gameObject, other));
        }
    }

    public bool CanWarp(Collider other)
    {
        // �÷��̾�� �浹�� ��쿡�� ���� �����ϵ��� ����
        return other.CompareTag("Player");
    }

    public IEnumerator WarpPlayer(GameObject Charaacter, Collider portal)
    {
        isWarping = true;

        // ���� �ִϸ��̼�, ����Ʈ, ���� ���� �߰��Ͽ� �ð����� �ǵ�� ���� ����

        yield return new WaitForSeconds(warpDelay);

        // ���� ��Ż�� ��ġ�ϴ� �������� ã��
        foreach (var warpDestination in warpDestinations)
        {
            if (warpDestination.warpPortal == portal)
            {
                // �÷��̾ ���õ� ���� �������� �̵�
                Charaacter.transform.position = warpDestination.warpDestination.position;
                break;
            }
        }

        isWarping = false;
    }
}
