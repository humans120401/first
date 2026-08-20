using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;
using Game.Data;
using Game.Progression;

namespace Game.Presentation
{
    public class UpgradeUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] GameObject panel;
        [SerializeField] UpgradeTables tables;

        [Header("Cards")]
        [SerializeField] UpgradeCardUI[] cards = new UpgradeCardUI[3];

        [Header("Buttons")]
        [SerializeField] Button drawButton;
        [SerializeField] Button rerollButton;
        [SerializeField] Button closeButton;

        [Header("Texts")]
        [SerializeField] TextMeshProUGUI currencyText;
        [SerializeField] TextMeshProUGUI statsText;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] TextMeshProUGUI drawButtonLabel;
        [SerializeField] TextMeshProUGUI rerollButtonLabel;

        UpgradeSession _session;

        void Awake()
        {
            if (panel != null) panel.SetActive(false);

            var rng = new UnityRandomProvider();
            var generator = new UpgradeOptionGenerator(rng, tables);
            _session = new UpgradeSession(generator, PlayerStats.Current);

            _session.OptionsChanged += RefreshCards;
            _session.OptionApplied += OnApplied;
            _session.Rejected += ShowMessage;

            if (drawButton != null) drawButton.onClick.AddListener(() => _session.Draw());
            if (rerollButton != null) rerollButton.onClick.AddListener(() => _session.Reroll());
            if (closeButton != null) closeButton.onClick.AddListener(Close);
        }

        void OnEnable() => GameEvents.OnUpgradeUIRequested += Open;
        void OnDisable() => GameEvents.OnUpgradeUIRequested -= Open;

        void Open()
        {
            if (panel != null) panel.SetActive(true);
            ShowMessage("");
            RefreshCards();
        }

        void Close()
        {
            if (panel != null) panel.SetActive(false);
        }

        void RefreshCards()
        {
            var options = _session.CurrentOptions;

            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] == null) continue;

                if (i < options.Count)
                    cards[i].Bind(i, options[i], OnCardClicked);
                else
                    cards[i].Hide();
            }

            bool hasOptions = options.Count > 0;
            if (drawButton != null) drawButton.gameObject.SetActive(!hasOptions);
            if (rerollButton != null) rerollButton.gameObject.SetActive(hasOptions);

            RefreshTexts();
        }

        void OnCardClicked(int index)
        {
            _session.Select(index);
        }

        void OnApplied(UpgradeOption option)
        {
            ShowMessage("적용되었습니다");
        }

        void RefreshTexts()
        {
            if (currencyText != null)
                currencyText.text = ProgressStore.Current.currency + "G";

            if (drawButtonLabel != null)
                drawButtonLabel.text = "강화 (" + _session.DrawCost + "G)";

            if (rerollButtonLabel != null)
                rerollButtonLabel.text = "리롤 (" + _session.NextRerollCost + "G)";

            if (statsText != null)
            {
                var s = PlayerStats.Current;
                statsText.text =
                    "공격력 " + s.Get(StatType.Attack).ToString("0.0") + "   " +
                    "체력 " + s.Get(StatType.MaxHp).ToString("0") + "   " +
                    "이속 " + s.Get(StatType.MoveSpeed).ToString("0.0") + "   " +
                    "공속 " + s.Get(StatType.AttackSpeed).ToString("0") + "%   " +
                    "쿨감 " + s.Get(StatType.CooldownRate).ToString("0") + "%";
            }
        }

        void ShowMessage(string msg)
        {
            if (messageText != null) messageText.text = msg;
        }
    }
}