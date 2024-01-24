using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP_Ctrl : MonoBehaviour
{
    [Header("--HP_Image--")]
    [SerializeField]
    Image CurBoss_HP_Img;
    [SerializeField]
    Image NextBoss_HP_Img;

    [Tooltip("One Line Hp Value")]
    [Header("--HP_Var--")]
    [SerializeField]
    int Boss_SingleBarHP;

    [Tooltip("Hp Value")]
    public int BossMax_HP;
    public int BossCur_HP;

    [Tooltip("Hp Colors")]
    public List<Color> HP_Colors;

    public Text HP_Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Refresh_BossHP();
    }

    public void Refresh_BossHP()
    {
        // ������ ���� ����
        CurBoss_HP_Img.rectTransform.sizeDelta =
            new Vector2(NextBoss_HP_Img.rectTransform.sizeDelta.x * GetHPRatioSingleBar(BossCur_HP),
            NextBoss_HP_Img.rectTransform.sizeDelta.y);

        // ���� ü�� �� ����
        CurBoss_HP_Img.color = GetColorByLayer(BossCur_HP);
        // ���� ü�¹� ������ ǥ��, 0���Ϸ� ������ �� ���������� ǥ��
        NextBoss_HP_Img.color = GetColorByLayer(BossCur_HP - Boss_SingleBarHP);

        HP_Text.text = "X " + (int)(BossCur_HP / Boss_SingleBarHP);
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
