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

        [Header("Debug")]
        [SerializeField] bool unlockAllFloors = true;   // 개발 중 전체 개방

        void Start()
        {
            for (int i = 0; i < floorButtons.Length; i++)
            {
                if (floorButtons[i] == null) continue;

                int floor = i + 1;   // 반복 변수를 직접 쓰면 모든 버튼이 마지막 값을 쓴다
                floorButtons[i].onClick.AddListener(() => GameEvents.RequestStage(floor));
            }

            RefreshLockState();
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
    }
}