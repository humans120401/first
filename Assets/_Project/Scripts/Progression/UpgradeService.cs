using System;
using Game.Core;
using Game.Data;


namespace Game.Progression
{
    // 강화를 실행하는 곳
    // MonoBehaviour가 아니라서 유니티 없이 테스트할 수 있다
    public class UpgradeService
    {
        readonly IRandomProvider _rng;
        readonly IUpgradeTable _table;
        readonly StatSet _stats;

        public event Action<UpgradeResult> UpgradeResolved;

        public UpgradeService(IRandomProvider rng, IUpgradeTable table, StatSet stats)
        {                                    // 여기도 IUpgradeTable
            _rng = rng;
            _table = table;
            _stats = stats;
        }

        public UpgradeResult TryUpgrade(StatType stat, RiskTier tier)
        {
            var entry = _table.Get(tier);
            var result = new UpgradeResult { Stat = stat };

            // 재화 확인
            if (ProgressStore.Current.currency < entry.Cost)
            {
                result.Outcome = UpgradeOutcome.NotEnough;
                UpgradeResolved?.Invoke(result);
                return result;
            }

            // 재화 차감 - 실패해도 돌려주지 않는다
            ProgressStore.SpendCurrency(entry.Cost);
            result.CostPaid = entry.Cost;

            // 확률 판정
            float roll = _rng.Value01();
            result.Outcome = entry.Resolve(roll);
            result.Multiplier = entry.GetMultiplier(result.Outcome);

            // 성공했으면 능력치에 반영
            if (result.Succeeded)
                _stats.AddModifier(new StatModifier(stat, result.Multiplier));

            UpgradeResolved?.Invoke(result);
            return result;
        }
    }
}