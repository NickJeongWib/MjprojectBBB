using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZero : MonoBehaviour
{
    void Update()
    {
        // rotation�� �׻� 0���� ����
        transform.rotation = Quaternion.identity;
    }
}
