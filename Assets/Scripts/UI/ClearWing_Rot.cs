using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearWing_Rot : MonoBehaviour
{
    [SerializeField]
    float Base_Rot_Speed;
    [SerializeField]
    float Rot_Speed;
    [SerializeField]
    float First_Rot_Speed;
    [SerializeField]
    float FirstWing_Time;
    [SerializeField]
    float Wing_Time;
    [SerializeField]
    RectTransform RectTr;
    [SerializeField]
    bool isOn;
    [SerializeField]
    bool isReverse;
    [SerializeField]
    bool isLeft;
    [SerializeField]
    bool isFirst;
    [SerializeField]
    bool isRight;

    [SerializeField]
    bool isCoroutineEnter;

    void OnEnable()
    {
        isOn = true;
        Base_Rot_Speed = Rot_Speed;
        Rot_Speed = First_Rot_Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOn)
        {
            if(isLeft)
            {
                if (isReverse == false)
                {
                    // ���� �ø���
                    RectTr.Rotate(Vector3.back, Rot_Speed * Time.deltaTime);

                    StartCoroutine(Change_Dir());
                    isCoroutineEnter = true;

                }
                if (isReverse == true)
                {
                    // ���� �ø���
                    RectTr.Rotate(Vector3.forward, Rot_Speed * Time.deltaTime);

                    StartCoroutine(Change_Dir());
                    isCoroutineEnter = true;
                }
            }
            else if(isRight)
            {
                if (isReverse == false)
                {
                    // ���� �ø���
                    RectTr.Rotate(Vector3.forward, Rot_Speed * Time.deltaTime);

                    StartCoroutine(Change_Dir());
                    isCoroutineEnter = true;

                }
                if (isReverse == true)
                {
                    // ���� �ø���
                    RectTr.Rotate(Vector3.back, Rot_Speed * Time.deltaTime);

                    StartCoroutine(Change_Dir());
                    isCoroutineEnter = true;
                }
            }
        }
    }

    IEnumerator Change_Dir()
    {
        if (isCoroutineEnter)
            yield break;

        // ù��° ������
        if (isFirst)
        {
            yield return new WaitForSeconds(FirstWing_Time);
            isFirst = false;

            Rot_Speed = Base_Rot_Speed;

            isCoroutineEnter = false;
            isReverse = true;
            yield break;
        }

        yield return new WaitForSeconds(Wing_Time);
        
        if (isReverse)
        {
            isReverse = false;
        }
        else if (isReverse == false)
        {
            isReverse = true;
        }

        isCoroutineEnter = false;
    }
}
