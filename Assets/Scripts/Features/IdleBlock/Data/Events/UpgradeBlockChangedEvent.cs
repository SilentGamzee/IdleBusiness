using OLS.Features.CoreServices.Data;

namespace OLS.Features.IdleBlock.Data.Events
{
    public struct UpgradeBlockChangedEvent : IEventComponent
    {
        public int EventEntityId { get; set; }
        public string SenderSystem { get; set; }
        public int SenderEntityId { get; set; }
    }
}