using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PlaySceneManager : MonoBehaviour
{
    public bool isArrivePlayScene;

    public GameObject deathMenu; // ��� �޴� UI ������Ʈ
    public RectTransform deathMenuRectTransform; // ��� �޴��� RectTransform
    private float currentWidth = 0f; // ���� Width ���� ������ ����
    public GameObject clearMenu; // ��� �޴� UI ������Ʈ
    private float startTime;
    public TextMeshProUGUI clearTimeText;

    // public AudioClip deathSound; // ��� ȿ����
    // public List<AudioSource> allAudioSources; // ��� ����� �ҽ�

    public int health;
    //public Player player;

    public Image[] UIhealth;

    [SerializeField]
    Slider SFX_Slider;
    [SerializeField]
    Slider BGM_Slider;

    [SerializeField]
    Transform StartPos;

    public float SFX_Volume;

    //public TextMeshProUGUI[] keyCodeName;

    //public GameObject keySettingImage;
    //public GameObject keySettingFailImage;

    [Header("----Raid_Variable----")]
    public bool isRaidStart;

    [Header("----Skill----")]
    public Skill_Test[] Skills_Info;

    [Header("----Character----")]
    [SerializeField]
    GameObject[] Spawn_Characters;
    public GameObject CurCharacter;

    [Header("----CutScene----")]
    public bool isCutScene;
    [SerializeField]
    PlayableDirector PD;
    [SerializeField]
    PlayableDirector DeathPD;


    // public AudioSource hitAudioSource; // �ǰ����� ����� AudioSource
    // public AudioSource deathAudioSource; // ������� ����� AudioSource

    // public AudioClip hitSound; // �ǰ���

    public BossAnimator bossAnimator; // BossAnimator�� ����

    private void Awake()
    {
        deathMenu.SetActive(false);

        GameManager.GMInstance.Set_PlaySceneManager(this);


        // GameManager.GMInstance.Set_PlaySceneManager(this);
        //AudioSource[] sources = FindObjectsOfType<AudioSource>(); // ��� ����� �ҽ��� ã�� �迭�� �߰�
        //allAudioSources = new List<AudioSource>(); // ����Ʈ �ʱ�ȭ

        //foreach (AudioSource src in sources) // �迭�� �ִ� ����� �ҽ��� ��ȸ
        //{
        //    if (src != deathAudioSource && src != hitAudioSource) // ������� �ǰ����� ����ϴ� ����� �ҽ��� �ƴ� ���
        //    {
        //        allAudioSources.Add(src); // ����Ʈ�� �߰�
        //    }
        //}

        // ���� ĳ���� ����
        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
        {
            GameObject Char = Instantiate(Spawn_Characters[0], StartPos.position, StartPos.rotation);
            CurCharacter = Char;
        }
    }

    private void Start()
    {
        Init();

        //for (int i = 0; i < keyCodeName.Length; i++)
        //{
        //    keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        //}
    }

    public void GoToMainMenu()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }

        // Time.timeScale = 1; // Ÿ�ӽ������� ������� ����
        // ���� �������� ���ư��� �� ���� ��� ����
        GameManager.GMInstance.cur_Scene = Define.Cur_Scene.NONE;
        SceneManager.LoadScene("Loading"); // "MainMenu"��� �̸��� Scene�� �ε�
    }

    public void ClearPopUpClose()
    {
        clearMenu.SetActive(false);
    }

    void Update()
    {
        if (deathMenu.activeSelf) // ��� �޴��� Ȱ��ȭ�� ���
        {
            currentWidth = Mathf.Lerp(currentWidth, 650, Time.unscaledDeltaTime * 0.02f); // �������� �Լ��� �̿��� Width ���� õõ�� ������Ŵ
            deathMenuRectTransform.sizeDelta = new Vector2(currentWidth, deathMenuRectTransform.sizeDelta.y); // ���� ���� Width ������ ��� �޴��� Width�� ������Ʈ
        }

        //for (int i = 0; i < keyCodeName.Length; i++)
        //{
        //    keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        //}

        //if (keySettingImage.activeSelf)
        //{
        //    // Ű������ ��� Ű�� ���� Ȯ���մϴ�.
        //    foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        //    {
        //        // ���콺 ��ư�� �ƴ� ��쿡�� ó���մϴ�.
        //        if (!IsMouseButton(keyCode) && Input.GetKeyDown(keyCode))
        //        {
        //            // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        //            keySettingImage.SetActive(false);
        //            break; // Ű �Է��� �����Ǿ����Ƿ� ������ �����մϴ�.
        //        }
        //    }
        //}

        //if (keySettingFailImage.activeSelf && Input.GetMouseButtonDown(0))
        //{
        //    // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        //    keySettingFailImage.SetActive(false);
        //}
    }

    //// �Էµ� Ű�� ���콺 ��ư���� Ȯ���մϴ�.
    //private bool IsMouseButton(KeyCode keyCode)
    //{
    //    return keyCode >= KeyCode.Mouse0 && keyCode <= KeyCode.Mouse6;
    //}

    public void HealthDown()
    {
        if (health > 0)
        {
            health--;

            UIhealth[health].gameObject.SetActive(false);

            // �ǰ��� ���
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.HitSound);

            // hitAudioSource.PlayOneShot(hitSound, 1.0f); // �ǰ��� ��� (������ 1.0)
        }
    }

    public void HealthActivateAll()
    {
        foreach (Image img in UIhealth)
        {
            if (img != null)

            {
                img.gameObject.SetActive(true);
            }
        }
    }

    void DisplayClearTime(float time)
    {
        // �ð��� ��, �ʷ� ��ȯ�Ͽ� ǥ��
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        //int milliseconds = Mathf.FloorToInt((time * 1000F) % 1000F);
        clearTimeText.text = string.Format("{0:00}��:{1:00}��", minutes, seconds);
    }
    public void BossClear()
    {
        float clearTime = Time.time - startTime;
        DisplayClearTime(clearTime);
        // StartCoroutine(ShowClearMenuAfterDelay(3.0f));
    }

    private IEnumerator ShowClearMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        clearMenu.SetActive(true);
    }
    public void CharacterDie()
    {
        //bossAnimator.AttRadyState = false; // �÷��̾ ����Ͽ����Ƿ� AttReadyState�� false�� ����

        //All Health UI Off


        //Player Die Effect
        // Time.timeScale = 0; // Ÿ�ӽ������� 0���� ����
        // deathMenu.SetActive(true); // ��� �޴��� Ȱ��ȭ

        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.DeadSong);
        DeathPD.Play();
        //AudioSource[] sources = FindObjectsOfType<AudioSource>(); // ��� ����� �ҽ��� ã�� �迭�� �߰�
        //allAudioSources.Clear(); // ����Ʈ�� ���ϴ�.

        //foreach (AudioSource src in sources) // �迭�� �ִ� ����� �ҽ��� ��ȸ
        //{
        //    if (src != deathAudioSource && src != hitAudioSource) // ������� �ǰ����� ����ϴ� ����� �ҽ��� �ƴ� ���
        //    {
        //        allAudioSources.Add(src); // ����Ʈ�� �߰�
        //    }
        //}

        //foreach (AudioSource audioSource in allAudioSources) // ��� ����� �ҽ��� ��ȸ
        //{
        //    audioSource.volume = 0; // ����� �ҽ��� ������ 0���� ����
        //    audioSource.clip = null; // ����� �ҽ��� Ŭ���� ��Ȱ��ȭ
        //    audioSource.Stop();
        //}

        //GameObject pObject = GameObject.FindGameObjectWithTag("P"); // P�±׸� ���� ������Ʈ�� ã���ϴ�.
        //if (pObject != null) // P�±׸� ���� ������Ʈ�� �����ϸ�
        //{
        //    BoxCollider boxCollider = pObject.GetComponent<BoxCollider>(); // �ش� ������Ʈ�� BoxCollider ������Ʈ�� ã���ϴ�.
        //    if (boxCollider != null) // BoxCollider ������Ʈ�� �����ϸ�
        //    {
        //        boxCollider.enabled = false; // �ش� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        //    }
        //}

        //deathAudioSource.PlayOneShot(deathSound, 1.0f); // ��� ȿ���� ���

        //Result UI
        // Debug.Log("�׾����ϴ�!");
    }


    private void Init()
    {
        // ����� ����
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Floor_1);

        PD = GetComponent<PlayableDirector>();

        // ���� ���� �ʱ�ȭ
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            SFX_Slider.value = GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume;
        }

        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            BGM_Slider.value = GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume;
        }

        PD.Play();
    }

    IEnumerator Sound_Enable()
    {
        yield return new WaitForSeconds(2.0f);
        isArrivePlayScene = true;
    }

    // TODO ## �κ�ȭ�� ȯ�漳�� ���� ���� �Լ�
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

    public void OpenToConfig(GameObject obj)
    {
        
        obj.gameObject.SetActive(true);
    }
    public void CloseToConfig(GameObject obj)
    {
        
        obj.gameObject.SetActive(false);
    }

    //public void OpenToKeySetting(GameObject obj)
    //{
    //    obj.gameObject.SetActive(true);
    //}

    #region Signal
    public void CutScene_Start()
    {
        isCutScene = true;
    }

    public void CutScene_Start_Anim()
    {
        // �����̸�
        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
        {
            CurCharacter.transform.GetChild(0).GetComponent<Animator>().SetTrigger("StartAnim");
        }
    }

    public void CutScene_End()
    {
        isCutScene = false;   
    }

    public void RaidClear_SFX()
    {
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.CLEAR_SOUND);
    }

    // �ñ׳� ȣ�� 
    public void RaidStart()
    {
        // ���̵� ����
        isRaidStart = true;

        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
        {
            CurCharacter.transform.GetChild(0).GetComponent<Assassin_Controller>().isDodge = false;
        }

        startTime = Time.time;
    }

    public void RaidEnd()
    {
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.CLEAR_SOUND);
    }

    public void CutSceneUI_Off()
    {
        StartCoroutine(UI_Off(clearMenu));
    }


    IEnumerator UI_Off(GameObject _obj)
    {
        yield return new WaitForSeconds(1.5f);

        _obj.SetActive(false);
    }
    #endregion
}
