using System;

namespace Game.Core
{
    public static class GameEvents
    {
        public static event Action<GameState, GameState> OnStateChanged;

        public static void RaiseStateChanged(GameState prev, GameState next)
            => OnStateChanged?.Invoke(prev, next);
    }
}