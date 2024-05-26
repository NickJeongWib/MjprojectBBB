using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treant_ObjPool : MonoBehaviour
{
    public GameObject LeafPlace_objectPrefab; // LeafPlace
    public GameObject LeafPlace_Guide_objectPrefab; 

    public int poolSize; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> LeafPlace_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> LeafPlace_Guide_objectPool; // ������Ʈ Ǯ

    [SerializeField]
    Transform LeafPlace_Parent;
    [SerializeField]
    Transform LeafPlace_Guide_Parent;
    // Start is called before the first frame update
    void Start()
    {
        LeafPlace_objectPool = new List<GameObject>();
        LeafPlace_Guide_objectPool = new List<GameObject>();

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
}