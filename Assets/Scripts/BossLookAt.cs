using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookAt : MonoBehaviour
{
    public GameObject player; // Player ��ü�� �����ϴ� ����
    public BossAnimator bossAnimator; // BossAnimator ��ũ��Ʈ�� �����ϴ� ����
    public AudioSource audioSource; // ��� ������ ����ϱ� ���� ����� �ҽ�
    public AudioClip bgmClip; // ��� ���� Ŭ��

    public BossSkillP bossSkillP; // BossSkillP ��ũ��Ʈ�� �����ϴ� ����

    public bool canUseSkills = false; // ��ų ��� ���� ���θ� �����ϴ� ����
    public bool isLook = true; // ������ �÷��̾ �ٶ󺸴��� ���θ� �����ϴ� ����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Player �±׸� ���� ������Ʈ�� ã�Ƽ� player ������ �Ҵ�
        bossAnimator = GetComponent<BossAnimator>(); // BossAnimator ������Ʈ�� ã�Ƽ� bossAnimator ������ �Ҵ�
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ�� ã�Ƽ� audioSource ������ �Ҵ�

        bossSkillP = GetComponent<BossSkillP>(); // BossSkillP ������Ʈ�� ã�Ƽ� bossSkillP ������ �Ҵ�
        bossSkillP.enabled = false; // BossSkillP ��ũ��Ʈ�� ó������ ��Ȱ��ȭ
    }

    void Update()
    {
        if (player != null && bossAnimator.AttRadyState && isLook)
        {
            Vector3 direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

            if (!audioSource.isPlaying)
            {
                audioSource.clip = bgmClip;
                audioSource.Play();
            }

            bossSkillP.enabled = canUseSkills;
            if (canUseSkills)
            {
                for (int i = 0; i < 4; i++)
                {
                    bossSkillP.UseSkill((BossSkillP.BossSkill)i);
                }
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            canUseSkills = true;
        }
    }
}