using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySettingManager : MonoBehaviour
{
    public TextMeshProUGUI[] keyCodeName;

    public GameObject keySettingImage;
    public GameObject keySettingFailImage;

    private void Start()
    {
        for (int i = 0; i < keyCodeName.Length; i++)
        {
            keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        }
    }

    void Update()
    {
        for (int i = 0; i < keyCodeName.Length; i++)
        {
            keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        }

        if (keySettingImage.activeSelf)
        {
            // Ű������ ��� Ű�� ���� Ȯ���մϴ�.
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                // ���콺 ��ư�� �ƴ� ��쿡�� ó���մϴ�.
                if (!IsMouseButton(keyCode) && Input.GetKeyDown(keyCode))
                {
                    // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
                    keySettingImage.SetActive(false);
                    break; // Ű �Է��� �����Ǿ����Ƿ� ������ �����մϴ�.
                }
            }
        }

        if (keySettingFailImage.activeSelf && Input.GetMouseButtonDown(0))
        {
            // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            keySettingFailImage.SetActive(false);
        }
    }

    // �Էµ� Ű�� ���콺 ��ư���� Ȯ���մϴ�.
    private bool IsMouseButton(KeyCode keyCode)
    {
        return keyCode >= KeyCode.Mouse0 && keyCode <= KeyCode.Mouse6;
    }

    public void OpenToKeySetting(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

}
