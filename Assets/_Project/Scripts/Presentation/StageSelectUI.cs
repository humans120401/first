using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;
using Game.Data;

namespace Game.Presentation
{
    public class StageSelectUI : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] GameObject panel;
        [SerializeField] Button closeButton;

        [Header("Floor Buttons")]
        [SerializeField] Button[] floorButtons = new Button[5];

        [Header("Currency")]
        [SerializeField] TextMeshProUGUI currencyText;

        [Header("Debug")]
        [SerializeField] bool unlockAllFloors = true;

        void Awake()
        {
            if (panel != null) panel.SetActive(false);

            for (int i = 0; i < floorButtons.Length; i++)
            {
                if (floorButtons[i] == null) continue;

                int floor = i + 1;
                floorButtons[i].onClick.AddListener(() => GameEvents.RequestStage(floor));
            }

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        void OnEnable()
        {
            GameEvents.OnStageSelectUIRequested += Open;
        }

        void OnDisable()
        {
            GameEvents.OnStageSelectUIRequested -= Open;
        }

        void Open()
        {
            if (panel != null) panel.SetActive(true);
            RefreshLockState();
            RefreshCurrency();
        }

        void Close()
        {
            if (panel != null) panel.SetActive(false);
        }

        void RefreshLockState()
        {
            for (int i = 0; i < floorButtons.Length; i++)
            {
                if (floorButtons[i] == null) continue;

                int floor = i + 1;
                bool unlocked = unlockAllFloors
                    || ProgressStore.Current.IsFloorUnlocked(floor);

                floorButtons[i].interactable = unlocked;

                var label = floorButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (label != null)
                    label.text = unlocked ? floor + "Ãþ" : floor + "Ãþ (Àá±è)";
            }
        }

        void RefreshCurrency()
        {
            if (currencyText != null)
                currencyText.text = ProgressStore.Current.currency + "G";
        }
    }
}