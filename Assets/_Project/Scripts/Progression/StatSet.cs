using System.Collections.Generic;
using Game.Core;

namespace Game.Progression
{
    // 기본값과 보정값들을 합쳐 최종 능력치를 계산한다
    public class StatSet
    {
        readonly Dictionary<StatType, float> _baseValues = new();
        readonly List<StatModifier> _modifiers = new();

        public StatSet(float baseAttack, float baseMaxHp, float baseMoveSpeed,
                       float baseAttackSpeed, float baseCooldownRate)
        {
            _baseValues[StatType.Attack] = baseAttack;
            _baseValues[StatType.MaxHp] = baseMaxHp;
            _baseValues[StatType.MoveSpeed] = baseMoveSpeed;
            _baseValues[StatType.AttackSpeed] = baseAttackSpeed;
            _baseValues[StatType.CooldownRate] = baseCooldownRate;
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void ClearModifiers()
        {
            _modifiers.Clear();
        }

        public float Get(StatType type)
        {
            float baseValue = _baseValues.TryGetValue(type, out var b) ? b : 0f;

            float bonus = 0f;
            foreach (var mod in _modifiers)
            {
                if (mod.Type == type)
                    bonus += mod.Value;
            }

            return baseValue + bonus;
        }

        // 몇 번 강화했는지 - UI 표시용
        public int GetUpgradeCount(StatType type)
        {
            int count = 0;
            foreach (var mod in _modifiers)
            {
                if (mod.Type == type) count++;
            }
            return count;
        }
    }
}