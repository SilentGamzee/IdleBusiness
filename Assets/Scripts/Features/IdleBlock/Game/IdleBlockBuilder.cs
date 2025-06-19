using System.Collections.Generic;
using Core.Feature;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Game.Base;
using OLS.Features.IdleBlock.Game.Mid;
using OLS.Features.IdleBlock.Game.Post;

namespace OLS.Features.IdleBlock.Game
{
    public class IdleBlockBuilder : FeatureBuilder
    {
        private readonly ContentManagerSystem _contentManagerSystem;
        private readonly EventsManagerSystem _eventsManagerSystem;
        private readonly CurrencyManagerSystem _currencyManagerSystem;

        public IdleBlockBuilder(ContentManagerSystem contentManagerSystem, EventsManagerSystem eventsManagerSystem, CurrencyManagerSystem currencyManagerSystem)
        {
            _contentManagerSystem = contentManagerSystem;
            _eventsManagerSystem = eventsManagerSystem;
            _currencyManagerSystem = currencyManagerSystem;
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
                new InitBlocksSystem(_contentManagerSystem, _eventsManagerSystem),
            };
        }

        protected override IEcsSystem[] GetMidSystems()
        {
            return new IEcsSystem[]
            {
                new BlockLevelUpOperationSystem(_eventsManagerSystem, _currencyManagerSystem),
                new UpgradeBlockOperationSystem(_eventsManagerSystem, _currencyManagerSystem),
                new BlockLevelChangedSystem(_eventsManagerSystem),
                
                new BlockIncomeCalculatorSystem(),
                new BlockIncomeProgressSystem(_eventsManagerSystem),
                new BlockIncomeReceiveSystem(_currencyManagerSystem),
            };
        }

        protected override IEcsSystem[] GetPostSystems()
        {
            return new IEcsSystem[]
            {
                new BlockIncomeRendererSystem(),
                new BlockProgressRendererSystem(),
                new BlockLevelUpRendererSystem(),
                new BlockButtonsUpdateOnCurrencyChangeSystem(),
                new UpgradeBlockChangedRendererSystem(),
                new BlockSaveSystem(),
            };
        }
    }
}