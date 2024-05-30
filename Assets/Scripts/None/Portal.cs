using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // ���� ����Ʈ�� ��Ÿ���� ������Ʈ

    public PlaySceneManager playSceneManager; // UI �Ŵ��� ����
    [SerializeField]
    PlayableDirector PD;


    private void OnTriggerEnter(Collider other)
    {
        PD.Play();
    }
    
    public void Next_Scene()
    {
        SceneManager.LoadScene("Loading");
    }
}
