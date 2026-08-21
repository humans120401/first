using UnityEngine;
using Game.Core;
using Game.Gameplay;
using Game.Progression;

namespace Game.Bootstrap
{
    // 강화된 능력치를 플레이어 컴포넌트에 적용한다
    public class PlayerStatApplier : MonoBehaviour
    {
        Damageable _damageable;
        PlayerController _controller;
        PlayerAttack _attack;

        void Awake()
        {
            _damageable = GetComponent<Damageable>();
            _controller = GetComponent<PlayerController>();
            _attack = GetComponent<PlayerAttack>();
        }

        void Start() => Apply();

        public void Apply()
        {
            var stats = PlayerStats.Current;

            int maxHp = Mathf.RoundToInt(stats.Get(StatType.MaxHp));
            float moveSpeed = stats.Get(StatType.MoveSpeed);
            int damage = Mathf.RoundToInt(stats.Get(StatType.Attack));
            float attackSpeed = stats.Get(StatType.AttackSpeed);

            if (_damageable != null) _damageable.SetMaxHp(maxHp);
            if (_controller != null) _controller.SetMoveSpeed(moveSpeed);
            if (_attack != null)
            {
                _attack.SetDamage(damage);
                _attack.SetAttackSpeed(attackSpeed);
            }

            Debug.Log("[Stats] 체력 " + maxHp
                    + " / 공격력 " + damage
                    + " / 이속 " + moveSpeed.ToString("0.0")
                    + " / 공속 " + attackSpeed.ToString("0") + "%");
        }
    }
}