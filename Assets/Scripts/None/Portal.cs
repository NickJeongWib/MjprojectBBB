using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // ���� ����Ʈ ������Ʈ

    private void OnTriggerEnter(Collider other)
    {
        // Collider�� �ڽ� ������Ʈ�� ��� �θ� ã�Ƽ� �̵���Ų��
        Transform parentTransform = null;

        // �±װ� WarpPlayer�� ������Ʈ �Ǵ� �θ� ������Ʈ�� ã�´�
        if (other.CompareTag("WarpPlayer"))
        {
            parentTransform = other.transform;
        }
        else if (other.transform.parent != null && other.transform.parent.CompareTag("WarpPlayer"))
        {
            parentTransform = other.transform.parent;
        }

        if (parentTransform != null)
        {
            // �θ� ������Ʈ(�Ǵ� ����)�� ��ġ�� ���� ����Ʈ ��ġ�� �̵�
            parentTransform.position = toObj.transform.position;

            // ��������� ��Ż ȿ���� ��� ������ �����ؾ� �� ��� ���⿡ �߰�
            //audioSource.PlayOneShot(portalSound, 1.0f); // ��Ż ȿ���� ��� �ڵ�
        }
    }
}
