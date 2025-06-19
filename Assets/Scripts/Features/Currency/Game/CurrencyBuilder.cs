using System.Collections.Generic;
using Core.Feature;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.Currency.Data;
using OLS.Features.Currency.Game.Base;
using OLS.Features.Currency.Game.Mid;

namespace OLS.Features.Currency.Game
{
    public class CurrencyBuilder : FeatureBuilder
    {
        private readonly EventsManagerSystem _eventsManagerSystem;

        public CurrencyBuilder(EventsManagerSystem eventsManagerSystem) : base()
        {
            _eventsManagerSystem = eventsManagerSystem;
        }
        
        protected override Dictionary<EcsWorld, string> GetFeatureWorlds()
        {
            return new Dictionary<EcsWorld, string>()
            {
                [new EcsWorld(CurrencyConst.CurrencyConfig)] = CurrencyConst.Currency,
            };
        }
        
        protected override IEcsSystem[] GetBaseSystems()
        {
            return new IEcsSystem[]
            {
                new CurrencyInitSystem(),
                new CurrencyManagerSystem(_eventsManagerSystem)
            };
        }

        protected override IEcsSystem[] GetMidSystems()
        {
            return new IEcsSystem[]
            {
               
            };
        }

        protected override IEcsSystem[] GetPostSystems()
        {
            return new IEcsSystem[]
            {
                new CurrencySaveSystem(),
                new CurrencyRenderSystem()
            };
        }
    }
}