using UnityEngine;
using Game.Core;

namespace Game.Gameplay
{
    public class StageController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Damageable player;
        [SerializeField] Damageable[] enemies;

        [Header("Stage Info")]
        [SerializeField] int floorNumber = 1;   // 이 씬이 몇 층인지

        [Header("Timing")]
        [SerializeField] float resultDelay = 0.8f;

        int _aliveEnemies;
        int _timesHit;
        float _startTime;
        bool _finished;

        void Start()
        {
            _startTime = Time.time;

            if (player != null)
            {
                player.Died += OnPlayerDied;
                player.Damaged += OnPlayerDamaged;
            }

            _aliveEnemies = 0;
            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;
                _aliveEnemies++;
                enemy.Died += OnEnemyDied;
            }

            Debug.Log($"[Stage {floorNumber}] 시작 / 적 {_aliveEnemies}체");
        }

        void OnDestroy()
        {
            if (player != null)
            {
                player.Died -= OnPlayerDied;
                player.Damaged -= OnPlayerDamaged;
            }

            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;
                enemy.Died -= OnEnemyDied;
            }
        }

        void OnPlayerDamaged() => _timesHit++;

        void OnEnemyDied()
        {
            if (_finished) return;

            _aliveEnemies--;
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

        void FireCleared()
        {
            float elapsed = Time.time - _startTime;
            var result = new StageResult(floorNumber, elapsed, _timesHit);

            Debug.Log($"[Stage {floorNumber}] 클리어 / {elapsed:F1}초 / 피격 {_timesHit}회");
            GameEvents.RaiseStageCleared(result);
        }

        void FireDied() => GameEvents.RaisePlayerDied();
    }
}