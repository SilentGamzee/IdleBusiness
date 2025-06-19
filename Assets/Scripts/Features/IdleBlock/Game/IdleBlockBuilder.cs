using System.Collections.Generic;
using Core.Feature;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Game.Base;

namespace OLS.Features.IdleBlock.Game
{
    public class IdleBlockBuilder : FeatureBuilder
    {
        private readonly ContentManagerSystem _contentManagerSystem;
        private readonly EventsManagerSystem _eventsManagerSystem;

        public IdleBlockBuilder(ContentManagerSystem contentManagerSystem, EventsManagerSystem eventsManagerSystem)
        {
            _contentManagerSystem = contentManagerSystem;
            _eventsManagerSystem = eventsManagerSystem;
        }
        
        protected override Dictionary<EcsWorld, string> GetFeatureWorlds()
        {
            return new Dictionary<EcsWorld, string>()
            {
                [new EcsWorld(IdleBlockConst.IdleBlockConfig)] = IdleBlockConst.IdleBlock,
            };
        }
        
        protected override IEcsSystem[] GetBaseSystems()
        {
            return new IEcsSystem[]
            {
                new InitBlocksSystem(_contentManagerSystem, _eventsManagerSystem)
            };
        }

        protected override IEcsSystem[] GetMidSystems()
        {
            return new IEcsSystem[0];
        }

        protected override IEcsSystem[] GetPostSystems()
        {
            return new IEcsSystem[0];
        }
    }
}