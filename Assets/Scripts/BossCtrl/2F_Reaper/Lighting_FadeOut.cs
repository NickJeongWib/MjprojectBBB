using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting_FadeOut : MonoBehaviour
{
    public float fadeSpeed = 0.5f; // ������� �ӵ�

    void Update()
    {
        // ���� �������� ������
        Vector3 currentScale = transform.localScale;

        // ������ �������� ����
        float newScale = Mathf.Clamp01(currentScale.x - fadeSpeed * Time.deltaTime);
        Vector3 newScaleVector = new Vector3(newScale, newScale, newScale);

        // ����� �������� ����
        transform.localScale = newScaleVector;

        // �������� 0�� �����ϸ� ������Ʈ�� ��Ȱ��ȭ�Ͽ� ������ ������� ��
        if (newScale <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
