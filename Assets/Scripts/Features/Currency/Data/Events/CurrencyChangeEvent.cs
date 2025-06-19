using OLS.Features.CoreServices.Data;

namespace OLS.Features.Currency.Data.Events
{
    public struct CurrencyChangeEvent : IEventComponent
    {
        public int EntityId { get; set; }
        public string SenderSystem { get; set; }
        public int SenderEntityId { get; set; }
        
        public int Count { get; set; }
    }
}