namespace Game.Core
{
    // 강화 화면에 뜨는 선택지 하나
    public struct UpgradeOption
    {
        public UpgradeRarity Rarity;
        public StatType Stat;
        public float Value;        // 실제 상승 수치 (혼돈이면 음수 가능)
        public bool IsChaos;

        public UpgradeOption(UpgradeRarity rarity, StatType stat, float value, bool isChaos = false)
        {
            Rarity = rarity;
            Stat = stat;
            Value = value;
            IsChaos = isChaos;
        }
    }
}