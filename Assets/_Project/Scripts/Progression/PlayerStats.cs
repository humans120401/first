using Game.Core;

namespace Game.Progression
{
    // 플레이어 능력치를 게임 전체에서 공유한다
    public static class PlayerStats
    {
        // 기본값 - 강화 전 수치
        const float BaseAttack = 10f;
        const float BaseMaxHp = 100f;
        const float BaseMoveSpeed = 8f;

        public static StatSet Current { get; private set; }
            = new StatSet(BaseAttack, BaseMaxHp, BaseMoveSpeed);

        public static void Reset()
        {
            Current = new StatSet(BaseAttack, BaseMaxHp, BaseMoveSpeed);
        }
    }
}