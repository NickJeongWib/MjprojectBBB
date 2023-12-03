using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int health;
    //public Player player;

    public Image[] UIhealth;

    public AudioSource audioSource; // �ǰ����� ����� AudioSource
    public AudioClip hitSound; // �ǰ���


    void Update()
    {
        
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;

            UIhealth[health].color = new Color(1, 0, 0, 0.01f);

            // �ǰ��� ���
            audioSource.PlayOneShot(hitSound, 1.0f); // �ǰ��� ��� (������ 1.0)
        }


        else
        {
            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.01f);

            //Player Die Effect
           // player.Die();

            //Result UI
            Debug.Log("�׾����ϴ�!");
        }

    }
}
