using System;
using Game.Core;
using Game.Data;

namespace Game.Progression
{
    // 클리어 이벤트를 듣고 재화를 지급한다
    public static class RewardService
    {
        // 지급된 금액을 UI에 알리기 위한 이벤트
        public static event Action<int> RewardGranted;

        public static void Initialize()
        {
            GameEvents.OnStageCleared -= OnCleared;   // 중복 구독 방지
            GameEvents.OnStageCleared += OnCleared;
        }

        static void OnCleared(StageResult result)
        {
            int reward = RewardCalculator.Calculate(result);
            ProgressStore.AddCurrency(reward);
            RewardGranted?.Invoke(reward);
        }
    }
}