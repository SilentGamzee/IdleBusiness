using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Post
{
    public class UpgradeBlockChangedRendererSystem : EventListenerSystem<UpgradeBlockChangedEvent>
    {
        private EcsPool<UpgradeBlock> _upgradeBlockPool;
        private EcsPool<UpgradeBlockViewPointer> _upgradeBlockViewPointerPool;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _upgradeBlockPool = idleBlockWorld.GetPool<UpgradeBlock>();
            _upgradeBlockViewPointerPool = idleBlockWorld.GetPool<UpgradeBlockViewPointer>();
        }

        protected override void OnEvent(int eventEntityId, UpgradeBlockChangedEvent component)
        {
            var upgradeBlock = _upgradeBlockPool.Get(component.SenderEntityId);
            if (upgradeBlock.IsUpgraded)
            {
                _upgradeBlockViewPointerPool.Get(component.SenderEntityId).View.SetUpgraded();
            }
        }
    }
}