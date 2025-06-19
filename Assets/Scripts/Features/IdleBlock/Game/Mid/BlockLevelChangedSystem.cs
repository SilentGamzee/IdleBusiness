using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Mid
{
    public class BlockLevelChangedSystem : EventListenerSystem<BlockChangedEvent>
    {
        private readonly EventsManagerSystem _eventsManagerSystem;
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;
        private EcsPool<IdleBlockOpened> _idleBlockOpenedPool;

        public BlockLevelChangedSystem(EventsManagerSystem eventsManagerSystem)
        {
            _eventsManagerSystem = eventsManagerSystem;
        }
        
        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
            _idleBlockOpenedPool = idleBlockWorld.GetPool<Data.Components.IdleBlockOpened>();
        }

        protected override void OnEvent(int eventEntityId, BlockChangedEvent component)
        {
            var blockEntityId = component.SenderEntityId;
            ref var idleBlock = ref _idleBlockPool.Get(blockEntityId);
            
            if (_idleBlockOpenedPool.Has(blockEntityId) == false && idleBlock.Level > 0)
            {
                _idleBlockOpenedPool.Add(blockEntityId);
            }
            
            idleBlock.LevelUpPrice = (idleBlock.Level + 1) * idleBlock.BaseCost;
        }
    }
}
