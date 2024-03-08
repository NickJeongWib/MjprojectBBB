using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall_Pilar_Ctrl : MonoBehaviour
{
    [SerializeField]
    GameObject CountAttack;

    public GameObject Message;
    public Material Mat;
    public float LightValue;

    void Start()
    {
        Mat = Message.GetComponent<Renderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DarkBall_Eff(Clone)")
        {
            
            // Mat.EnableKeyword("_EMISSION");
            // Mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            Mat.SetColor("_EmissionColor", new Color(83, 34, 191) * 1);

            // ��ü ��ġ �ʱ�ȭ
            CountAttack.transform.position = this.transform.position;
            // �ݰ� ��ü ����
            CountAttack.SetActive(true);

            // ��ü �Ҹ�
            other.gameObject.SetActive(false);
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
