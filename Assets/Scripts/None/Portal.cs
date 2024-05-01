using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // ���� ����Ʈ�� ��Ÿ���� ������Ʈ

    private void OnTriggerEnter(Collider other)
    {
        // �� �ڵ�� �۷ι� ��ǥ�� ����Ͽ� ������Ʈ�� ��ġ�� ȸ���� �����մϴ�.
        Transform objectTransform = other.transform;

        other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        // ������Ʈ�� ��ġ�� ȸ���� toObj ������Ʈ�� �۷ι� ��ġ�� ȸ������ �����մϴ�.
        objectTransform.position = toObj.transform.position; // �۷ι� ��ġ ����
        objectTransform.rotation = toObj.transform.rotation; // �۷ι� ȸ�� ����
        other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
}
