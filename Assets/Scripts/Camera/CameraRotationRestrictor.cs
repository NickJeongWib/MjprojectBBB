using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRotationRestrictor : MonoBehaviour
{
    // ī�޶��� ȸ���� ������� ����
    public bool allowRotation = false;

    private void LateUpdate()
    {
        // ī�޶��� ȸ���� ������� ������ ī�޶��� ȸ���� �ʱ� ���·� ����
        if (!allowRotation)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
