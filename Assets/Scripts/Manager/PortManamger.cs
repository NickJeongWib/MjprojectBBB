using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortManager : MonoBehaviour
{
    [System.Serializable]
    public class BossPortalPair
    {
        public GameObject boss;
        public GameObject portal;
    }

    public List<BossPortalPair> bossPortalPairs;

    void Start()
    {
        // ���� ���� �� ��� ��Ż ��Ȱ��ȭ
        foreach (var pair in bossPortalPairs)
        {
            pair.portal.SetActive(false);
        }
    }

    void Update()
    {
        foreach (var pair in bossPortalPairs)
        {
            if (pair.boss == null || pair.portal == null)
            {
                Debug.LogError("PortManager: BossPortalPair�� null ��ü�� �ֽ��ϴ�.");
                continue;
            }

            Boss_HP_Controller bossHpController = pair.boss.GetComponent<Boss_HP_Controller>();
            if (bossHpController == null)
            {
                Debug.LogError("PortManager: Boss_HP_Controller ������Ʈ�� ã�� �� �����ϴ�.");
                continue;
            }

            if (bossHpController.isDead && !pair.portal.activeInHierarchy)
            {
                // �ش� ������ �׾���, ��Ż�� ���� Ȱ��ȭ���� �ʾҴٸ� ��Ż Ȱ��ȭ
                pair.portal.SetActive(true);
            }
        }
    }
}

