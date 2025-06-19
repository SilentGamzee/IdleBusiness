namespace OLS.Features.CoreServices.Data
{
    public interface IEventComponent
    {
        public int EventEntityId { get; set; }
        public string SenderSystem { get; set; } //DEBUG INFO
        public int SenderEntityId { get; set; }
    }
}