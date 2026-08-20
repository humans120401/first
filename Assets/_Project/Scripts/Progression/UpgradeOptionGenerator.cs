using System.Collections.Generic;
using Game.Core;

namespace Game.Progression
{
    // 강화 선택지 3개를 뽑는다
    public class UpgradeOptionGenerator
    {
        readonly IRandomProvider _rng;
        readonly IUpgradeTables _tables;

        public UpgradeOptionGenerator(IRandomProvider rng, IUpgradeTables tables)
        {
            _rng = rng;
            _tables = tables;
        }

        public List<UpgradeOption> Generate(int count = 3)
        {
            var result = new List<UpgradeOption>();
            var usedStats = new HashSet<StatType>();

            for (int i = 0; i < count; i++)
            {
                var option = GenerateOne(usedStats);
                result.Add(option);
                usedStats.Add(option.Stat);
            }

            return result;
        }

        UpgradeOption GenerateOne(HashSet<StatType> exclude)
        {
            var rarity = PickRarity();
            var stat = PickStat(exclude);

            var rarityInfo = FindRarity(rarity);
            var statInfo = FindStat(stat);

            // 등급 기준 수치를 뽑고 능력치 배율을 곱한다
            float t = _rng.Value01();
            float baseValue = rarityInfo.MinValue + (rarityInfo.MaxValue - rarityInfo.MinValue) * t;
            float value = baseValue * statInfo.Scale;

            bool isChaos = rarity == UpgradeRarity.Chaos;

            // 혼돈은 절반 확률로 음수가 된다
            if (isChaos && _rng.Value01() < 0.5f)
                value = -value;

            return new UpgradeOption(rarity, stat, value, isChaos);
        }

        UpgradeRarity PickRarity()
        {
            float total = 0f;
            foreach (var r in _tables.Rarities) total += r.Weight;

            float roll = _rng.Value01() * total;
            float acc = 0f;

            foreach (var r in _tables.Rarities)
            {
                acc += r.Weight;
                if (roll < acc) return r.Rarity;
            }

            return _tables.Rarities[0].Rarity;
        }

        StatType PickStat(HashSet<StatType> exclude)
        {
            // 제외 대상을 뺀 가중치 합
            float total = 0f;
            foreach (var s in _tables.Stats)
            {
                if (exclude.Contains(s.Stat)) continue;
                total += s.Weight;
            }

            // 전부 제외됐으면 제외를 무시한다
            if (total <= 0f)
            {
                foreach (var s in _tables.Stats) total += s.Weight;
                exclude = new HashSet<StatType>();
            }

            float roll = _rng.Value01() * total;
            float acc = 0f;

            foreach (var s in _tables.Stats)
            {
                if (exclude.Contains(s.Stat)) continue;
                acc += s.Weight;
                if (roll < acc) return s.Stat;
            }

            return _tables.Stats[0].Stat;
        }

        IRarityInfo FindRarity(UpgradeRarity rarity)
        {
            foreach (var r in _tables.Rarities)
                if (r.Rarity == rarity) return r;
            return _tables.Rarities[0];
        }

        IStatInfo FindStat(StatType stat)
        {
            foreach (var s in _tables.Stats)
                if (s.Stat == stat) return s;
            return _tables.Stats[0];
        }
    }
}