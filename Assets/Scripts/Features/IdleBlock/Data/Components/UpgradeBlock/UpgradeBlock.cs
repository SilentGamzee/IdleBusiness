namespace OLS.Features.IdleBlock.Data.Components
{
    public struct UpgradeBlock
    {
        public int EntityId { get; set; }
        public int IdleBlockEntityId { get; set; }
        public int UpgradeBlockIndex { get; set; }
        public bool IsUpgraded { get; set; }
        public int UpgradePrice { get; set; }
        public float IncomeMultiplier { get; set; }
    }
}