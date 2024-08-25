using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_ObjPool : MonoBehaviour
{
    public GameObject mouseMoveEffect;         // ���콺 Ŭ�� ������Ʈ

    public int poolSize; // Ǯ ũ��

    [SerializeField]
    public List<GameObject> mouseMoveEffect_objectPool; // ������Ʈ Ǯ


    [SerializeField]
    Transform mouseMoveEffect_Parent;

    // Start is called before the first frame update
    void Start()
    {
        mouseMoveEffect_objectPool = new List<GameObject>();
        // �ʱ⿡ Ǯ�� ������Ʈ�� �����Ͽ� ����
        for (int i = 0; i < poolSize; i++)
        {
            // ���콺 Ŭ�� �̵� ����Ʈ
            GameObject obj_3 = Instantiate(mouseMoveEffect);
            obj_3.transform.Rotate(Vector3.zero);
            obj_3.SetActive(false);
            obj_3.transform.parent = mouseMoveEffect_Parent;
            mouseMoveEffect_objectPool.Add(obj_3);
        }
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
