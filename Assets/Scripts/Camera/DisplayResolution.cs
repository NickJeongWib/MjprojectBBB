using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayResolution : MonoBehaviour
{
    FullScreenMode screenMode;
    // ����ٿ� ui
    [SerializeField]
    Dropdown resolutionDropdown;
    // �ػ� ����Ʈ Index
    [SerializeField]
    int DropboxIndex;
    [SerializeField]
    CanvasScaler CanvasScale;
    [SerializeField]
    Toggle FullScreenBtn;

    // �ػ� �� �����ϱ� ���� ����
    List<Resolution> resolutions = new List<Resolution>();
    [SerializeField]
    RawImage BackImage;

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ�ȭ
        InitUI();
    }

    public void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if(Screen.resolutions[i].width >= 1200)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        // �ػ� ����
        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();

        int optionIndex = 0;

        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            // text ����
            option.text = item.width + " X " + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            // ���� ���� ���� �ػ󵵿� �°� �ؽ�Ʈ�� ���� �� �ֵ���
            if (item.width == Screen.width && item.height == Screen.height)
            {
                // ���� �������� �ػ󵵰� �´ٸ� value���� �ٲ� ǥ���ϵ���
                resolutionDropdown.value = optionIndex;
                GameManager.GMInstance.DisplayWidth = Screen.width;
                GameManager.GMInstance.DisplayHeight = Screen.height;
            }

            // 
            optionIndex++;
        }

        // �缳��
        resolutionDropdown.RefreshShownValue();

        FullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int _x)
    {
        DropboxIndex = _x;
    }

    public void On_FullScreenBtn(bool _isFull)
    {
        // ���̸� ��üȭ�� �ƴϸ� â���
        screenMode = _isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ChangeDisplay()
    {
        GameManager.GMInstance.DisplayWidth = resolutions[DropboxIndex].width;
        GameManager.GMInstance.DisplayHeight = resolutions[DropboxIndex].height;

        // Get_SetResolution null�� �ƴ϶��
        if (GameManager.GMInstance.Get_SetResolution() != null)
        {
            GameManager.GMInstance.Get_SetResolution().ResolutionSet();
        }

        // ���ȭ���� null�� �ƴ϶��
        if (BackImage != null)
        {
            BackImage.rectTransform.sizeDelta =
                new Vector2(GameManager.GMInstance.DisplayWidth, GameManager.GMInstance.DisplayHeight);
        }

        if (CanvasScale != null)
        {
            CanvasScale.referenceResolution =
                new Vector2(GameManager.GMInstance.DisplayWidth, GameManager.GMInstance.DisplayHeight);
        }

        Screen.SetResolution(resolutions[DropboxIndex].width,
          resolutions[DropboxIndex].height,
          screenMode);
    }
}
