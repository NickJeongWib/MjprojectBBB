using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGuard_ObjPool : MonoBehaviour
{
    public GameObject Down_3Combo_objectPrefab; // 9�� ��� ������
    public GameObject SpiritSword_objectPrefab; // �˱� ������
    public GameObject JumpAtk_objectPrefab; // ���� ���� ������

    public int poolSize = 5; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> Down_3CombpVFX_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> SpiritSword_objectPool; // ������Ʈ Ǯ
    [SerializeField]
    private List<GameObject> JumpAtk_objectPool; // ������Ʈ Ǯ

    [SerializeField]
    Transform DownEffect_Parent;
    [SerializeField]
    Transform Spirit_Parent;
    [SerializeField]
    Transform JumpAtk_Parent;

    private void Start()
    {
        Down_3CombpVFX_objectPool = new List<GameObject>();
        SpiritSword_objectPool = new List<GameObject>();
        JumpAtk_objectPool = new List<GameObject>();

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(Down_3Combo_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = DownEffect_Parent;
            Down_3CombpVFX_objectPool.Add(obj);
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(SpiritSword_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = Spirit_Parent;
            SpiritSword_objectPool.Add(obj);
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(JumpAtk_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = JumpAtk_Parent;
            JumpAtk_objectPool.Add(obj);
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

    public GameObject Get_SpiritSword_ObjectFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < SpiritSword_objectPool.Count; i++)
        {
            if (!SpiritSword_objectPool[i].activeInHierarchy)
            {
                SpiritSword_objectPool[i].SetActive(true);
                return SpiritSword_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(SpiritSword_objectPrefab);
        newObj.SetActive(true);
        SpiritSword_objectPool.Add(newObj);
        return newObj;
    }

    public GameObject Get_JumpAtk_ObjectFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < JumpAtk_objectPool.Count; i++)
        {
            if (!JumpAtk_objectPool[i].activeInHierarchy)
            {
                JumpAtk_objectPool[i].SetActive(true);
                return JumpAtk_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(JumpAtk_objectPrefab);
        newObj.SetActive(true);
        JumpAtk_objectPool.Add(newObj);
        return newObj;
    }

    // ������Ʈ�� Ǯ�� ��ȯ�ϴ� �޼���
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
