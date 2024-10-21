using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_ObjPool : MonoBehaviour
{
    public int poolSize; // Ǯ ũ��

    [Header("----WindAtk_---")]
    public GameObject WindAtk_objectPrefab;
    [SerializeField]
    private List<GameObject> WindAtk_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    Transform WindAtk_Parent;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            // LeafPlace ������Ʈ Ǯ ����
            GameObject obj_1 = Instantiate(WindAtk_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = WindAtk_Parent;
            WindAtk_objectPool.Add(obj_1);
        }
    }

    public GameObject GetWindAtkFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < WindAtk_objectPool.Count; i++)
        {
            if (!WindAtk_objectPool[i].activeInHierarchy)
            {
                WindAtk_objectPool[i].SetActive(true);
                return WindAtk_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(WindAtk_objectPrefab);
        newObj.SetActive(true);
        WindAtk_objectPool.Add(newObj);
        newObj.transform.parent = WindAtk_Parent;
        return newObj;
    }
}
