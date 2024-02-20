using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_ObjPool : MonoBehaviour
{
    public GameObject DarkDecline_objectPrefab; // ����Ǽ��

    public int poolSize; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> DarkDecline_objectPool; // ������Ʈ Ǯ

    [SerializeField]
    Transform DarkDecline_Parent;

    // Start is called before the first frame update
    void Start()
    {
        DarkDecline_objectPool = new List<GameObject>();

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(DarkDecline_objectPrefab);
            obj.transform.Rotate(Vector3.zero);
            obj.SetActive(false);
            obj.transform.parent = DarkDecline_Parent;
            DarkDecline_objectPool.Add(obj);
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
        return newObj;
    }
}
