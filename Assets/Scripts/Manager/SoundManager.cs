using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SoundManager : MonoBehaviour
{
    /** ������� ���� */
    [Header("---BGM---")]
    public AudioClip[] BGMClips;
    public AudioSource[] BGMPlayers;
    public int BGMChannels;
    int BGMChannelIndex;

    /** ȿ���� ���� */
    [Header("---SFX---")]
    public AudioClip[] SFXClips;
    public AudioClip[] Assasin_SFXClips;
    public AudioClip[] Boss_1F_SFXClips;
    public AudioClip[] Boss_2F_SFXClips;
    public int SFXChannels;
    public AudioSource[] SFXPlayers;
    int SFXChannelIndex;

    public bool bIsSFXOn;
    public bool bIsBGMOn;

    public enum BGM
    {
        Lobby,
        Floor_1,
        BOSS_1FLOOR,
        BOSS_2FLOOR,
        BOSS_3FLOOR,
        LOADING,
    }

    #region Enum_SFX
    public enum SFX
    {
        HitSound,
        DeadSong,
        DoorOpening,
        DoorClosing,
        
        PORTAL,

        BOSS_HIT_1,
        BOSS_HIT_2,

        END,
    }

    public enum Boss_1F_SFX
    {
        RAZER,
        JUMP_GROUND_ATK,
        COMBO_9_ATK,
        SWORD_SPIN,
        THROW_SWORDSPIRIT,
    }

    public enum Boss_2F_SFX
    {
        BASE_ATK_SFX,
        DARK_DECLINE_SFX,
        DARK_BALL_THROW_SFX,
        DARK_SOUL_SFX,

    }

    public enum Assasin_SFX
    {
       // ��ų ����
       SWING_1, // 0
       SWING_2, // 1
       SWING_3, // 2
       R_Sound, // 3

       // ���̽� ����
       ASSASIN_VOICE_1, // 4
       ASSASIN_VOICE_2, // 5
       ASSASIN_VOICE_3, // 6
       ASSASIN_VOICE_4, // 7

       // ������ ��ô ����
       THROW_KNIFE,
    }

    #endregion

    void Awake()
    {
        Init();
    }

    void Init()
    {
        /** ����� �÷��̾� �ʱ�ȭ */
        GameObject BGMObject = new GameObject("BGMPlayer");
        /** BGMObject�� �θ�Ŭ���� �� ��ũ��Ʈ�� ���� ������Ʈ�� �Ѵ�. */
        BGMObject.transform.parent = transform;
        /** ä���� ������ŭ ����� ����� ���� */
        BGMPlayers = new AudioSource[BGMChannels];

        bIsBGMOn = true;

        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            /** BGMPlayer�� BGMObject�� �߰��� AudioSource�� �����´�. */
            BGMPlayers[i] = BGMObject.AddComponent<AudioSource>();
            /** ����� ��� ���� �ݺ� */
            BGMPlayers[i].loop = true;
            /** ����� �÷��� */
            BGMPlayers[i].clip = BGMClips[0];
            BGMPlayers[i].volume = 0.5f;
        }

        /** ȿ���� �÷��̾� �ʱ�ȭ */
        GameObject SFXObject = new GameObject("SFXPlayer");
        /** SFXObject�� �θ�Ŭ���� �� ��ũ��Ʈ�� ���� ������Ʈ�� �Ѵ�. */
        SFXObject.transform.parent = transform;
        /** ä���� ������ŭ ȿ���� ����� ���� */
        SFXPlayers = new AudioSource[SFXChannels];
        bIsSFXOn = true;


        /** ����� ȿ���� ������ŭ �ݺ� */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            SFXPlayers[i] = SFXObject.AddComponent<AudioSource>();
            /** �ʱ� ��� off */
            SFXPlayers[i].playOnAwake = false;
            SFXPlayers[i].volume = 0.5f;
        }

        // PlayBGM(BGM.Lobby);

    }

    void Start()
    {
        // GameManager.GMInstance.SoundManagerRef = this;
    }


    #region SFX_Sound
    /** TODO ## SoundManager.cs ȿ���� ��� ���� �Լ� */
    /** SFX�� �Ű������� �޴� ȿ���� ��� �Լ� ���� */
    // ���� UI, UX ���� ȿ���� ���
    public void PlaySFX(SFX sfx)
    {
        /** ����� Length����ŭ �ݺ� */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** ���� ���� ȿ������ �������̸�? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** �ٽ� �ݺ��� �ʱ���� ���� */
                continue;
            }

            /** ChanelIndex�� LoopIndex������ �ٲ��ش�. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers�� 0��° Clip�� SFX Enum�� ������ �����´�. */
            SFXPlayers[LoopIndex].clip = SFXClips[(int)sfx];
            /** ��� */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    // ĳ���� ��ؽ� Ŭ���� ���� ȿ���� ���
    public void Play_Assasin_SFX(Assasin_SFX sfx)
    {
        /** ����� Length����ŭ �ݺ� */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** ���� ���� ȿ������ �������̸�? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** �ٽ� �ݺ��� �ʱ���� ���� */
                continue;
            }

            /** ChanelIndex�� LoopIndex������ �ٲ��ش�. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers�� 0��° Clip�� SFX Enum�� ������ �����´�. */
            SFXPlayers[LoopIndex].clip = Assasin_SFXClips[(int)sfx];
            /** ��� */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    // 1�� ���� ���� ȿ���� ���
    public void Play_1FBoss_SFX(Boss_1F_SFX sfx)
    {
        /** ����� Length����ŭ �ݺ� */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** ���� ���� ȿ������ �������̸�? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** �ٽ� �ݺ��� �ʱ���� ���� */
                continue;
            }

            /** ChanelIndex�� LoopIndex������ �ٲ��ش�. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers�� 0��° Clip�� SFX Enum�� ������ �����´�. */
            SFXPlayers[LoopIndex].clip = Boss_1F_SFXClips[(int)sfx];
            /** ��� */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    // 2�� ���� ���� ȿ���� ���
    public void Play_2FBoss_SFX(Boss_2F_SFX sfx)
    {
        /** ����� Length����ŭ �ݺ� */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** ���� ���� ȿ������ �������̸�? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** �ٽ� �ݺ��� �ʱ���� ���� */
                continue;
            }

            /** ChanelIndex�� LoopIndex������ �ٲ��ش�. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers�� 0��° Clip�� SFX Enum�� ������ �����´�. */
            SFXPlayers[LoopIndex].clip = Boss_2F_SFXClips[(int)sfx];
            /** ��� */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }
    #endregion


    #region BGM_Sound
    /** TODO ## SoundManager.cs ����� ��� ���� �Լ� */
    public void PlayBGM(BGM bgm)
    {
        /** ����� Length����ŭ �ݺ� */
        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            int LoopIndex = (i + BGMChannelIndex) % BGMPlayers.Length;

            ///** ���� ���� ȿ������ �������̸�? */
            //if (BGMPlayers[LoopIndex].isPlaying)
            //{
            //    /** �ٽ� �ݺ��� �ʱ���� ���� */
            //    continue;
            //}

            /** ChanelIndex�� LoopIndex������ �ٲ��ش�. */
            BGMChannelIndex = LoopIndex;
            /** SFXPlayers�� 0��° Clip�� SFX Enum�� ������ �����´�. */
            BGMPlayers[LoopIndex].clip = BGMClips[(int)bgm];
            /** ��� */
            BGMPlayers[LoopIndex].Play();
            break;
        }
    }
    #endregion
}