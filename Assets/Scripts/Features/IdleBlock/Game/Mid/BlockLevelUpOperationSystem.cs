using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Mid
{
    public class BlockLevelUpOperationSystem : EventListenerSystem<BlockLevelUpOperationEvent>
    {
        private readonly EventsManagerSystem _eventsManagerSystem;
        private readonly CurrencyManagerSystem _currencyManagerSystem;
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;

        public BlockLevelUpOperationSystem(EventsManagerSystem eventsManagerSystem, CurrencyManagerSystem currencyManagerSystem)
        {
            _eventsManagerSystem = eventsManagerSystem;
            _currencyManagerSystem = currencyManagerSystem;
        }

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
        }

        protected override void OnEvent(int eventEntityId, BlockLevelUpOperationEvent component)
        {
            var blockEntityId = component.SenderEntityId;
            ref var idleBlock = ref _idleBlockPool.Get(blockEntityId);
            var levelUpPrice = idleBlock.LevelUpPrice;
            
            var softCount = _currencyManagerSystem.GetSoftCount();
            if (softCount < levelUpPrice)
            {
                return;
            }
            
            _currencyManagerSystem.AddSoft(-levelUpPrice);
            idleBlock.Level++;
            
            _eventsManagerSystem.SendEvent<BlockChangedEvent, BlockLevelUpOperationSystem>(blockEntityId);
        }
    }
}