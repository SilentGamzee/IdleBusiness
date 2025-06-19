using Leopotam.EcsLite;

namespace OLS.Features.CoreServices.Data
{
    public static class CoreServicesConst
    {
        public const string CoreServices = nameof(CoreServices);
        public static readonly EcsWorld.Config CoreServicesConfig = new(); 
        
        public const string EventsWorld = nameof(EventsWorld);
        public static readonly EcsWorld.Config EventsWorldConfig = new(); 
    }
}