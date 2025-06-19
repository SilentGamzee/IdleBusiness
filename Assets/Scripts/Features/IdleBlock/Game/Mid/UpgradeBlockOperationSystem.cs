using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Mid
{
    public class UpgradeBlockOperationSystem : EventListenerSystem<UpgradeBlockOperationEvent>
    {
        private readonly EventsManagerSystem _eventsManagerSystem;
        private readonly CurrencyManagerSystem _currencyManagerSystem;
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;
        private EcsPool<UpgradeBlock> _upgradeBlockPool;

        public UpgradeBlockOperationSystem(EventsManagerSystem eventsManagerSystem,
            CurrencyManagerSystem currencyManagerSystem)
        {
            _eventsManagerSystem = eventsManagerSystem;
            _currencyManagerSystem = currencyManagerSystem;
        }

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);

            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _upgradeBlockPool = idleBlockWorld.GetPool<Data.Components.UpgradeBlock>();
        }

        protected override void OnEvent(int eventEntityId, UpgradeBlockOperationEvent component)
        {
            ref var upgradeBlock = ref _upgradeBlockPool.Get(component.SenderEntityId);

            var softCount = _currencyManagerSystem.GetSoftCount();
            if (softCount < upgradeBlock.UpgradePrice)
            {
                return;
            }

            _currencyManagerSystem.AddSoft(-upgradeBlock.UpgradePrice);
            upgradeBlock.IsUpgraded = true;

            _eventsManagerSystem.SendEvent<BlockChangedEvent, BlockLevelUpOperationSystem>(upgradeBlock.IdleBlockEntityId);
            _eventsManagerSystem.SendEvent<UpgradeBlockChangedEvent, BlockLevelUpOperationSystem>(component.SenderEntityId);
        }
    }
}