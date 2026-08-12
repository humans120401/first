namespace Game.Core
{
    public interface IUpgradeTier
    {
        int Cost { get; }
        UpgradeOutcome Resolve(float roll);
        float GetMultiplier(UpgradeOutcome outcome);
    }

    public interface IUpgradeTable
    {
        IUpgradeTier Get(RiskTier tier);
    }
}