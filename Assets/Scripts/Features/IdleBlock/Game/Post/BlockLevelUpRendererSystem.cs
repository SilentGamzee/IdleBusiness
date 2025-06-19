using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Post
{
    public class BlockLevelUpRendererSystem : EventListenerSystem<BlockChangedEvent>
    {
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;
        private EcsPool<IdleBlockViewPointer> _idleBlockViewPointerPool;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
            _idleBlockViewPointerPool = idleBlockWorld.GetPool<IdleBlockViewPointer>();
        }

        protected override void OnEvent(int eventEntityId, BlockChangedEvent component)
        {
            var blockEntityId = component.SenderEntityId;
            var idleBlock = _idleBlockPool.Get(blockEntityId);
            var view = _idleBlockViewPointerPool.Get(blockEntityId).View;
            
            view.SetLevelUpPrice(idleBlock.LevelUpPrice);
            view.SetLevel(idleBlock.Level);
        }
    }
}