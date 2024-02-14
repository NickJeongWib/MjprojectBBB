
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookAt : MonoBehaviour
{

    public GameObject bossHPCanvas;

    public GameObject player; // Player ��ü�� �����ϴ� ����
    public BossAnimator bossAnimator; // BossAnimator ��ũ��Ʈ�� �����ϴ� ����
    public AudioSource audioSource; // ��� ������ ����ϱ� ���� ����� �ҽ�
    public AudioClip bgmClip; // ��� ���� Ŭ��

    public BossSkillP bossSkillP; // BossSkillP ��ũ��Ʈ�� �����ϴ� ����

    public bool canUseSkills = false; // ��ų ��� ���� ���θ� �����ϴ� ����
    public bool isLook = true; // ������ �÷��̾ �ٶ󺸴��� ���θ� �����ϴ� ����
    public float Boss_RotSpeed;

    public Vector3 direction;
    public bool isCheck = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Player �±׸� ���� ������Ʈ�� ã�Ƽ� player ������ �Ҵ�
        bossAnimator = GetComponent<BossAnimator>(); // BossAnimator ������Ʈ�� ã�Ƽ� bossAnimator ������ �Ҵ�
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ�� ã�Ƽ� audioSource ������ �Ҵ�

        bossSkillP = GetComponent<BossSkillP>(); // BossSkillP ������Ʈ�� ã�Ƽ� bossSkillP ������ �Ҵ�
        bossSkillP.enabled = false; // BossSkillP ��ũ��Ʈ�� ó������ ��Ȱ��ȭ

        bossHPCanvas = GameObject.FindGameObjectWithTag("BossHPC");
        bossHPCanvas.SetActive(false); // ó������ ��Ȱ��ȭ

        bossHPCanvas.GetComponent<BossHP_Ctrl>().BossMax_HP = this.GetComponent<HPtest>().maxHealth;
        bossHPCanvas.GetComponent<BossHP_Ctrl>().BossCur_HP = this.GetComponent<HPtest>().maxHealth;

        
       
    }

    void Update()
    {
        if (player != null && bossAnimator.AttRadyState && isLook)
        {
            direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Boss_RotSpeed);

            bossHPCanvas.SetActive(true);

            if (bossHPCanvas.activeSelf == true && isCheck == false)
            {
                isCheck = true;

                bossHPCanvas.GetComponent<BossHP_Ctrl>().Refresh_BossHP();
            }
            if (!audioSource.isPlaying)
            {
                audioSource.clip = bgmClip;
                audioSource.Play();
            }

            // ��ų ��� ���� ���ο� ���� ��ų ��ũ��Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
            if (canUseSkills)
            {
                bossSkillP.enabled = true; // ��ų ��� ������ �� BossSkillP ��ũ��Ʈ Ȱ��ȭ
            }
            else
            {
                bossSkillP.enabled = false; // ��ų ��� �Ұ����� �� BossSkillP ��ũ��Ʈ ��Ȱ��ȭ
            }
        }
        else if (canUseSkills)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = bgmClip;
                audioSource.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            canUseSkills = true;
        }
    }
    /*
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

            Invoke("OffDamaged", 3);
        }
    }*/
}