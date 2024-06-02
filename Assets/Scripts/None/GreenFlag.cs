using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFlag : MonoBehaviour
{ 
    void Start()
    {
        // ��� ������Ʈ�� Renderer ������Ʈ�� ã���ϴ�.
        Renderer flagRenderer = GetComponent<Renderer>();

        // ���� ������� ������ �����մϴ�. RGB ������ ���� ����� ������ �� �ֽ��ϴ�.
        // ���� ���, ���� ����� RGB ���� (0, 100, 0)�� �� �� �ֽ��ϴ�.
        Color darkGreen = new Color(0f, 0.392f, 0f);

        // ������Ʈ�� ��Ƽ���� ������ ���� ������� �����մϴ�.
        flagRenderer.material.color = darkGreen;
    }
}
