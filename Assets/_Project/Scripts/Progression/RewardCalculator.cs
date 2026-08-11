using Game.Core;

namespace Game.Progression
{
    // 스테이지 성적을 재화로 환산한다
    public static class RewardCalculator
    {
        const int BaseReward = 100;        // 기본 보상
        const int PerFloorBonus = 50;      // 층당 추가
        const int NoHitBonus = 150;        // 무피격 보너스

        public static int Calculate(StageResult result)
        {
            int reward = BaseReward + (result.Floor - 1) * PerFloorBonus;

            if (result.NoHit)
                reward += NoHitBonus;
            else if (result.TimesHit <= 2)
                reward += NoHitBonus / 2;   // 2회 이하는 절반 보너스

            return reward;
        }
    }
}