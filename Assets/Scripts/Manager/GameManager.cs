using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //// ManagerRef
    //[SerializeField]
    //TitleSceneManager TitleSceneManagerRef;
    //public TitleSceneManager titleManagerRef { get { return TitleSceneManagerRef; } set { TitleSceneManagerRef = value; } }

    //[SerializeField]
    //LobbyManager LobbyManagerRef;
    //public LobbyManager lobbyManagerRef { get { return LobbyManagerRef; } set { LobbyManagerRef = value; } }

    // �̱���
    public static GameManager GMInstance;

    // Managers Reference
    public SoundManager SoundManagerRef;

    void Awake()
    {
        /** GMInstance�� �� Ŭ������ �ǹ��Ѵ�. */
        GMInstance = this;

        /** ȭ���� �ٲ��� Ŭ���� ���� */
        DontDestroyOnLoad(gameObject);
    }
}