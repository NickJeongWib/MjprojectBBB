using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    private void Start()
    {
        GameManager.GMInstance.Set_SetResolution(this);

        ResolutionSet(); // �ʱ⿡ ���� �ػ� ����
    }

    /* �ػ� �����ϴ� �Լ� */
    public void ResolutionSet()
    {
        int setWidth = GameManager.GMInstance.DisplayWidth; // ����� ���� �ʺ�
        int setHeight = GameManager.GMInstance.DisplayHeight; // ����� ���� ����

        int deviceWidth = UnityEngine.Screen.width; // ��� �ʺ� ����
        int deviceHeight = UnityEngine.Screen.height; // ��� ���� ����

        UnityEngine.Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }
}