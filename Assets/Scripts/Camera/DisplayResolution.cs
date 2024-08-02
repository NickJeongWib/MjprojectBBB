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
    // �ػ� �� �����ϱ� ���� ����
    List<Resolution> resolutions = new List<Resolution>();

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ�ȭ
        InitUI();
    }

    public void InitUI()
    {
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
    }

    public void DropboxOptionChange(int x)
    {
        DropboxIndex = x;
    }

    public void ChangeDisplay()
    {
        Screen.SetResolution(resolutions[DropboxIndex].width,
            resolutions[DropboxIndex].height,
            screenMode);

        GameManager.GMInstance.DisplayWidth = resolutions[DropboxIndex].width;
        GameManager.GMInstance.DisplayHeight = resolutions[DropboxIndex].height;

        // Get_SetResolution null�� �ƴ϶��
        if (GameManager.GMInstance.Get_SetResolution() != null)
        {
            GameManager.GMInstance.Get_SetResolution().ResolutionSet();
        }
    }
}
