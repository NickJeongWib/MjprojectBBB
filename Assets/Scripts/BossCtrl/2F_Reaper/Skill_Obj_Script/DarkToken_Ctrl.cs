using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkToken_Ctrl : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Activefalse());
    }

    IEnumerator Activefalse()
    {
        // ���󺹱� �� � ����ġ
        yield return new WaitForSeconds(5.0f);
        this.transform.GetChild(2).GetChild(0).transform.localPosition = Vector3.zero;
        this.transform.GetChild(2).GetChild(1).transform.localPosition = Vector3.zero;
        this.transform.GetChild(2).gameObject.SetActive(true);
        this.transform.gameObject.SetActive(false);
    }
}
