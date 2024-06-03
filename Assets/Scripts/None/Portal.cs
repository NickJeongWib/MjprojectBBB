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
        // ��Ż ȿ���� ���
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.PORTAL);

        // ��ؽ��̸�
        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
        {
            // ĳ���� ã�ƿͼ� �ִϸ��̼� ����
            GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Animator>().SetTrigger("GoNext");
            // ĳ���� ã�ƿͼ� �ִϸ��̼� ����
            GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Assassin_Controller>().moveSpeed_Discount =
                GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Assassin_Controller>().moveSpeed;
        }


        PD.Play();
    }
    
    public void Next_Scene()
    {
        SceneManager.LoadScene("Loading");
    }
}
