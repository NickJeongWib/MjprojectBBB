using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPtest : MonoBehaviour
{
    [Header("�� ü��")]
    public int maxHealth;
    public int curHealth;

    public MeshRenderer[] meshs;

    public Slider[] healthBars; // ü�� �ٵ��� �����ϴ� ����
    public int[] virtualHealths; // ���� ü�µ��� �����ϴ� �迭

    public BossSkillP bossSkillP; // BossSkillP ��ũ��Ʈ�� �����ϴ� ����
    public Canvas healthBarCanvas; // ü�� ���� �θ� ĵ������ �����ϴ� ����

    // Start is called before the first frame update
    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        healthBarCanvas = healthBars[0].transform.parent.GetComponent<Canvas>();

        // ���� ü�µ��� �ʱ�ȭ
        virtualHealths = new int[healthBars.Length - 1];
        for (int i = 0; i < virtualHealths.Length; i++)
        {
            virtualHealths[i] = 8000;
            healthBars[i].maxValue = virtualHealths[i]; // �����̴��� �ִ� ���� ���� ü���� �ִ� ������ ����
        }

        // �������� ü�� ���� �ִ� ���� maxHealth�� ����
        healthBars[healthBars.Length - 1].maxValue = maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (this.gameObject.CompareTag("Enemy"))
            {
                Attack weapon = other.GetComponent<Attack>();
                Debug.Log("Damage: " + weapon.damage);

                int remainingDamage = weapon.damage; // ���� ���������� ����ϱ� ���� ����

                // ���� ü�� ����
                for (int i = 0; i < virtualHealths.Length; i++)
                {
                    if (virtualHealths[i] > 0)
                    {
                        int actualDamage = Mathf.Min(virtualHealths[i], remainingDamage); // ������ ���� ü�¿��� ���ҽ�ų ������ ���
                        virtualHealths[i] = Mathf.Max(virtualHealths[i] - actualDamage, 0);
                        remainingDamage = Mathf.Max(remainingDamage - actualDamage, 0); // ���� �������� ����

                        if (remainingDamage <= 0) // ��� �������� �����ߴٸ� ���� ����
                        {
                            break;
                        }
                    }
                }

                // ���� ü���� ��� �����Ǿ��� ���� ü�� ����
                if (remainingDamage > 0)
                {
                    curHealth = Mathf.Max(curHealth - remainingDamage, 0);
                }

                // ü���� ������ �Ŀ� ü�¹ٸ� ����
                UpdateHealthBars();

                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = Color.red;

                StartCoroutine(ResetColorAfterDelay(0.5f));

                if (curHealth <= 0)
                {
                    curHealth = 0; // curHealth ���� 0���� ����
                    bossSkillP.isDead = true; // isDead ���� true�� ����
                }
            }
        }
    }

    IEnumerator ResetColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
    }

    void UpdateHealthBars()
    {
        // �������� ü�� �� ������Ʈ
        healthBars[healthBars.Length - 1].value = curHealth;

        // ���� ü�� �ٵ� ������Ʈ
        for (int i = 0; i < healthBars.Length - 1; i++)
        {
            healthBars[i].value = virtualHealths[i];
            if (virtualHealths[i] <= 0)
            {
                healthBars[i].gameObject.SetActive(false); // ���� ü���� 0�� �Ǹ� �ش� ü�� �� ��Ȱ��ȭ
            }
        }

        if (curHealth <= 0)
        {
            healthBarCanvas.gameObject.SetActive(false);
        }
    }
}