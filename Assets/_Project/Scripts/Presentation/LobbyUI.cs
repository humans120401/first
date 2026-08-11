using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;
using Game.Data;

namespace Game.Presentation
{
    public class LobbyUI : MonoBehaviour
    {
        [Header("Floor Buttons")]
        [SerializeField] Button[] floorButtons = new Button[5];

        [Header("Currency")]                                    // 추가
        [SerializeField] TextMeshProUGUI currencyText;           // 추가

        [Header("Debug")]
        [SerializeField] bool unlockAllFloors = true;   // 개발 중 전체 개방

        void Start()
        {
            for (int i = 0; i < floorButtons.Length; i++)
            {
                if (floorButtons[i] == null)
                {
                    Debug.LogError($"[LobbyUI] 버튼 {i}번이 연결되지 않았습니다");
                    continue;
                }

                int floor = i + 1;
                floorButtons[i].onClick.AddListener(() =>
                {
                    Debug.Log($"[LobbyUI] {floor}층 버튼 클릭됨");
                    GameEvents.RequestStage(floor);
                });
            }

            RefreshLockState();
            RefreshCurrency();                                  // 추가
            Debug.Log("[LobbyUI] 초기화 완료");
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
                    label.text = unlocked ? $"{floor}층" : $"{floor}층 (잠김)";
            }
        }

        // 추가된 메서드
        void RefreshCurrency()
        {
            Debug.Log($"[Lobby] 보유 재화 {ProgressStore.Current.currency}G");
            if (currencyText != null)
                currencyText.text = $"{ProgressStore.Current.currency}G";
        }
    }
}