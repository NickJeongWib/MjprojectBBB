using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ActiveT : MonoBehaviour
{
    [SerializeField]
    GameObject[] ActiveObj;

    void OnEnable()
    {
        // ��Ȱ��ȭ �� ������Ʈ �� Ȱ��ȭ
        for (int i = 0; i < ActiveObj.Length; i++)
        {
            if (ActiveObj[i].activeSelf == false)
            {
                ActiveObj[i].SetActive(true);
            }
        }
    }
}
