using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;
using UnityEngine;

namespace OLS.Features.IdleBlock.Game.Mid
{
    public class BlockIncomeProgressSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EventsManagerSystem _eventsManagerSystem;
        private EcsPool<IdleBlockIncomeProgress> _idleBlockIncomeProgressPool;
        private EcsFilter _idleBlocksFilter;

        public BlockIncomeProgressSystem(EventsManagerSystem eventsManagerSystem)
        {
            _eventsManagerSystem = eventsManagerSystem;
        }

        public void Init(IEcsSystems systems)
        {
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockIncomeProgressPool = idleBlockWorld.GetPool<IdleBlockIncomeProgress>();

            _idleBlocksFilter = idleBlockWorld.Filter<IdleBlockIncomeProgress>()
                .Inc<IdleBlockOpened>().End(100);
        }

        public void Run(IEcsSystems systems)
        {
            var delta = Time.deltaTime;
            foreach (var blockEntityId in _idleBlocksFilter)
            {
                ref var idleBlockIncomeProgress = ref _idleBlockIncomeProgressPool.Get(blockEntityId);
                idleBlockIncomeProgress.Progress += delta;
                if (idleBlockIncomeProgress.Progress >= idleBlockIncomeProgress.MaxProgress)
                {
                    idleBlockIncomeProgress.Progress = 0;
                    _eventsManagerSystem.SendEvent<BlockReceivedIncomeEvent, BlockIncomeProgressSystem>(blockEntityId);
                }

                _eventsManagerSystem.SendEvent<BlockProgressChangedEvent, BlockIncomeProgressSystem>(blockEntityId);
            }
        }
    }
}