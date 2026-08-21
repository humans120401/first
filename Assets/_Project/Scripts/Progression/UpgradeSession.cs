using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data;

namespace Game.Progression
{
    // 강화 화면 한 번의 진행 상태를 관리한다
    public class UpgradeSession
    {
        readonly UpgradeOptionGenerator _generator;
        readonly StatSet _stats;

        public IReadOnlyList<UpgradeOption> CurrentOptions => _options;
        public int RerollCount { get; private set; }
        public int NextRerollCost => baseRerollCost + RerollCount * rerollCostStep;

        List<UpgradeOption> _options = new();

        // 비용 설정 - 나중에 테이블로 뺄 수 있다
        const int drawCost = 100;
        const int baseRerollCost = 50;
        const int rerollCostStep = 50;

        public event Action OptionsChanged;
        public event Action<UpgradeOption> OptionApplied;
        public event Action<string> Rejected;

        public UpgradeSession(UpgradeOptionGenerator generator, StatSet stats)
        {
            _generator = generator;
            _stats = stats;
        }

        // 강화 화면에 들어와서 첫 선택지를 뽑는다
        public bool Draw()
        {
            if (ProgressStore.Current.currency < drawCost)
            {
                Rejected?.Invoke($"재화가 부족합니다 ({drawCost} 필요)");
                return false;
            }

            ProgressStore.SpendCurrency(drawCost);
            RerollCount = 0;
            _options = _generator.Generate(3);
            OptionsChanged?.Invoke();
            return true;
        }

        // 마음에 안 들면 다시 뽑는다 - 비용이 점점 오른다
        public bool Reroll()
        {
            if (_options.Count == 0)
            {
                Rejected?.Invoke("먼저 강화를 시작하세요");
                return false;
            }

            int cost = NextRerollCost;
            if (ProgressStore.Current.currency < cost)
            {
                Rejected?.Invoke($"재화가 부족합니다 ({cost} 필요)");
                return false;
            }

            ProgressStore.SpendCurrency(cost);
            RerollCount++;
            _options = _generator.Generate(3);
            OptionsChanged?.Invoke();
            return true;
        }

        // 셋 중 하나를 고른다
        public bool Select(int index)
        {
            if (index < 0 || index >= _options.Count)
            {
                Rejected?.Invoke("잘못된 선택입니다");
                return false;
            }

            var option = _options[index];
            _stats.AddModifier(new StatModifier(option.Stat, option.Value));

            _options.Clear();
            RerollCount = 0;

            GameEvents.RaiseStatsChanged();   // 추가

            OptionApplied?.Invoke(option);
            OptionsChanged?.Invoke();
            return true;
        }

        public int DrawCost => drawCost;


    }
}