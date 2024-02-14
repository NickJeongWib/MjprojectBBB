using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerMaker_Ctrl1 : MonoBehaviour
{
    public bool isRazerAtk;

    // Update is called once per frame
    void Update()
    {
        // ������ �߻� ������Ʈ�� �ö������ ����
        if (this.transform.position.y < 0.0f && isRazerAtk == false)
        {
            this.transform.Translate(Vector3.up * 10.0f * Time.deltaTime, Space.World);
        }

        // ������ �߻� �� ������Ʈ�� ���������� ����
        if (isRazerAtk == true && this.transform.position.y > -10.0f)
        {
            this.transform.Translate(Vector3.down * 10.0f * Time.deltaTime, Space.World);
        }
    }
}
