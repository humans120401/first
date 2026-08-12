using UnityEngine;
using Game.Core;
using Game.Data;
using Game.Progression;

namespace Game.Bootstrap
{
    // 강화 로직 확인용 임시 스크립트 - 나중에 삭제
    public class UpgradeTester : MonoBehaviour
    {
        [SerializeField] UpgradeTable table;
        [SerializeField] int trials = 1000;
        [SerializeField] int seed = 12345;

        [ContextMenu("Run Simulation")]
        void RunSimulation()
        {
            if (table == null)
            {
                Debug.LogError("UpgradeTable을 연결하세요");
                return;
            }

            RunTier(RiskTier.Low);
            RunTier(RiskTier.High);
        }

        void RunTier(RiskTier tier)
        {
            var rng = new UnityRandomProvider(seed);
            var stats = new StatSet(10f, 100f, 8f);
            var service = new UpgradeService(rng, table, stats);

            // 시뮬레이션용 재화 지급
            ProgressStore.Reset();
            ProgressStore.AddCurrency(999999);

            int fail = 0, normal = 0, great = 0;
            int spent = 0;

            for (int i = 0; i < trials; i++)
            {
                var r = service.TryUpgrade(StatType.Attack, tier);
                spent += r.CostPaid;

                switch (r.Outcome)
                {
                    case UpgradeOutcome.Fail: fail++; break;
                    case UpgradeOutcome.Normal: normal++; break;
                    case UpgradeOutcome.Great: great++; break;
                }
            }

            float finalAttack = stats.Get(StatType.Attack);

            Debug.Log(
                $"[{tier}] {trials}회 / 실패 {fail} ({fail * 100f / trials:F1}%) " +
                $"보통 {normal} ({normal * 100f / trials:F1}%) " +
                $"대성공 {great} ({great * 100f / trials:F1}%)\n" +
                $"소모 {spent}G / 공격력 10 → {finalAttack:F1} " +
                $"(1G당 {(finalAttack - 10f) / spent * 1000f:F2} 상승/1000G)");
        }
    }
}