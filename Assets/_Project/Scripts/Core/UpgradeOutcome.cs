namespace Game.Core
{
    public enum UpgradeOutcome
    {
        Fail,       // 실패 - 재화만 소모
        Normal,     // 보통 성공
        Great,      // 대성공
        NotEnough   // 재화 부족 - 시도 자체가 안 됨
    }
}