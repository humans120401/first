using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Gameplay
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] Transform attackPoint;
        [SerializeField] float attackRange = 0.8f;
        [SerializeField] int damage = 10;
        [SerializeField] float cooldown = 0.35f;
        [SerializeField] LayerMask enemyLayer;

        PlayerControls _controls;
        float _cooldownTimer;

        void Awake() => _controls = new PlayerControls();

        void OnEnable()
        {
            _controls.Player.Enable();
            _controls.Player.Attack.started += OnAttack;
        }

        void OnDisable()
        {
            _controls.Player.Attack.started -= OnAttack;
            _controls.Player.Disable();
        }

        void Update() => _cooldownTimer -= Time.deltaTime;

        void OnAttack(InputAction.CallbackContext ctx)
        {
            if (_cooldownTimer > 0f) return;
            _cooldownTimer = cooldown;

            var hits = Physics2D.OverlapCircleAll(
                attackPoint.position, attackRange, enemyLayer);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Damageable>(out var target))
                    target.TakeDamage(damage);
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