using UnityEngine;

namespace Game.Gameplay
{
    // 죽지 않는 훈련용 표적 - 누적 데미지와 DPS를 잰다
    [RequireComponent(typeof(Damageable))]
    public class TrainingDummy : MonoBehaviour
    {
        [SerializeField] float resetAfterIdle = 3f;   // 이 시간 동안 안 맞으면 초기화

        Damageable _damageable;
        int _totalDamage;
        int _hitCount;
        float _firstHitTime;
        float _lastHitTime;

        public int TotalDamage => _totalDamage;
        public int HitCount => _hitCount;
        public float Dps
        {
            get
            {
                float elapsed = _lastHitTime - _firstHitTime;
                if (elapsed <= 0.01f) return _totalDamage;
                return _totalDamage / elapsed;
            }
        }

        public System.Action Changed;

        void Awake() => _damageable = GetComponent<Damageable>();

        void OnEnable() => _damageable.Damaged += OnDamaged;
        void OnDisable() => _damageable.Damaged -= OnDamaged;

        void Update()
        {
            if (_hitCount > 0 && Time.time - _lastHitTime > resetAfterIdle)
                ResetStats();
        }

        void OnDamaged()
        {
            // 체력을 계속 채워 죽지 않게 한다
            int before = _damageable.CurrentHp;
            _damageable.SetMaxHp(_damageable.MaxHp, healToFull: true);

            int damage = _damageable.MaxHp - before;

            if (_hitCount == 0) _firstHitTime = Time.time;
            _lastHitTime = Time.time;

            _totalDamage += damage;
            _hitCount++;

            Changed?.Invoke();
        }

        public void ResetStats()
        {
            _totalDamage = 0;
            _hitCount = 0;
            Changed?.Invoke();
        }
    }
}