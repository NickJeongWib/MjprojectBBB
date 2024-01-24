using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPtest : MonoBehaviour
{
    [Header("�� ü��")]
    public int maxHealth;
    public int curHealth;

    public AudioSource deathSoundSource; // ���� ��� ���带 ����� AudioSource ������Ʈ�� �����ϴ� ����

    public MeshRenderer[] meshs;

    public Slider[] healthBars; // ü�� �ٵ��� �����ϴ� ����
    public int[] virtualHealths; // ���� ü�µ��� �����ϴ� �迭

    public BossSkillP bossSkillP; // BossSkillP ��ũ��Ʈ�� �����ϴ� ����
    public Canvas healthBarCanvas; // ü�� ���� �θ� ĵ������ �����ϴ� ����

    public Animator bossAnimator; // ������ �ִϸ����͸� �����ϴ� ����
    public GameObject deathPopup; // ��� �� ��� �˾��� �����ϴ� ����

    public bool isDead; // ������ �׾����� ���θ� ��Ÿ���� ����
    // Start is called before the first frame update
    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        healthBarCanvas = healthBars[healthBars.Length - 1].transform.parent.GetComponent<Canvas>();

        // ���� ü�µ��� �ʱ�ȭ
        virtualHealths = new int[healthBars.Length - 1];
        for (int i = 0; i < virtualHealths.Length; i++)
        {
            virtualHealths[i] = 1000;
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
                    
                    // TODO ## ���� ���� ü�� HP�ٿ� �����ϴ� �κ�
                    this.GetComponent<BossLookAt>().bossHPCanvas.GetComponent<BossHP_Ctrl>().BossCur_HP = curHealth;
                }

                // ü���� ������ �Ŀ� ü�¹ٸ� ����
                UpdateHealthBars();

                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = Color.red;

                StartCoroutine(ResetColorAfterDelay(0.5f));

                if (curHealth <= 0)
                {
                    curHealth = 0; // curHealth ���� 0���� ����
                    isDead = true; // isDead ���� true�� ����

                    // �������� ��� �ִϸ��̼� ����
                    int randomDieAnimation = Random.Range(1, 3); // 1 �Ǵ� 2�� ���� �������� ���õ˴ϴ�.
                    bossAnimator.SetTrigger("doDie" + randomDieAnimation); // ���õ� �ִϸ��̼��� ����մϴ�.

                    // �˾��� Ȱ��ȭ�ϰ� �ִϸ��̼� �Ŀ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ�� �����մϴ�.
                    StartCoroutine(ShowPopupAndDisableBossAfterDelay(5f));
                }
            }
        }
    }

    IEnumerator ShowPopupAndDisableBossAfterDelay(float delay)
    {

        bossSkillP.isDead = true;
        yield return new WaitForSeconds(0.5f);

        // ������ �׾��� �� ���� ���
        if (deathSoundSource != null)
        {
            deathSoundSource.Play();
        }

        deathPopup.SetActive(true);
        StartCoroutine(ExpandPopupWidth(3f, 650f)); // �˾��� �ʺ� 1�� ���� 650���� Ȯ���ϴ� �ڷ�ƾ�� �����մϴ�.

        yield return new WaitForSeconds(delay);

        // �˾��� õõ�� ������� ����� �ڷ�ƾ�� �����մϴ�.
        StartCoroutine(FadeOutPopup(1f));

        // ü�¹ٸ� ��Ȱ��ȭ�ϴ� ���
        GameObject healthBar = GameObject.Find("BossHealthO"); // ü�¹��� �̸��� ����Ͽ� ã���ϴ�.
        if (healthBar != null)
        {
            healthBar.SetActive(false); // ü�¹ٸ� ��Ȱ��ȭ�մϴ�.
        }
        // ü�¾����ܸ� ��Ȱ��ȭ�ϴ� ���
        GameObject healthIcon = GameObject.Find("hpicon"); // ü�¹��� �̸��� ����Ͽ� ã���ϴ�.
        if (healthIcon != null)
        {
            healthIcon.SetActive(false); // ü�¹ٸ� ��Ȱ��ȭ�մϴ�.
        }


        yield return new WaitForSeconds(3f); // ü�¹� ĵ������ ������ ����� �Ŀ� ������ ��Ȱ��ȭ�ϱ� ���� 0.5�� ��ٸ��ϴ�.

        // ������ ��Ȱ��ȭ�մϴ�.
        this.gameObject.SetActive(false);
    }

    IEnumerator FadeOutPopup(float duration)
    {
        CanvasGroup canvasGroup = deathPopup.GetComponent<CanvasGroup>();
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        canvasGroup.alpha = 0; // �˾��� ������ �����ϰ� ����ϴ�.

        yield return new WaitForSeconds(2f);
        deathPopup.SetActive(false); // �˾��� ��Ȱ��ȭ�մϴ�.
    }

    IEnumerator ExpandPopupWidth(float duration, float targetWidth)
    {
        RectTransform rectTransform = deathPopup.GetComponent<RectTransform>();
        float startWidth = rectTransform.rect.width;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            rectTransform.sizeDelta = new Vector2(Mathf.Lerp(startWidth, targetWidth, t), rectTransform.sizeDelta.y);
            yield return null;
        }

        rectTransform.sizeDelta = new Vector2(targetWidth, rectTransform.sizeDelta.y); // �˾��� �ʺ� ��ǥġ�� �����մϴ�.
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

        if (isDead) return; // ������ �׾��ٸ� �� �޼ҵ带 �� �̻� �������� �ʽ��ϴ�.

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