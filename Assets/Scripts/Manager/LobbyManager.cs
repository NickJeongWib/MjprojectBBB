using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    PlayableDirector PD;

    [SerializeField]
    Slider SFX_Slider;
    [SerializeField]
    Slider BGM_Slider;
    public float SFX_Volume;

    public GameObject settingUI;
    public GameObject ExitPop; // ExitPop �̹��� ������Ʈ�� Inspector���� �����մϴ�.
    public bool isPaused;
    public bool doSetUI;



    // Start is called before the first frame update
    void Start()
    {
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Lobby);
        GameManager.GMInstance.cur_Scene = Define.Cur_Scene.MAIN;

        PD = GetComponent<PlayableDirector>();

        PD.Play();

        // ���� ���� �ʱ�ȭ
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            SFX_Slider.value = GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume;
        }

        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            BGM_Slider.value = GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume;
        }
    }
    #region Sound BGM / SFX
    public void SetSFXVolume(float volume)
    {
        // �迭�� �����ϴ� ����Ʈ ������ ũ�⸦ �����Ѵ�.
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            // ȿ���� ���Ұ� �� ���� ���
            //if (SFXToggle.GetComponent<Toggle>().isOn == false)
            //{
            //    return;
            //}

            GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume = volume;
        }


        SFX_Volume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        // �迭�ȿ� �����ϴ� ������� ũ�⸦ �����Ѵ�.
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            // ����� ���Ұ� �� ���� ���
            //if (BGMToggle.GetComponent<Toggle>().isOn == false)
            //{
            //    return;
            //}

            GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume = volume;
        }

    }
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ�� ���ȴ��� Ȯ��
        {
            if (isPaused )
            {
                OnClickXb(); // ExitPop�� ��Ȱ��ȭ�Ͽ� �˾��� �ݽ��ϴ�.
            }
            else
            {
                OnClickExitOpBtn(); // ExitPop�� Ȱ��ȭ
            }
        }
    }



    // Ob ��ư�� ������ ���� ������ ������ �Լ��Դϴ�.
    public void OnClickOb()
    {
        Application.Quit(); // ������ �����մϴ�.
    }

    // Xb ��ư�� ������ ���� ������ ������ �Լ��Դϴ�.
    public void OnClickXb()
    {
        ExitPop.SetActive(false); // ExitPop�� ��Ȱ��ȭ�Ͽ� �˾��� �ݽ��ϴ�.
        isPaused = false;
    }

    public void OnClickExitOpBtn()
    {
        if (!doSetUI)
        {
            ExitPop.SetActive(true); // ExitPop�� Ȱ��ȭ
            isPaused = true;
        }
    }

    public void OpenSettingUI()
    {
        settingUI.SetActive(true);
        doSetUI = true;
    }

    public void CloseSettingUI()
    {
        settingUI.SetActive(false);
        doSetUI = false;
    }
}
