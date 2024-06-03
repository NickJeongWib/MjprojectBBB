using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorOpen : MonoBehaviour
{
    public float openSpeed = 5f; // �� ���� �ӵ�
    public float closeSpeed = 5f; // �� ���� �ӵ�
    public float openHeight = 100f; // ���� ���� �� �̵��� ����

    private bool isOpening = false; // ���� ������ �ִ��� Ȯ��
    private bool isClosing = false; // ���� ������ �ִ��� Ȯ��
    private Vector3 closedPosition; // �� ���� ��ġ
    private Vector3 openPosition; // �� ���� ��ġ

    [SerializeField]


    void Start()
    {
        // �ʱ� �� ��ġ ����
        closedPosition = transform.position;
        // �� ���� ��ġ ��� (���� ��ġ���� y������ openHeight��ŭ �̵�)
        openPosition = new Vector3(closedPosition.x, closedPosition.y + openHeight, closedPosition.z);
    }

    void Update()
    {
        if (isOpening)
        {
            // �� ����
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
            if (transform.position == openPosition)
            {
                isOpening = false; // ���� ������ ������ ���� ���� ����
            }
        }
        else if (isClosing)
        {
            // �� ����
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, closeSpeed * Time.deltaTime);
            if (transform.position == closedPosition)
            {
                isClosing = false; // ���� ������ ������ ���� ���� ����
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.DoorOpening);

            isOpening = true; // �� ���� ���·� ����
            isClosing = false; // �� ���� ���� ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (openHeight == 100) {
            if (other.CompareTag("Player"))
            {
                GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.DoorClosing);
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<NavMeshObstacle>().enabled = true;
                isClosing = true; // �� ���� ���·� ����
                isOpening = false; // �� ���� ���� ����
            }
        }
    }
}
