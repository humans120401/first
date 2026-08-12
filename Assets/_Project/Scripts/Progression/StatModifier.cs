using Game.Core;

namespace Game.Progression
{
    // 능력치에 더해지는 보정값 하나
    // 원본은 건드리지 않고 이것들을 쌓아 계산한다
    [System.Serializable]
    public class StatModifier
    {
        public StatType Type;
        public float Value;   // 기본값 대비 비율 (0.05 = 기본값의 5%만큼 추가)

        public StatModifier(StatType type, float value)
        {
            Type = type;
            Value = value;
        }
    }
}