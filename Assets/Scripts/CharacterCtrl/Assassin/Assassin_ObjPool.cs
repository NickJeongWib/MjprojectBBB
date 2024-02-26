using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_ObjPool : MonoBehaviour
{
    public GameObject shuriken_objectPrefab; // ������ �߻�ü
    
    public int poolSize; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> shuriken_objectPool; // ������Ʈ Ǯ

    [SerializeField]
    Transform shuriken_Parent;

    // Start is called before the first frame update
    void Start()
    {
        shuriken_objectPool = new List<GameObject>();

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj_1 = Instantiate(shuriken_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = shuriken_Parent;
            shuriken_objectPool.Add(obj_1);
        }
    }

    public GameObject ShurikenFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < shuriken_objectPool.Count; i++)
        {
            if (!shuriken_objectPool[i].activeInHierarchy)
            {
                shuriken_objectPool[i].SetActive(true);
                return shuriken_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(shuriken_objectPrefab);
        newObj.SetActive(true);
        shuriken_objectPool.Add(newObj);
        newObj.transform.parent = shuriken_Parent;
        return newObj;
    }
}
