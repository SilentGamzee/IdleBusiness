using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.Currency.Data.Events;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;

namespace OLS.Features.IdleBlock.Game.Post
{
    public class BlockButtonsUpdateOnCurrencyChangeSystem : EventListenerSystem<CurrencyChangeEvent>
    {
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;
        private EcsPool<IdleBlockUpgradeBlocks> _idleBlockUpgradeBlocksPool;
        private EcsPool<IdleBlockViewPointer> _idleBlockViewPointerPool;
        private EcsPool<UpgradeBlock> _upgradeBlockPool;
        private EcsPool<UpgradeBlockViewPointer> _upgradeBlockViewPointerPool;
        private EcsFilter _idleBlockFilter;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
            _idleBlockUpgradeBlocksPool = idleBlockWorld.GetPool<Data.Components.IdleBlockUpgradeBlocks>();
            _idleBlockViewPointerPool = idleBlockWorld.GetPool<IdleBlockViewPointer>();

            _upgradeBlockPool = idleBlockWorld.GetPool<UpgradeBlock>();
            _upgradeBlockViewPointerPool = idleBlockWorld.GetPool<UpgradeBlockViewPointer>();

            _idleBlockFilter = idleBlockWorld.Filter<Data.Components.IdleBlock>().End(100);
        }

        protected override void OnEvent(int eventEntityId, CurrencyChangeEvent component)
        {
            var softCount = component.Count;
            
            foreach (var blockEntityId in _idleBlockFilter)
            {
                var idleBlock = _idleBlockPool.Get(blockEntityId);
                var isEnabled = softCount >= idleBlock.LevelUpPrice;
                _idleBlockViewPointerPool.Get(blockEntityId).View.SetLevelButtonReady(isEnabled);

                var upgradeBlocksEntityIds = _idleBlockUpgradeBlocksPool.Get(blockEntityId).UpgradeBlocksEntityIds;
                foreach (var upgradeBlocksEntityId in upgradeBlocksEntityIds)
                {
                    var upgradeBlock = _upgradeBlockPool.Get(upgradeBlocksEntityId);
                    if (upgradeBlock.IsUpgraded)
                    {
                        continue;
                    }
                    
                    var isUpgradeButtonEnabled = softCount >= upgradeBlock.UpgradePrice;
                    _upgradeBlockViewPointerPool.Get(upgradeBlocksEntityId).View.SetButtonEnabled(isUpgradeButtonEnabled);
                }
            }
        }
    }
}