using System;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;
using OLS.Features.IdleBlock.Render;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OLS.Features.IdleBlock.Game.Base
{
    public class InitBlocksSystem : IEcsInitSystem
    {
        private readonly ContentManagerSystem _contentManager;
        private readonly EventsManagerSystem _eventsManagerSystem;
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;
        private EcsPool<IdleBlockIncomeProgress> _idleBlockIncomePool;
        private EcsPool<IdleBlockUpgradeBlocks> _idleBlockUpgradeBlocksPool;
        private EcsPool<UpgradeBlock> _upgradeBlockPool;
        
        public InitBlocksSystem(ContentManagerSystem contentManager, EventsManagerSystem eventsManagerSystem)
        {
            _contentManager = contentManager;
            _eventsManagerSystem = eventsManagerSystem;
        }

        public void Init(IEcsSystems systems)
        {
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);

            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
            _idleBlockIncomePool = idleBlockWorld.GetPool<IdleBlockIncomeProgress>();
            _idleBlockUpgradeBlocksPool = idleBlockWorld.GetPool<IdleBlockUpgradeBlocks>();
            _upgradeBlockPool = idleBlockWorld.GetPool<UpgradeBlock>();
            var idleBlockViewPointerPool = idleBlockWorld.GetPool<IdleBlockViewPointer>();
            var upgradeBlockViewPointerPool = idleBlockWorld.GetPool<UpgradeBlockViewPointer>();

            var blockResourcePrefab = _contentManager.Load<GameObject>(IdleBlockConst.BlockResourceName)
                .GetComponent<IdleBlockView>();
            var config = _contentManager.Load<IdleBlocksConfigSO>(IdleBlockConst.ConfigResourceName);
            var containerTransform = Object.FindFirstObjectByType<BlocksContainerView>().transform;
            
            for (int i = 0; i < config.Blocks.Length; i++)
            {
                var configData = config.Blocks[i];

                var blockEntityId = InitIdleBlockComponents(i, idleBlockWorld, configData);

                ref var idleBlockViewPointer = ref idleBlockViewPointerPool.Add(blockEntityId);
                var idleBlock = Object.Instantiate(blockResourcePrefab, containerTransform);
                idleBlock.Init(configData.Name, 0, () => OnLevelUpClick(blockEntityId));
                idleBlockViewPointer.View = idleBlock;

                var upgradeBlockEntityIds = _idleBlockUpgradeBlocksPool.Get(blockEntityId).UpgradeBlocksEntityIds;
                
                for (int j = 0; j < configData.UpgradeDatas.Length; j++)
                {
                    var upgradeData = configData.UpgradeDatas[j];

                    var upgradeBlockEntityId = upgradeBlockEntityIds[j];

                    ref var upgradeBlockViewPointer = ref upgradeBlockViewPointerPool.Get(upgradeBlockEntityId);
                    var upgradeBlock = idleBlock.GetBlockUpgradeByIndex(j);
                    upgradeBlock.Init($"{IdleBlockConst.UpgradeBlockPrefix} {j + 1}", upgradeData.IncomeMultiplier,
                        upgradeData.Price, () => OnButtonUpgradeClick(upgradeBlockEntityId));

                    upgradeBlockViewPointer.View = upgradeBlock;
                }

                _eventsManagerSystem.SendEvent<BlockIncomeChanged, InitBlocksSystem>(blockEntityId);
            }
        }

        private int InitIdleBlockComponents(int blockIndex, EcsWorld world, BlockData blockData)
        {
            var blockEntityId = world.NewEntity();

            int level;
            float progress;
            int[] upgradeIndexes;
            if (TryGetBlockSaveData(blockIndex, out var blockSaveData))
            {
                level = blockSaveData.Level;
                progress = blockSaveData.Progress;
                upgradeIndexes = blockSaveData.UpgradeIndexes;
            }
            else
            {
                level = blockIndex == 0 ? 1 : 0;
                progress = 0;
                upgradeIndexes = Array.Empty<int>();
            }

            ref var idleBlock = ref _idleBlockPool.Add(blockEntityId);
            idleBlock.EntityId = blockEntityId;
            idleBlock.Level = level;
            idleBlock.BaseCost = blockData.BaseCost;
            idleBlock.BaseIncome = blockData.BaseIncome;

            ref var idleBlockIncome = ref _idleBlockIncomePool.Add(blockEntityId);
            idleBlockIncome.Progress = progress;
            idleBlockIncome.MaxProgress = blockData.IncomeTime;

            ref var idleBlockUpgradeBlocks = ref _idleBlockUpgradeBlocksPool.Add(blockEntityId);
            idleBlockUpgradeBlocks.UpgradeBlocksEntityIds =
                InitUpgradeBlocksComponents(blockData.UpgradeDatas, world, upgradeIndexes);

            return blockEntityId;
        }

        private int[] InitUpgradeBlocksComponents(UpgradeData[] data, EcsWorld world, int[] upgradedIndexes)
        {
            bool IsUpgraded(int blockIndex)
            {
                foreach (var index in upgradedIndexes)
                {
                    if (index == blockIndex)
                    {
                        return true;
                    }
                }

                return false;
            }

            int[] upgradeBlocksEntityIds = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var upgradeData = data[i];

                var upgradeBlockEntityId = world.NewEntity();
                
                ref var upgradeBlock = ref _upgradeBlockPool.Add(upgradeBlockEntityId);
                upgradeBlock.EntityId = upgradeBlockEntityId;
                upgradeBlock.IsUpgraded = IsUpgraded(i);
                upgradeBlock.IncomeMultiplier = upgradeData.IncomeMultiplier;
                upgradeBlock.UpgradePrice = upgradeData.Price;

                upgradeBlocksEntityIds[i] = upgradeBlock.EntityId;
            }

            return upgradeBlocksEntityIds;
        }

        private bool TryGetBlockSaveData(int blockIndex, out BlockSaveData data)
        {
            var key = $"{IdleBlockConst.BlockSavePrefix}{blockIndex}";
            if (PlayerPrefs.HasKey(key))
            {
                var json = PlayerPrefs.GetString(key);
                data = JsonUtility.FromJson<BlockSaveData>(json);
                return true;
            }

            data = null;
            return false;
        }

        private void OnLevelUpClick(int blockEntityId)
        {
        }

        private void OnButtonUpgradeClick(int upgradeBlockEntityId)
        {
        }
    }
}