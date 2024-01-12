using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGuard_ObjPool : MonoBehaviour
{
    public GameObject Down_3Combo_objectPrefab; // 9�� ��� ������
    public GameObject Razer_objectPrefab; // ������ ������

    public int poolSize = 10; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> Down_3CombpVFX_objectPool; // ������Ʈ Ǯ
    
    [SerializeField]
    Transform DownEffect_Parent;
    [SerializeField]
    Transform Razer_Parent;

    private void Start()
    {
        Down_3CombpVFX_objectPool = new List<GameObject>();

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(Down_3Combo_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = DownEffect_Parent;
            Down_3CombpVFX_objectPool.Add(obj);
        }
    }

    // ������Ʈ�� Ǯ���� �������� �޼���
    public GameObject GetObjectFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < Down_3CombpVFX_objectPool.Count; i++)
        {
            if (!Down_3CombpVFX_objectPool[i].activeInHierarchy)
            {
                Down_3CombpVFX_objectPool[i].SetActive(true);
                return Down_3CombpVFX_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(Down_3Combo_objectPrefab);
        newObj.SetActive(true);
        Down_3CombpVFX_objectPool.Add(newObj);
        return newObj;
    }

    // ������Ʈ�� Ǯ�� ��ȯ�ϴ� �޼���
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
