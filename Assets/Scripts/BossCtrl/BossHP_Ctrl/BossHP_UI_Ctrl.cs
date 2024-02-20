using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP_UI_Ctrl : MonoBehaviour
{
    [Header("--HP_Image--")]
    [SerializeField]
    Image CurBoss_HP_Img;
    [SerializeField]
    Image NextBoss_HP_Img;
    [SerializeField]
    Image TakeDamage_Img;

    [Tooltip("One Line Hp Value")]
    [Header("--HP_Var--")]
    [SerializeField]
    int Boss_SingleBarHP;

    [Tooltip("Hp Value")]
    public int BossMax_HP;
    public int BossCur_HP;
    public int Before_Boss_HP;

    [SerializeField]
    int remain_BossHP_Line;

    [Tooltip("Hp Colors")]
    public List<Color> HP_Colors;

    public Text HP_Text;

    // Start is called before the first frame update
    void Start()
    {
        // Refresh_BossHP();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Refresh_BossHP()
    {
        // ������ ���� ����
        CurBoss_HP_Img.rectTransform.sizeDelta =
            new Vector2(NextBoss_HP_Img.rectTransform.sizeDelta.x * GetHPRatioSingleBar(BossCur_HP),
            NextBoss_HP_Img.rectTransform.sizeDelta.y);

        // ���� ü�� �� ����
        CurBoss_HP_Img.color = GetColorByLayer(BossCur_HP);
        StartCoroutine(TakeDamageBar_Refresh());

        // ���� ü�¹� ������ ǥ��, 0���Ϸ� ������ �� ���������� ǥ��
        NextBoss_HP_Img.color = GetColorByLayer(BossCur_HP - Boss_SingleBarHP);

        HP_Text.text = "X " + (int)(BossCur_HP / Boss_SingleBarHP);

        if (remain_BossHP_Line != (int)(BossCur_HP / Boss_SingleBarHP))
        {
            // ������ ���� ����
            TakeDamage_Img.rectTransform.sizeDelta =
                new Vector2(NextBoss_HP_Img.rectTransform.sizeDelta.x * GetHPRatioSingleBar(BossCur_HP),
                NextBoss_HP_Img.rectTransform.sizeDelta.y);
        }

        remain_BossHP_Line = (int)(BossCur_HP / Boss_SingleBarHP);
    }

    IEnumerator TakeDamageBar_Refresh()
    {
        yield return new WaitForSeconds(1.0f);

        // ������ ���� ����
        TakeDamage_Img.rectTransform.sizeDelta =
            new Vector2(NextBoss_HP_Img.rectTransform.sizeDelta.x * GetHPRatioSingleBar(BossCur_HP),
            NextBoss_HP_Img.rectTransform.sizeDelta.y);
    }

    float GetHPRatioSingleBar(int _targetHP)
    {
        float result_Ratio = 0.0f;

        // ���� HP�� 0���� Ŭ ��
        if (_targetHP > 0)
        {
            float divided = (float)_targetHP / Boss_SingleBarHP;

            // �ǳ��� ���� �� 0 or 1
            if (divided == (int)divided)
            {
                result_Ratio = 1;
            }
            else
            {
                float moduled = _targetHP % Boss_SingleBarHP;

                result_Ratio = moduled / Boss_SingleBarHP;
            }
        }
        else
        {
            result_Ratio = 0.0f;
        }

        return result_Ratio;
    }

    Color GetColorByLayer(int _targetHP)
    {
        // �⺻ �÷��� ������
        Color result = Color.black;

        // ������ ü���� �������� ���
        if (_targetHP > 0)
        {

            // ���� ü�¿��� ���� ü���� ������ ���� 
            float divided = (float)_targetHP / Boss_SingleBarHP;

            // ���� �κ��� �ε����� ������
            int index = (int)divided;

            if (divided == (int)divided)
            {
                index--;
            }

            result = HP_Colors[index % HP_Colors.Count];
        }

        return result;
    }
}
