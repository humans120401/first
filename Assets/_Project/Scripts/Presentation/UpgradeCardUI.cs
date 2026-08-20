using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;

namespace Game.Presentation
{
    // 강화 선택지 카드 한 장
    public class UpgradeCardUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] Button button;
        [SerializeField] Image background;
        [SerializeField] TextMeshProUGUI rarityText;
        [SerializeField] TextMeshProUGUI statText;
        [SerializeField] TextMeshProUGUI valueText;

        int _index;
        Action<int> _onClick;

        void Awake()
        {
            if (button != null)
                button.onClick.AddListener(() => _onClick?.Invoke(_index));
        }

        public void Bind(int index, UpgradeOption option, Action<int> onClick)
        {
            _index = index;
            _onClick = onClick;

            if (rarityText != null) rarityText.text = RarityLabel(option.Rarity);
            if (statText != null) statText.text = StatLabel(option.Stat);

            if (valueText != null)
            {
                string sign = option.Value >= 0 ? "+" : "";
                valueText.text = sign + option.Value.ToString("0.0");
                valueText.color = option.Value >= 0 ? Color.white : new Color(1f, 0.4f, 0.4f);
            }

            if (background != null)
                background.color = RarityColor(option.Rarity);

            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        static string RarityLabel(UpgradeRarity r) => r switch
        {
            UpgradeRarity.Common => "일반",
            UpgradeRarity.Uncommon => "고급",
            UpgradeRarity.Rare => "희귀",
            UpgradeRarity.Epic => "영웅",
            UpgradeRarity.Legendary => "전설",
            UpgradeRarity.Transcend => "초월",
            UpgradeRarity.Chaos => "혼돈",
            _ => "?"
        };

        static Color RarityColor(UpgradeRarity r) => r switch
        {
            UpgradeRarity.Common => new Color(0.45f, 0.45f, 0.45f),
            UpgradeRarity.Uncommon => new Color(0.25f, 0.55f, 0.30f),
            UpgradeRarity.Rare => new Color(0.20f, 0.40f, 0.70f),
            UpgradeRarity.Epic => new Color(0.50f, 0.25f, 0.65f),
            UpgradeRarity.Legendary => new Color(0.75f, 0.55f, 0.15f),
            UpgradeRarity.Transcend => new Color(0.85f, 0.30f, 0.30f),
            UpgradeRarity.Chaos => new Color(0.15f, 0.15f, 0.15f),
            _ => Color.gray
        };

        static string StatLabel(StatType s) => s switch
        {
            StatType.Attack => "공격력",
            StatType.MaxHp => "최대 체력",
            StatType.MoveSpeed => "이동 속도",
            StatType.AttackSpeed => "공격 속도",
            StatType.CooldownRate => "쿨타임 감소",
            _ => "?"
        };
    }
}