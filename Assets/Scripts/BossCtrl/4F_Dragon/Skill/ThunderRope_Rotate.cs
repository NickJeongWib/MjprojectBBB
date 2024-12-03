using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRope_Rotate : MonoBehaviour
{
    Vector3 currentVector = Vector3.back;
    [SerializeField]
    float RotSpeed;

    // Update is called once per frame
    void Update()
    {
        // ���� �� �������� ȸ��
        transform.Rotate(currentVector, RotSpeed * Time.deltaTime, Space.Self);
    }
}
