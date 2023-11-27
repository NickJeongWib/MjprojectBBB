using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookAt : MonoBehaviour
{
    public GameObject player; // Player ��ü�� �����ϴ� ����
    public BossAnimator bossAnimator; // BossAnimator ��ũ��Ʈ�� �����ϴ� ����
    public AudioSource audioSource; // ��� ������ ����ϱ� ���� ����� �ҽ�
    public AudioClip bgmClip; // ��� ���� Ŭ��

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Player �±׸� ���� ������Ʈ�� ã�Ƽ� player ������ �Ҵ�
        bossAnimator = GetComponent<BossAnimator>(); // BossAnimator ������Ʈ�� ã�Ƽ� bossAnimator ������ �Ҵ�
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ�� ã�Ƽ� audioSource ������ �Ҵ�
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && bossAnimator.AttRadyState) // player�� null�� �ƴϰ�, AttRadyState�� true�� ���
        {
            Vector3 direction = player.transform.position - transform.position; // player�� ���� ������Ʈ ������ ���� ���� ���
            Quaternion rotation = Quaternion.LookRotation(direction); // ���� ���͸� �������� ȸ�� ���� ���
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2); // �ε巴�� ȸ��

            // ��� ���� ���
            if (!audioSource.isPlaying) // ����� �ҽ��� ���� ��� ���� �ƴ϶��
            {
                audioSource.clip = bgmClip; // ��� ���� Ŭ���� ����� �ҽ��� Ŭ������ ����
                audioSource.Play(); // ����� �ҽ� ���
            }
        }
        else if (audioSource.isPlaying) // AttRadyState�� false�̰�, ����� �ҽ��� ���� ��� ���̶��
        {
            audioSource.Stop(); // ����� �ҽ� ��� ����
        }
    }
}