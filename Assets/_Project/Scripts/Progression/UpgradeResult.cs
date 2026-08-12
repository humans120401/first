using Game.Core;

namespace Game.Progression
{
    public struct UpgradeResult
    {
        public UpgradeOutcome Outcome;
        public StatType Stat;
        public int CostPaid;
        public float Multiplier;

        public bool Succeeded =>
            Outcome == UpgradeOutcome.Normal || Outcome == UpgradeOutcome.Great;
    }
}