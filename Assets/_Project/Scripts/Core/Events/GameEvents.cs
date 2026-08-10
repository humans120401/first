using System;

namespace Game.Core
{
    public static class GameEvents
    {
        // 게임 상태 변경
        public static event Action<GameState, GameState> OnStateChanged;
        public static void RaiseStateChanged(GameState prev, GameState next)
            => OnStateChanged?.Invoke(prev, next);

        // 씬 이동 요청 - UI가 발행하고 Bootstrap이 처리한다
        public static event Action<int> OnStageRequested;
        public static event Action OnLobbyRequested;
        public static event Action OnRetryRequested;

        public static void RequestStage(int floor) => OnStageRequested?.Invoke(floor);
        public static void RequestLobby() => OnLobbyRequested?.Invoke();
        public static void RequestRetry() => OnRetryRequested?.Invoke();

        // 스테이지 결과 - Gameplay가 발행하고 Presentation과 Bootstrap이 듣는다
        public static event Action OnStageCleared;
        public static event Action OnPlayerDied;

        public static void RaiseStageCleared() => OnStageCleared?.Invoke();
        public static void RaisePlayerDied() => OnPlayerDied?.Invoke();
    }
}