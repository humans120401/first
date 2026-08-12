using UnityEngine;
using Game.Core;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "UpgradeTable", menuName = "Game/Upgrade Table")]
    public class UpgradeTable : ScriptableObject, IUpgradeTable
    {
        [System.Serializable]
        public class TierEntry : IUpgradeTier
        {
            public RiskTier Tier;
            public int cost = 100;

            [Header("확률 (합이 1이 되도록)")]
            [Range(0f, 1f)] public float FailChance = 0.2f;
            [Range(0f, 1f)] public float GreatChance = 0.1f;

            [Header("배율")]
            public float NormalMultiplier = 1.05f;
            public float GreatMultiplier = 1.20f;

            public int Cost => cost;

            public UpgradeOutcome Resolve(float roll)
            {
                if (roll < FailChance) return UpgradeOutcome.Fail;
                if (roll < FailChance + GreatChance) return UpgradeOutcome.Great;
                return UpgradeOutcome.Normal;
            }

            public float GetMultiplier(UpgradeOutcome outcome)
            {
                return outcome switch
                {
                    UpgradeOutcome.Normal => NormalMultiplier,
                    UpgradeOutcome.Great => GreatMultiplier,
                    _ => 0f          // 1f → 0f
                };
            }
        }

        public TierEntry lowRisk = new TierEntry
        {
            Tier = RiskTier.Low,
            cost = 100,
            FailChance = 0.1f,
            GreatChance = 0.05f,
            NormalMultiplier = 1.05f,
            GreatMultiplier = 1.12f
        };

        public TierEntry highRisk = new TierEntry
        {
            Tier = RiskTier.High,
            cost = 250,
            FailChance = 0.4f,
            GreatChance = 0.25f,
            NormalMultiplier = 1.10f,
            GreatMultiplier = 1.35f
        };

        public IUpgradeTier Get(RiskTier tier)
            => tier == RiskTier.Low ? lowRisk : highRisk;
    }
}