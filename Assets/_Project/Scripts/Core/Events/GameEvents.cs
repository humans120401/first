using System;

namespace Game.Core
{
    public static class GameEvents
    {
        // 게임 상태 변경
        public static event Action<GameState, GameState> OnStateChanged;
        public static void RaiseStateChanged(GameState prev, GameState next)
            => OnStateChanged?.Invoke(prev, next);

        // 상호작용 안내 표시 - Gameplay가 발행하고 Presentation이 듣는다
        public static event Action<string> OnPromptShown;
        public static event Action OnPromptHidden;

        public static void ShowPrompt(string text) => OnPromptShown?.Invoke(text);
        public static void HidePrompt() => OnPromptHidden?.Invoke();

        // UI 열기 요청
        public static event Action OnUpgradeUIRequested;
        public static event Action OnStageSelectUIRequested;

        public static void RequestUpgradeUI() => OnUpgradeUIRequested?.Invoke();
        public static void RequestStageSelectUI() => OnStageSelectUIRequested?.Invoke();

        // 씬 이동 요청 - UI가 발행하고 Bootstrap이 처리한다
        public static event Action<int> OnStageRequested;
        public static event Action OnLobbyRequested;
        public static event Action OnRetryRequested;

        public static void RequestStage(int floor) => OnStageRequested?.Invoke(floor);
        public static void RequestLobby() => OnLobbyRequested?.Invoke();
        public static void RequestRetry() => OnRetryRequested?.Invoke();

        // 스테이지 결과
        public static event Action<StageResult> OnStageCleared;
        public static event Action OnPlayerDied;

        public static void RaiseStageCleared(StageResult result) => OnStageCleared?.Invoke(result);
        public static void RaisePlayerDied() => OnPlayerDied?.Invoke();
    }
}