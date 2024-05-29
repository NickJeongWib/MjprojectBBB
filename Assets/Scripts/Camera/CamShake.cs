using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour
{
    CinemachineVirtualCamera CMVCam;
    float ShakeTime;

    void Awake()
    {
        // �ó׸ӽ� ���߾� ������Ʈ
        CMVCam = GetComponent<CinemachineVirtualCamera>();
        // ���� �Ŵ����� �Ѱ���
        GameManager.GMInstance.CamShakeRef = this;


    }

    void Start()
    {
        CMVCam.Follow = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(1);
        CMVCam.LookAt = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(1);

        // ��鸮�� �ϴ� ������Ʈ�� ������
        CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (CamChannel.m_AmplitudeGain != 0)
        {
            CamChannel.m_AmplitudeGain = 0;
        }
    }

    public void ShakeCam(float _intensity, float _time)
    {
        // ��鸮�� �ϴ� ������Ʈ�� ������
        CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // ��鸲 ������ �Ű������� ���� ������ ����
        CamChannel.m_AmplitudeGain = _intensity;
        // ��鸮�� �ð� ����
        ShakeTime = _time;
    }

    void Update()
    {
        // ���� �� ShakeTime��ŭ ����
        if (ShakeTime > 0)
        {
            // ShakeTime ����
            ShakeTime -= Time.deltaTime;
            if (ShakeTime <= 0.0f)
            {
                // ��鸮�� �ϴ� ������Ʈ ������
                CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                CamChannel.m_AmplitudeGain = 0f;
            }
        }
    }
}
