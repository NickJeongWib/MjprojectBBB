using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_ObjPool : MonoBehaviour
{
    public GameObject shuriken_objectPrefab_Q; // ������ �߻�ü
    public GameObject shuriken_objectPrefab_W; // ������ �߻�ü
    public GameObject mouseMoveEffect;         // ���콺 Ŭ�� ������Ʈ

    public int poolSize; // Ǯ ũ��
    [SerializeField]
    private List<GameObject> shuriken_objectPool_Q; // ������Ʈ Ǯ
    [SerializeField]
    public List<GameObject> shuriken_objectPool_W; // ������Ʈ Ǯ
    [SerializeField]
    public List<GameObject> mouseMoveEffect_objectPool; // ������Ʈ Ǯ

    [SerializeField]
    Transform shuriken_Parent_Q;
    [SerializeField]
    public Transform shuriken_Parent_W;
    [SerializeField]
    Transform mouseMoveEffect_Parent;

    // Start is called before the first frame update
    void Start()
    {
        shuriken_objectPool_Q = new List<GameObject>();
        shuriken_objectPool_W = new List<GameObject>();
        mouseMoveEffect_objectPool = new List<GameObject>();

        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            // Q��ų ������ ����
            GameObject obj_1 = Instantiate(shuriken_objectPrefab_Q);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = shuriken_Parent_Q;
            shuriken_objectPool_Q.Add(obj_1);
            // W��ų ������ ����
            GameObject obj_2 = Instantiate(shuriken_objectPrefab_W);
            obj_2.transform.Rotate(Vector3.zero);
            obj_2.SetActive(false);
            obj_2.transform.parent = shuriken_Parent_W;
            shuriken_objectPool_W.Add(obj_2);
            // ���콺 Ŭ�� �̵� ����Ʈ
            GameObject obj_3 = Instantiate(mouseMoveEffect);
            obj_3.transform.Rotate(Vector3.zero);
            obj_3.SetActive(false);
            obj_3.transform.parent = mouseMoveEffect_Parent;
            mouseMoveEffect_objectPool.Add(obj_3);
        }
    }

    public GameObject ShurikenFromPool_Q()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < shuriken_objectPool_Q.Count; i++)
        {
            if (!shuriken_objectPool_Q[i].activeInHierarchy)
            {
                shuriken_objectPool_Q[i].SetActive(true);
                return shuriken_objectPool_Q[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(shuriken_objectPrefab_Q);
        newObj.SetActive(true);
        shuriken_objectPool_Q.Add(newObj);
        newObj.transform.parent = shuriken_Parent_Q;
        return newObj;
    }

    public GameObject ShurikenFromPool_W()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < shuriken_objectPool_W.Count; i++)
        {
            if (!shuriken_objectPool_W[i].activeInHierarchy)
            {
                shuriken_objectPool_W[i].SetActive(true);
                return shuriken_objectPool_W[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(shuriken_objectPrefab_W);
        newObj.SetActive(true);
        shuriken_objectPool_W.Add(newObj);
        newObj.transform.parent = shuriken_Parent_W;
        return newObj;
    }

    public GameObject MouseMoveEffectFromPool()
    {
        // ��Ȱ��ȭ�� ������Ʈ�� ã�� ��ȯ
        for (int i = 0; i < mouseMoveEffect_objectPool.Count; i++)
        {
            if (!mouseMoveEffect_objectPool[i].activeInHierarchy)
            {
                mouseMoveEffect_objectPool[i].SetActive(true);
                return mouseMoveEffect_objectPool[i];
            }
        }

        // ��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ���� �� ��ȯ
        GameObject newObj = Instantiate(mouseMoveEffect);
        newObj.SetActive(true);
        mouseMoveEffect_objectPool.Add(newObj);
        newObj.transform.parent = mouseMoveEffect_Parent;
        return newObj;
    }
}
