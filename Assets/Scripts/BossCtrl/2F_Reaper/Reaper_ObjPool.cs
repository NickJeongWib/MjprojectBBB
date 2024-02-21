using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_ObjPool : MonoBehaviour
{
    public GameObject DarkDecline_objectPrefab; // ����Ǽ��
    public GameObject DarkHand_objectPrefab; // ����Ǽ���
    public GameObject DarkHand2_objectPrefab; // ����Ǽ���2

    public int poolSize; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> DarkDecline_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> DarkHand_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> DarkHand2_objectPool; // ������Ʈ Ǯ


    [SerializeField]
    Transform DarkDecline_Parent;
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
            GameObject obj = Instantiate(DarkDecline_objectPrefab);
            obj.transform.Rotate(Vector3.zero);
            obj.SetActive(false);
            obj.transform.parent = DarkDecline_Parent;
            DarkDecline_objectPool.Add(obj);
        }

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(DarkHand_objectPrefab);
            obj.transform.Rotate(Vector3.zero);
            obj.SetActive(false);
            obj.transform.parent = DarkHand_Parent;
            DarkHand_objectPool.Add(obj);
        }

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(DarkHand2_objectPrefab);
            obj.transform.Rotate(Vector3.zero);
            obj.SetActive(false);
            obj.transform.parent = DarkHand2_Parent;
            DarkHand2_objectPool.Add(obj);
        }
    }

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
