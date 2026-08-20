using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Data;
using Game.Progression;

namespace Game.Bootstrap
{
    // 선택지 생성 검증용 - 나중에 삭제
    public class UpgradeSimulator : MonoBehaviour
    {
        [SerializeField] UpgradeTables tables;
        [SerializeField] int trials = 3000;
        [SerializeField] int seed = 12345;

        [ContextMenu("1. Show Sample Draws")]
        void ShowSamples()
        {
            if (!Ready()) return;

            var gen = new UpgradeOptionGenerator(new UnityRandomProvider(seed), tables);

            for (int i = 0; i < 5; i++)
            {
                var options = gen.Generate(3);
                string line = $"[{i + 1}회차] ";
                foreach (var o in options)
                    line += $"  [{Label(o.Rarity)}] {StatName(o.Stat)} {o.Value:+0.0;-0.0}  ";
                Debug.Log(line);
            }
        }

        [ContextMenu("2. Check Rarity Distribution")]
        void CheckDistribution()
        {
            if (!Ready()) return;

            var gen = new UpgradeOptionGenerator(new UnityRandomProvider(seed), tables);
            var count = new Dictionary<UpgradeRarity, int>();
            var statCount = new Dictionary<StatType, int>();

            int totalOptions = 0;

            for (int i = 0; i < trials; i++)
            {
                foreach (var o in gen.Generate(3))
                {
                    count.TryGetValue(o.Rarity, out int c);
                    count[o.Rarity] = c + 1;

                    statCount.TryGetValue(o.Stat, out int sc);
                    statCount[o.Stat] = sc + 1;

                    totalOptions++;
                }
            }

            string msg = $"=== {trials}회 x 3선택지 = {totalOptions}개 ===\n";

            // 기대 확률과 비교
            float totalWeight = 0f;
            foreach (var r in tables.Rarities) totalWeight += r.Weight;

            foreach (var r in tables.Rarities)
            {
                count.TryGetValue(r.Rarity, out int actual);
                float expected = r.Weight / totalWeight * 100f;
                float measured = actual * 100f / totalOptions;

                msg += $"{Label(r.Rarity),-6} 기대 {expected,5:F1}%  실측 {measured,5:F1}%  ({actual}개)\n";
            }

            msg += "\n--- 능력치 분포 ---\n";
            foreach (var s in tables.Stats)
            {
                statCount.TryGetValue(s.Stat, out int sc);
                msg += $"{StatName(s.Stat),-8} {sc * 100f / totalOptions,5:F1}%\n";
            }

            Debug.Log(msg);
        }

        [ContextMenu("3. Check Value Ranges")]
        void CheckValues()
        {
            if (!Ready()) return;

            var gen = new UpgradeOptionGenerator(new UnityRandomProvider(seed), tables);
            var min = new Dictionary<UpgradeRarity, float>();
            var max = new Dictionary<UpgradeRarity, float>();

            for (int i = 0; i < trials; i++)
            {
                foreach (var o in gen.Generate(3))
                {
                    // 공격력만 봐야 배율 영향을 배제할 수 있다
                    if (o.Stat != StatType.Attack) continue;

                    if (!min.ContainsKey(o.Rarity)) { min[o.Rarity] = o.Value; max[o.Rarity] = o.Value; }
                    if (o.Value < min[o.Rarity]) min[o.Rarity] = o.Value;
                    if (o.Value > max[o.Rarity]) max[o.Rarity] = o.Value;
                }
            }

            string msg = "=== 공격력 기준 수치 범위 ===\n";
            foreach (var r in tables.Rarities)
            {
                if (!min.ContainsKey(r.Rarity)) continue;
                msg += $"{Label(r.Rarity),-6} {min[r.Rarity]:F2} ~ {max[r.Rarity]:F2}   " +
                       $"(설정: {r.MinValue:F1} ~ {r.MaxValue:F1})\n";
            }

            Debug.Log(msg);
        }

        bool Ready()
        {
            if (tables == null)
            {
                Debug.LogError("UpgradeTables 에셋을 연결하세요");
                return false;
            }
            return true;
        }

        static string Label(UpgradeRarity r) => r switch
        {
            UpgradeRarity.Common => "일반",
            UpgradeRarity.Uncommon => "고급",
            UpgradeRarity.Rare => "희귀",
            UpgradeRarity.Epic => "영웅",
            UpgradeRarity.Legendary => "전설",
            UpgradeRarity.Transcend => "초월",
            UpgradeRarity.Chaos => "혼돈",
            _ => "?"
        };

        static string StatName(StatType s) => s switch
        {
            StatType.Attack => "공격력",
            StatType.MaxHp => "체력",
            StatType.MoveSpeed => "이동속도",
            StatType.AttackSpeed => "공격속도",
            StatType.CooldownRate => "쿨감",
            _ => "?"
        };
    }
}