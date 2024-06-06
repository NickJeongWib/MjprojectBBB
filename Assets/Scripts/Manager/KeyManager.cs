using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KeyAction { Skill1, Skill2, Skill3, Skill4, Dodge, KeyCount }

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>();
}

public class KeyManager : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.Space };
    public KeySettingManager keySettingManager;

    private void Awake()
    {
        for (int i = 0; i < (int)KeyAction.KeyCount; i++)
        {
            if (!KeySetting.Keys.ContainsKey((KeyAction)i)) // �ߺ��� Ű�� �ƴ϶�� �߰�
            {
                KeySetting.Keys.Add((KeyAction)i, defaultKeys[i]);
            }
        }
    }

    public void OnGUI()
    {
        Event keyEvent = Event.current;

        if (keyEvent.isKey && key != -1)
        {
            KeyAction keyAction = (KeyAction)key;

            // �ߺ��� Ű�� ������ �߰�
            if (!KeySetting.Keys.ContainsValue(keyEvent.keyCode))
            {
                KeySetting.Keys[keyAction] = keyEvent.keyCode;
            }
            else
            {
                keySettingManager.keySettingFailImage.SetActive(true);
                
            }

            key = -1;
        }
    }

    int key = -1;

    public void ChangeKey(int num)
    {
        key = num;
    }

    public void ResetToDefaultKeys()
    {
        for (int i = 0; i < (int)KeyAction.KeyCount; i++)
        {
            KeySetting.Keys[(KeyAction)i] = defaultKeys[i];
        }
    }
}