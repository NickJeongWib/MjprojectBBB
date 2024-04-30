using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // ���� ����Ʈ�� ��Ÿ���� ������Ʈ

    private void OnTriggerEnter(Collider other)
    {
        // �� �ڵ�� �۷ι� ��ǥ�� ����Ͽ� ������Ʈ�� ��ġ�� ȸ���� �����մϴ�.
        Transform objectTransform = other.transform;

        // ������Ʈ�� ��ġ�� ȸ���� toObj ������Ʈ�� �۷ι� ��ġ�� ȸ������ �����մϴ�.
        objectTransform.position = toObj.transform.position; // �۷ι� ��ġ ����
        objectTransform.rotation = toObj.transform.rotation; // �۷ι� ȸ�� ����
    }
}
