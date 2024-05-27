using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Ctrl : MonoBehaviour
{
    [SerializeField]
    Transform StartPos;
    [SerializeField]
    Transform StoneStart_Parent;
    [SerializeField]
    float UpSpeed;
    [SerializeField]
    float ThrowSpeed;
    [SerializeField]
    bool isActiveT;
    public bool isSeize;
    public bool isThrow;


    void OnEnable()
    {

        Quaternion rotation = Quaternion.identity;
        this.transform.rotation = rotation;
        // ó���� ������ ������
        isThrow = false;
        // ������ �ʾ����� false
        isSeize = false;
        // active Ȱ��ȭ
        isActiveT = true;
        // ���� �θ� ������Ʈ �ʱ�ȭ
        this.transform.parent = StoneStart_Parent;
        // ��ġ �ʱ�ȭ
        Vector3 Pos = StartPos.position + StartPos.forward * 15.0f;
        this.transform.position = new Vector3(Pos.x, -4.0f, Pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ö���� ����
        if (isActiveT && this.transform.localPosition.y <= 10.0f && !isSeize)
        {
            this.transform.Translate(Vector3.up * UpSpeed * Time.deltaTime);
        }
        
        if (isThrow == true)
        {
            this.transform.Translate(Vector3.forward * ThrowSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && isThrow)
        {
            this.gameObject.SetActive(false);
        }
    }
}
