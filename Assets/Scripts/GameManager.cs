using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject deathMenu; // ��� �޴� UI ������Ʈ
    public RectTransform deathMenuRectTransform; // ��� �޴��� RectTransform
    private float currentWidth = 0f; // ���� Width ���� ������ ����

    public AudioClip deathSound; // ��� ȿ����
    public List<AudioSource> allAudioSources; // ��� ����� �ҽ�

    public int health;
    //public Player player;

    public Image[] UIhealth;

    public AudioSource hitAudioSource; // �ǰ����� ����� AudioSource
    public AudioSource deathAudioSource; // ������� ����� AudioSource

    public AudioClip hitSound; // �ǰ���

    public BossAnimator bossAnimator; // BossAnimator�� ����

    private void Awake()
    {
        deathMenu.SetActive(false);
        AudioSource[] sources = FindObjectsOfType<AudioSource>(); // ��� ����� �ҽ��� ã�� �迭�� �߰�
        allAudioSources = new List<AudioSource>(); // ����Ʈ �ʱ�ȭ

        foreach (AudioSource src in sources) // �迭�� �ִ� ����� �ҽ��� ��ȸ
        {
            if (src != deathAudioSource && src != hitAudioSource) // ������� �ǰ����� ����ϴ� ����� �ҽ��� �ƴ� ���
            {
                allAudioSources.Add(src); // ����Ʈ�� �߰�
            }
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Ÿ�ӽ������� ������� ����
        SceneManager.LoadScene("Main"); // "MainMenu"��� �̸��� Scene�� �ε�
    }

    void Update()
    {
        if (deathMenu.activeSelf) // ��� �޴��� Ȱ��ȭ�� ���
        {
            currentWidth = Mathf.Lerp(currentWidth, 650, Time.unscaledDeltaTime * 0.02f); // �������� �Լ��� �̿��� Width ���� õõ�� ������Ŵ
            deathMenuRectTransform.sizeDelta = new Vector2(currentWidth, deathMenuRectTransform.sizeDelta.y); // ���� ���� Width ������ ��� �޴��� Width�� ������Ʈ
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;

            UIhealth[health].color = new Color(1, 0, 0, 0.01f);

            // �ǰ��� ���
            hitAudioSource.PlayOneShot(hitSound, 1.0f); // �ǰ��� ��� (������ 1.0)
        }


        else
        {
            //bossAnimator.AttRadyState = false; // �÷��̾ ����Ͽ����Ƿ� AttReadyState�� false�� ����

            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.01f);

            //Player Die Effect
            Time.timeScale = 0; // Ÿ�ӽ������� 0���� ����
            deathMenu.SetActive(true); // ��� �޴��� Ȱ��ȭ

            AudioSource[] sources = FindObjectsOfType<AudioSource>(); // ��� ����� �ҽ��� ã�� �迭�� �߰�
            allAudioSources.Clear(); // ����Ʈ�� ���ϴ�.

            foreach (AudioSource src in sources) // �迭�� �ִ� ����� �ҽ��� ��ȸ
            {
                if (src != deathAudioSource && src != hitAudioSource) // ������� �ǰ����� ����ϴ� ����� �ҽ��� �ƴ� ���
                {
                    allAudioSources.Add(src); // ����Ʈ�� �߰�
                }
            }

            foreach (AudioSource audioSource in allAudioSources) // ��� ����� �ҽ��� ��ȸ
            {
                audioSource.volume = 0; // ����� �ҽ��� ������ 0���� ����
                audioSource.clip = null; // ����� �ҽ��� Ŭ���� ��Ȱ��ȭ
                audioSource.Stop();
            }

            GameObject pObject = GameObject.FindGameObjectWithTag("P"); // P�±׸� ���� ������Ʈ�� ã���ϴ�.
            if (pObject != null) // P�±׸� ���� ������Ʈ�� �����ϸ�
            {
                BoxCollider boxCollider = pObject.GetComponent<BoxCollider>(); // �ش� ������Ʈ�� BoxCollider ������Ʈ�� ã���ϴ�.
                if (boxCollider != null) // BoxCollider ������Ʈ�� �����ϸ�
                {
                    boxCollider.enabled = false; // �ش� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
                }
            }

            deathAudioSource.PlayOneShot(deathSound, 1.0f); // ��� ȿ���� ���

            //Result UI
            Debug.Log("�׾����ϴ�!");
        }

    }
}
