using UnityEngine;
using Game.Core;

namespace Game.Gameplay
{
    public class StageController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Damageable player;
        [SerializeField] Damageable[] enemies;

        [Header("Timing")]
        [SerializeField] float resultDelay = 0.8f;   // 결과 표시까지 여유

        int _aliveEnemies;
        bool _finished;

        void Start()
        {
            if (player != null)
                player.Died += OnPlayerDied;

            _aliveEnemies = 0;
            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;
                _aliveEnemies++;
                enemy.Died += OnEnemyDied;
            }

            Debug.Log($"[Stage] 시작 / 적 {_aliveEnemies}체");
        }

        void OnDestroy()
        {
            if (player != null)
                player.Died -= OnPlayerDied;

            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;
                enemy.Died -= OnEnemyDied;
            }
        }

        void OnEnemyDied()
        {
            if (_finished) return;

            _aliveEnemies--;
            Debug.Log($"[Stage] 적 사망 / 남은 적 {_aliveEnemies}체");

            if (_aliveEnemies <= 0)
            {
                _finished = true;
                Invoke(nameof(FireCleared), resultDelay);
            }
        }

        void OnPlayerDied()
        {
            if (_finished) return;

            _finished = true;
            Invoke(nameof(FireDied), resultDelay);
        }

        void FireCleared() => GameEvents.RaiseStageCleared();
        void FireDied() => GameEvents.RaisePlayerDied();
    }
}