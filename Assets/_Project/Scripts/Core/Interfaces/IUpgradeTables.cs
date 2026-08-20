using System.Collections.Generic;

namespace Game.Core
{
    // 등급 하나의 정보
    public interface IRarityInfo
    {
        UpgradeRarity Rarity { get; }
        float Weight { get; }
        float MinValue { get; }
        float MaxValue { get; }
    }

    // 능력치 하나의 정보
    public interface IStatInfo
    {
        StatType Stat { get; }
        float Scale { get; }
        float Weight { get; }
        string Suffix { get; }
    }

    // 선택지 생성에 필요한 데이터 전체
    public interface IUpgradeTables
    {
        IReadOnlyList<IRarityInfo> Rarities { get; }
        IReadOnlyList<IStatInfo> Stats { get; }
    }
}