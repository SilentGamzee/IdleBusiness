using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Post
{
    public class BlockProgressRendererSystem : EventListenerSystem<BlockProgressChangedEvent>
    {
        private EcsPool<IdleBlockIncomeProgress> _idleBlockIncomeProgressPool;
        private EcsPool<IdleBlockViewPointer> _idleBlockViewPointerPool;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockIncomeProgressPool = idleBlockWorld.GetPool<Data.Components.IdleBlockIncomeProgress>();
            _idleBlockViewPointerPool = idleBlockWorld.GetPool<IdleBlockViewPointer>();
        }
        
        protected override void OnEvent(int eventEntityId, BlockProgressChangedEvent component)
        {
            var blockEntityId = component.SenderEntityId;

            var blockIncomeProgress = _idleBlockIncomeProgressPool.Get(blockEntityId);
            _idleBlockViewPointerPool.Get(blockEntityId).View.SetProgress(blockIncomeProgress.Progress, blockIncomeProgress.MaxProgress);
        }
    }
}