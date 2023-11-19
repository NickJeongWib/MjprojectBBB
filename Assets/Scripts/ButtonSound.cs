using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    public Button[] buttons; // ���带 ����� ��ư��
    public AudioSource audioSource; // ���带 ����� AudioSource
    public AudioClip[] clickSounds; // �� ��ư�� ���� Ŭ�� �����
    public AudioClip[] releaseSounds; // �� ��ư�� ���� ������ �����

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; // ��ư �ε����� �����Ͽ� Ŭ������ ���

            // ��ư PointerDown �̺�Ʈ�� �ش� ��ư�� ���� ��� �Լ��� �����մϴ�.
            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((eventData) => PlayClickSound(buttonIndex));
            buttons[i].gameObject.AddComponent<EventTrigger>().triggers.Add(pointerDownEntry);

            // ��ư PointerUp �̺�Ʈ�� �ش� ��ư�� ���� ��� �Լ��� �����մϴ�.
            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((eventData) => PlayReleaseSound(buttonIndex));
            buttons[i].gameObject.GetComponent<EventTrigger>().triggers.Add(pointerUpEntry);
        }
    }

    // �ش� ��ư Ŭ�� �� ���带 ����ϴ� �Լ�
    private void PlayClickSound(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= clickSounds.Length)
            return;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clickSounds[buttonIndex]);
        }
    }

    // �ش� ��ư ������ �� ���带 ����ϴ� �Լ�
    private void PlayReleaseSound(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= releaseSounds.Length)
            return;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(releaseSounds[buttonIndex]);
        }
    }
}