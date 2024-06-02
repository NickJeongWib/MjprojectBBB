using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenBossClear : MonoBehaviour
{
    public float openSpeed = 5f; // �� ���� �ӵ�
    public float openHeight = 100f; // ���� ���� �� �̵��� ����
    public Boss_HP_Controller bossHPController; // ���� HP ��Ʈ�ѷ� ����

    private bool isOpening = false; // ���� ������ �ִ��� Ȯ��
    private Vector3 closedPosition; // �� ���� ��ġ
    private Vector3 openPosition; // �� ���� ��ġ

    void Start()
    {
        // �ʱ� �� ��ġ ����
        closedPosition = transform.position;
        // �� ���� ��ġ ��� (���� ��ġ���� y������ openHeight��ŭ �̵�)
        openPosition = new Vector3(closedPosition.x, closedPosition.y + openHeight, closedPosition.z);
    }

    void Update()
    {
        // ������ ����ߴ��� Ȯ��
        if (bossHPController.isDead && !isOpening)
        {
            isOpening = true; // �� ���� ���·� ����
        }

        if (isOpening)
        {
            // �� ����
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
            if (transform.position == openPosition)
            {
                isOpening = false; // ���� ������ ������ ���� ���� ����
            }
        }
    }
}
