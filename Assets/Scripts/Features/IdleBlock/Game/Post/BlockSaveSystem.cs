using System.Collections.Generic;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;
using UnityEngine;

namespace OLS.Features.IdleBlock.Game.Post
{
    public class BlockSaveSystem : EventListenerSystem<BlockChangedEvent>
    {
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;
        private EcsPool<IdleBlockUpgradeBlocks> _idleBlockUpgradeBlocksPool;
        private EcsPool<IdleBlockIncomeProgress> _idleBlockIncomeProgress;
        private EcsPool<UpgradeBlock> _upgradeBlocksPool;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
            _idleBlockUpgradeBlocksPool = idleBlockWorld.GetPool<Data.Components.IdleBlockUpgradeBlocks>();
            _idleBlockIncomeProgress = idleBlockWorld.GetPool<Data.Components.IdleBlockIncomeProgress>();
            _upgradeBlocksPool = idleBlockWorld.GetPool<UpgradeBlock>();
        }

        protected override void OnEvent(int eventEntityId, BlockChangedEvent component)
        {
            var blockEntityId = component.SenderEntityId;
            var idleBlock = _idleBlockPool.Get(blockEntityId);

            var upgradeBlocksEntityIds = _idleBlockUpgradeBlocksPool.Get(blockEntityId).UpgradeBlocksEntityIds;
            List<int> upgradedBlocksIds = new List<int>(upgradeBlocksEntityIds.Length);
            foreach (var upgradeBlocksEntityId in upgradeBlocksEntityIds)
            {
                var upgradeBlock = _upgradeBlocksPool.Get(upgradeBlocksEntityId);
                if (upgradeBlock.IsUpgraded)
                {
                    upgradedBlocksIds.Add(upgradeBlock.UpgradeBlockIndex);
                }
            }
            
            var blockSaveData = new BlockSaveData
            {
                Level = idleBlock.Level,
                Progress = _idleBlockIncomeProgress.Get(blockEntityId).Progress,
                UpgradeIndexes = upgradedBlocksIds.ToArray()
            };

            var json = JsonUtility.ToJson(blockSaveData);
            
            var key = $"{IdleBlockConst.BlockSavePrefix}{idleBlock.BlockIndex}";
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }
}