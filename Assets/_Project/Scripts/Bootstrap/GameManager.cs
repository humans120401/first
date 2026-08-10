using UnityEngine;
using Game.Core;

namespace Game.Bootstrap
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState Current { get; private set; } = GameState.Boot;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            ChangeState(GameState.MainMenu);
        }

        public void ChangeState(GameState next)
        {
            if (Current == next) return;

            var prev = Current;
            Current = next;
            Debug.Log($"[GameManager] {prev} ¡æ {next}");
            GameEvents.RaiseStateChanged(prev, next);
        }
    }
}