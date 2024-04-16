using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DarkBall_Pilar_Ctrl : MonoBehaviour
{
    [SerializeField]
    GameObject CountAttack;

    public Reaper_Pattern_Color pilarColor;
    public GameObject Message;
    public Material Mat;
    public float LightValue;
    public bool isEnter;
    
    public GameObject Pilar_Explosion;
    public GameObject Awakening_Pilar_Explosion;
    void Start()
    {
        Mat = Message.GetComponent<Renderer>().material;
        pilarColor = Reaper_Pattern_Color.NORMAL;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DarkBall"))
        {
            return;
        }

        if (other.gameObject.name == "DarkBall_Eff(Clone)" && isEnter == false)
        {
            // ��Ƽ���� ������ ����
            Mat.SetColor("_EmissionColor", new Color(83, 34, 191) * LightValue);

            // ��ü ��ġ �ʱ�ȭ
            CountAttack.transform.position = this.transform.position;
            // �ݰ� ��ü ����
            CountAttack.SetActive(true);

            // ��ü �Ҹ�
            other.gameObject.SetActive(false);
            this.GetComponent<BoxCollider>().enabled = false;
            isEnter = true;
        }
        else if (this.gameObject.name == "Awakening_Pattern_Obj_11" && isEnter == false)
        {
            if (other.gameObject.name == "DarkBall_Red_Eff")
            {
                // ������ ������
                Mat.SetColor("_EmissionColor", new Color(191, 0, 1) * LightValue);

                // ��ü ��ġ �ʱ�ȭ
                CountAttack.transform.position = this.transform.position;
                // �ݰ� ��ü ����
                CountAttack.SetActive(true);

                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                isEnter = true;
            }
            else // �߸� �־��� ���
            {
                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                Awakening_Pilar_Explosion.SetActive(true);
            }
        }
        else if (this.gameObject.name == "Awakening_Pattern_Obj_5" && isEnter == false)
        {
            if (other.gameObject.name == "DarkBall_Yellow_Eff")
            {
                // ����� ������
                Mat.SetColor("_EmissionColor", new Color(191, 157, 34) * LightValue);

                // ��ü ��ġ �ʱ�ȭ
                CountAttack.transform.position = this.transform.position;
                // �ݰ� ��ü ����
                CountAttack.SetActive(true);

                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                isEnter = true;
            }
            else // �߸� �־��� ���
            {
                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                Awakening_Pilar_Explosion.SetActive(true);
            }
        }
        else if (this.gameObject.name == "Awakening_Pattern_Obj_7" && isEnter == false)
        {
            if (other.gameObject.name == "DarkBall_Green_Eff")
            {
                // �ʷϻ� ������
                Mat.SetColor("_EmissionColor", new Color(3, 191, 0) * LightValue);

                // ��ü ��ġ �ʱ�ȭ
                CountAttack.transform.position = this.transform.position;
                // �ݰ� ��ü ����
                CountAttack.SetActive(true);

                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                isEnter = true;
            }
            else // �߸� �־��� ���
            {
                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                Awakening_Pilar_Explosion.SetActive(true);
            }
        }
        else if (this.gameObject.name == "Awakening_Pattern_Obj_1" && isEnter == false)
        {
            if (other.gameObject.name == "DarkBall_Blue_Eff")
            {
                // �Ķ��� ������
                Mat.SetColor("_EmissionColor", new Color(0, 9, 191) * LightValue);

                // ��ü ��ġ �ʱ�ȭ
                CountAttack.transform.position = this.transform.position;
                // �ݰ� ��ü ����
                CountAttack.SetActive(true);

                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                isEnter = true;
            }
            else // �߸� �־��� ���
            {
                // ��ü �Ҹ�
                other.gameObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                Awakening_Pilar_Explosion.SetActive(true);
            }
        }
    }
}
