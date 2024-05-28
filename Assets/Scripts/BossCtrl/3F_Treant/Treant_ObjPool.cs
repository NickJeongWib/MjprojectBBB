using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treant_ObjPool : MonoBehaviour
{
    public GameObject LeafPlace_objectPrefab; // LeafPlace
    public GameObject LeafPlace_Guide_objectPrefab;

    public GameObject Golem_objectPrefab;
    public GameObject Stone_Crash_objectPrefab;

    public GameObject Stone_Guide_objectPrefab;

    public int poolSize; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> LeafPlace_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> LeafPlace_Guide_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> Golem_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> Stone_Crash_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> Stone_Guide_objectPool; // ������Ʈ Ǯ

    [SerializeField]
    Transform LeafPlace_Parent;
    [SerializeField]
    Transform LeafPlace_Guide_Parent;
    [SerializeField]
    Transform Golem_Parent;
    [SerializeField]
    Transform Stone_Crash_Parent;
    [SerializeField]
    Transform Stone_Guide_Parent;

    // Start is called before the first frame update
    void Start()
    {
        Golem_objectPool = new List<GameObject>();
        LeafPlace_objectPool = new List<GameObject>();
        LeafPlace_Guide_objectPool = new List<GameObject>();
        Stone_Crash_objectPool = new List<GameObject>();

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            // LeafPlace ������Ʈ Ǯ ����
            GameObject obj_1 = Instantiate(LeafPlace_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = LeafPlace_Parent;
            LeafPlace_objectPool.Add(obj_1);

            // LeafPlace_Guide ������Ʈ Ǯ ����
            GameObject obj_2 = Instantiate(LeafPlace_Guide_objectPrefab);
            obj_2.transform.Rotate(Vector3.zero);
            obj_2.SetActive(false);
            obj_2.transform.parent = LeafPlace_Guide_Parent;
            LeafPlace_Guide_objectPool.Add(obj_2);

            // Golem ������Ʈ Ǯ ����
            GameObject obj_3 = Instantiate(Golem_objectPrefab);
            obj_3.transform.Rotate(Vector3.zero);
            obj_3.SetActive(false);
            obj_3.transform.parent = Golem_Parent;
            Golem_objectPool.Add(obj_3);

            // ����ũ���� ������Ʈ Ǯ ����
            GameObject obj_4 = Instantiate(Stone_Crash_objectPrefab);
            obj_4.transform.Rotate(Vector3.zero);
            obj_4.SetActive(false);
            obj_4.transform.parent = Stone_Crash_Parent;
            Stone_Crash_objectPool.Add(obj_4);

            // ����ũ���� ������Ʈ Ǯ ����
            GameObject obj_5 = Instantiate(Stone_Guide_objectPrefab);
            obj_5.transform.Rotate(Vector3.zero);
            obj_5.SetActive(false);
            obj_5.transform.parent = Stone_Guide_Parent;
            Stone_Guide_objectPool.Add(obj_5);
        }
    }

    public GameObject GetLeafPlaceFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < LeafPlace_objectPool.Count; i++)
        {
            if (!LeafPlace_objectPool[i].activeInHierarchy)
            {
                LeafPlace_objectPool[i].SetActive(true);
                return LeafPlace_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(LeafPlace_objectPrefab);
        newObj.SetActive(true);
        LeafPlace_objectPool.Add(newObj);
        newObj.transform.parent = LeafPlace_Parent;
        return newObj;
    }

    public GameObject GetLeafPlace_Guide_FromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < LeafPlace_Guide_objectPool.Count; i++)
        {
            if (!LeafPlace_Guide_objectPool[i].activeInHierarchy)
            {
                LeafPlace_Guide_objectPool[i].SetActive(true);
                return LeafPlace_Guide_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(LeafPlace_Guide_objectPrefab);
        newObj.SetActive(true);
        LeafPlace_Guide_objectPool.Add(newObj);
        newObj.transform.parent = LeafPlace_Guide_Parent;
        return newObj;
    }

    public GameObject GetGolem_FromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < Golem_objectPool.Count; i++)
        {
            if (!Golem_objectPool[i].activeInHierarchy)
            {
                Golem_objectPool[i].SetActive(true);
                return Golem_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(Golem_objectPrefab);
        newObj.SetActive(true);
        Golem_objectPool.Add(newObj);
        newObj.transform.parent = Golem_Parent;
        return newObj;
    }

    public GameObject GetStone_Crash_FromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < Stone_Crash_objectPool.Count; i++)
        {
            if (!Stone_Crash_objectPool[i].activeInHierarchy)
            {
                Stone_Crash_objectPool[i].SetActive(true);
                return Stone_Crash_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(Stone_Crash_objectPrefab);
        newObj.SetActive(true);
        Stone_Crash_objectPool.Add(newObj);
        newObj.transform.parent = Stone_Crash_Parent;
        return newObj;
    }

    public GameObject GetStone_Guide_FromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < Stone_Guide_objectPool.Count; i++)
        {
            if (!Stone_Guide_objectPool[i].activeInHierarchy)
            {
                Stone_Guide_objectPool[i].SetActive(true);
                return Stone_Guide_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(Stone_Guide_objectPrefab);
        newObj.SetActive(true);
        Stone_Guide_objectPool.Add(newObj);
        newObj.transform.parent = Stone_Guide_Parent;
        return newObj;
    }
}