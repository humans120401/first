using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Gameplay
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] Transform attackPoint;
        [SerializeField] float attackRange = 0.8f;
        [SerializeField] float baseCooldown = 0.35f;
        [SerializeField] LayerMask enemyLayer;

        PlayerControls _controls;
        float _cooldownTimer;

        // 외부에서 주입받는 값
        int _damage = 10;
        float _cooldownMultiplier = 1f;

        public void SetDamage(int value) => _damage = value;
        public void SetAttackSpeed(float percent)
        {
            if (percent <= 0f) percent = 100f;
            _cooldownMultiplier = 100f / percent;
        }

        void Awake() => _controls = new PlayerControls();

        void OnEnable()
        {
            if (_controls == null) _controls = new PlayerControls();

            _controls.Player.Enable();
            _controls.Player.Attack.started += OnAttack;
        }

        void OnDisable()
        {
            if (_controls == null) return;

            _controls.Player.Attack.started -= OnAttack;
            _controls.Player.Disable();
        }

        void Update() => _cooldownTimer -= Time.deltaTime;

        void OnAttack(InputAction.CallbackContext ctx)
        {
            if (_cooldownTimer > 0f) return;
            _cooldownTimer = baseCooldown * _cooldownMultiplier;

            var hits = Physics2D.OverlapCircleAll(
                attackPoint.position, attackRange, enemyLayer);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Damageable>(out var target))
                    target.TakeDamage(_damage);
            }
        }

        void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}