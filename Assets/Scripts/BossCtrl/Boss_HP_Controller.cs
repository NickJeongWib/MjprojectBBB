using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Boss_HP_Controller : MonoBehaviour
{
    public GameObject Boss_HP_Canvas;
    public int BossMaxHP;
    public int BossCurHP;
    public bool isAwakening;
    public bool isDead;
    public bool isWSkillChek;

    [SerializeField]
    PlayableDirector PD;

    [SerializeField]
    GameObject[] Boss_Skill_Objects;

    [Header("-----ShakeCam-----")] // ī�޶� ��鸲 ����
    [SerializeField]
    float CamShake_Intensity;
    [SerializeField]
    float CamShake_Time;


    [Header("-----Reaper-----")] // 2�� ������ ��� 
    [SerializeField]
    int Reaper_SP_1_HP; // ù��° ������ ü��
    [SerializeField]
    int Reaper_SP_2_HP;
    [SerializeField]
    int Reaper_SP_3_HP;
    public bool isReaper_SP_ATK_1;
    public bool isReaper_SP_ATK_2;
    public bool isReaper_SP_ATK_3;

    [Header("-----Treant-----")] // 3�� ������ ��� 
    [SerializeField]
    int Treant_Possible_FormChange_HP; //�� ü���� ü��


    [Header("-----Dragon-----")] // 4�� ������ ���
    [SerializeField]
    bool isTriggerHandled = false;
    const int ChangeThunder_HP = 70;
    public bool isChange_Thunder;

    [SerializeField]
    const int ChangeFire_HP = 40;
    public bool isChange_Fire;


    public PlaySceneManager playSceneManager;



    // Start is called before the first frame update
    void Start()
    {
        Boss_HP_Canvas.transform.localScale = Vector3.zero;
        // PD.GetComponent<PlayableDirector>();
    }

    void Update()
    {
        #region IronGuard2_n_Death
        if (this.gameObject.name == "IronGuard2_n" && BossCurHP <= 0 && !isDead)
        {
            BossCurHP = 0;
            isDead = true;

            // ���� ��ų ��Ȱ��ȭ
            this.gameObject.transform.parent.GetChild(1).gameObject.SetActive(false);

            // �ݶ��̴� ��Ȱ��ȭ
            this.GetComponent<BoxCollider>().enabled = false;
            // �״� ��� ���
            Animator reaperanimator = GetComponent<Animator>();
            reaperanimator.SetTrigger("doDie2");
            Boss_HP_Canvas.transform.localScale = Vector3.zero;
            playSceneManager.BossClear();

            for (int i = 0; i < Boss_Skill_Objects.Length; i++)
            {
                Boss_Skill_Objects[i].SetActive(false);
            }

            PD.Play();
        }
        #endregion

        #region Reaper_Death
        if (this.gameObject.name == "Reaper" && BossCurHP <= 0 && !isDead)
        {
            BossCurHP = 0;
            isDead = true;
            this.GetComponent<CapsuleCollider>().enabled = false;

            Animator reaperanimator = GetComponent<Animator>();
            reaperanimator.SetTrigger("isDeath");
            Boss_HP_Canvas.transform.localScale = Vector3.zero;
            playSceneManager.BossClear();


            for (int i = 0; i < Boss_Skill_Objects.Length; i++)
            {
                Boss_Skill_Objects[i].SetActive(false);
            }

            PD.Play();
        }
        #endregion

        #region Treant_Death
        if (this.gameObject.name == "Treant" && BossCurHP <= 0 && !isDead)
        {
            BossCurHP = 0;
            isDead = true;
            this.GetComponent<CapsuleCollider>().enabled = false;

            Animator reaperanimator = GetComponent<Animator>();
            reaperanimator.SetTrigger("isDeath");
            Boss_HP_Canvas.transform.localScale = Vector3.zero;
            playSceneManager.BossClear();
            PD.Play();
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Debug.Log(this.name);

            // ���� �ǰ� �Ҹ� ���
            GameManager.GMInstance.SoundManagerRef.PlaySFX(((SoundManager.SFX)Random.Range((int)SoundManager.SFX.BOSS_HIT_1, (int)SoundManager.SFX.CLEAR_SOUND)));
            GameManager.GMInstance.CamShakeRef.ShakeCam(CamShake_Intensity, CamShake_Time);

            // HP�� 0�̻��� �� �۵�
            if (BossCurHP > 0)
            {
                #region Reaper 
                if (this.gameObject.name == "Reaper")
                {
                    // ���� ü���� 50�� ���� �۾�����
                    if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_1_HP && isReaper_SP_ATK_1 == false)
                    {
                        isReaper_SP_ATK_1 = true;
                    }
                    // ���� ü���� 50�� ���� �۾�����
                    else if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_2_HP && isReaper_SP_ATK_2 == false)
                    {
                        isReaper_SP_ATK_2 = true;
                    }
                    // ���� ü���� 50�� ���� �۾�����
                    else if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_3_HP && isReaper_SP_ATK_3 == false)
                    {
                        isReaper_SP_ATK_3 = true;
                    }

                    // ���� ü���� 50�� ���� �۾�����
                    if (BossCurHP <= (BossMaxHP / 100) * 50 && isAwakening == false)
                    {
                        isAwakening = true;
                    }
                }

                #endregion // ������ hp�� ���� ���� ����

                #region Treant
                // 3�� ������ ���
                if (this.gameObject.name == "Treant")
                {
                    // ������ ü�� ���� �۾��� ���
                    if (BossCurHP <= (BossMaxHP / 100) * Treant_Possible_FormChange_HP)
                    {
                        this.GetComponent<Treant_Controller>().isStartFormChange = true;
                    }

                    // ��� ���¸� return
                    if (this.GetComponent<Treant_Controller>().isBarrier)
                    {
                        return;
                    }
                }
                #endregion

                #region Dragon
                if (this.gameObject.name == "Dragon")
                {
                    // ���� ü���� 70�� ���� �۰� ���������� �������� �ʾҴٸ�
                    if (BossCurHP <= (BossMaxHP / 100) * ChangeThunder_HP && isChange_Thunder == false)
                    {
                        isChange_Thunder = true;
                    }
                    // ���� ü���� 40�� ���� �۾�����
                    else if (BossCurHP <= (BossMaxHP / 100) * ChangeFire_HP && isChange_Fire == false)
                    {
                        isChange_Fire = true;
                    }
                }
                #endregion

                //if (!isWSkillChek)
                //{
                //    Attack weapon = other.GetComponent<Attack>();
                //    // Debug.Log("Damage: " + weapon.damage);

                //    BossCurHP -= weapon.damage;
                //}

                Attack weapon = other.GetComponent<Attack>();
                BossCurHP -= weapon.damage;

                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossCurHP;
                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
            }
        }
        else if (other.gameObject.name == "DarkBallCounter_Eff")
        {
            // ����� ��ü �ݰ�
            other.gameObject.SetActive(false);

            BossCurHP -= 1000;

            Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossCurHP;
            // this.GetComponent<Reaper_Controller>().CurHP = BossCurHP;
            Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggerHandled = false; // Ʈ���� ���� �� �÷��� �ʱ�ȭ
    }
}
