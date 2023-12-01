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

    public Vector3 direction;

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
            direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

            if (!audioSource.isPlaying)
            {
                audioSource.clip = bgmClip;
                audioSource.Play();
            }

            if (canUseSkills)
            {
                bossSkillP.enabled = true; // ��ų ��� ������ �� BossSkillP ��ũ��Ʈ Ȱ��ȭ
                for (int i = 0; i < 5; i++)
                {
                    bossSkillP.UseSkill((BossSkillP.BossSkill)i);
                }
            }
            else
            {
                bossSkillP.enabled = false; // ��ų ��� �Ұ����� �� BossSkillP ��ũ��Ʈ ��Ȱ��ȭ
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            canUseSkills = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            canUseSkills = false;
        }
    }
}