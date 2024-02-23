using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_ObjPool : MonoBehaviour
{
    public GameObject DarkDecline_objectPrefab; // ����Ǽ��
    public GameObject DarkDecline2_objectPrefab; // ����Ǽ�� ����
    public GameObject DarkHand_objectPrefab; // ����Ǽ���
    public GameObject DarkHand2_objectPrefab; // ����Ǽ���2

    public int poolSize; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> DarkDecline_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> DarkDecline2_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> DarkHand_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> DarkHand2_objectPool; // ������Ʈ Ǯ


    [SerializeField]
    Transform DarkDecline_Parent;
    [SerializeField]
    Transform DarkDecline2_Parent;
    [SerializeField]
    Transform DarkHand_Parent;
    [SerializeField]
    Transform DarkHand2_Parent;

    // Start is called before the first frame update
    void Start()
    {
        DarkDecline_objectPool = new List<GameObject>();
        DarkHand_objectPool = new List<GameObject>();
        DarkHand2_objectPool = new List<GameObject>();

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj_1 = Instantiate(DarkDecline_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = DarkDecline_Parent;
            DarkDecline_objectPool.Add(obj_1);

            GameObject obj_2 = Instantiate(DarkHand_objectPrefab);
            obj_2.transform.Rotate(Vector3.zero);
            obj_2.SetActive(false);
            obj_2.transform.parent = DarkHand_Parent;
            DarkHand_objectPool.Add(obj_2);

            GameObject obj_3 = Instantiate(DarkHand2_objectPrefab);
            obj_3.transform.Rotate(Vector3.zero);
            obj_3.SetActive(false);
            obj_3.transform.parent = DarkHand2_Parent;
            DarkHand2_objectPool.Add(obj_3);

            GameObject obj_4 = Instantiate(DarkDecline2_objectPrefab);
            obj_4.transform.Rotate(Vector3.zero);
            obj_4.SetActive(false);
            obj_4.transform.parent = DarkDecline2_Parent;
            DarkDecline2_objectPool.Add(obj_4);
        }
    }

        //// �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject obj_2 = Instantiate(DarkHand_objectPrefab);
        //    obj_2.transform.Rotate(Vector3.zero);
        //    obj_2.SetActive(false);
        //    obj_2.transform.parent = DarkHand_Parent;
        //    DarkHand_objectPool.Add(obj_2);
        //}

        //// �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject obj_3 = Instantiate(DarkHand2_objectPrefab);
        //    obj_3.transform.Rotate(Vector3.zero);
        //    obj_3.SetActive(false);
        //    obj_3.transform.parent = DarkHand2_Parent;
        //    DarkHand2_objectPool.Add(obj_3);
        //}

    public GameObject GetDarkDeclineFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < DarkDecline_objectPool.Count; i++)
        {
            if (!DarkDecline_objectPool[i].activeInHierarchy)
            {
                DarkDecline_objectPool[i].SetActive(true);
                return DarkDecline_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(DarkDecline_objectPrefab);
        newObj.SetActive(true);
        DarkDecline_objectPool.Add(newObj);
        newObj.transform.parent = DarkDecline_Parent;
        return newObj;
    }

    public GameObject GetDarkDecline2FromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < DarkDecline2_objectPool.Count; i++)
        {
            if (!DarkDecline2_objectPool[i].activeInHierarchy)
            {
                DarkDecline2_objectPool[i].SetActive(true);
                return DarkDecline2_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(DarkDecline2_objectPrefab);
        newObj.SetActive(true);
        DarkDecline2_objectPool.Add(newObj);
        newObj.transform.parent = DarkDecline2_Parent;
        return newObj;
    }

    public GameObject GetDarkHandFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < DarkHand_objectPool.Count; i++)
        {
            if (!DarkHand_objectPool[i].activeInHierarchy)
            {
                DarkHand_objectPool[i].SetActive(true);
                return DarkHand_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(DarkHand_objectPrefab);
        newObj.SetActive(true);
        DarkHand_objectPool.Add(newObj);
        newObj.transform.parent = DarkHand_Parent;
        return newObj;
    }

    public GameObject GetDarkHand2FromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < DarkHand2_objectPool.Count; i++)
        {
            if (!DarkHand2_objectPool[i].activeInHierarchy)
            {
                DarkHand2_objectPool[i].SetActive(true);
                return DarkHand2_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(DarkHand2_objectPrefab);
        newObj.SetActive(true);
        DarkHand2_objectPool.Add(newObj);
        newObj.transform.parent = DarkHand2_Parent;
        return newObj;
    }
}
