using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Mid
{
    public class BlockIncomeCalculatorSystem : EventListenerSystem<BlockChangedEvent>
    {
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;
        private EcsPool<IdleBlockUpgradeBlocks> _idleBlockUpgradeBlocks;
        private EcsPool<UpgradeBlock> _upgradeBlock;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
            _idleBlockUpgradeBlocks = idleBlockWorld.GetPool<IdleBlockUpgradeBlocks>();
            _upgradeBlock = idleBlockWorld.GetPool<UpgradeBlock>();
        }

        protected override void OnEvent(int eventEntityId, BlockChangedEvent component)
        {
            var blockEntityId = component.SenderEntityId;
            ref var idleBlock = ref _idleBlockPool.Get(blockEntityId);
            idleBlock.Income = (int)(idleBlock.Level * idleBlock.BaseIncome * GetUpgradeBlocksMultiplier(blockEntityId));
        }

        private float GetUpgradeBlocksMultiplier(int blockEntityId)
        {
            var blocksEntityIds = _idleBlockUpgradeBlocks.Get(blockEntityId).UpgradeBlocksEntityIds;
            float multiplier = 1;
            foreach (var upgradeBlockEntityId in blocksEntityIds)
            {
                var upgradeBlock = _upgradeBlock.Get(upgradeBlockEntityId);
                if (upgradeBlock.IsUpgraded)
                {
                    multiplier += upgradeBlock.IncomeMultiplier;
                }
            }

            return multiplier;
        }
    }
}