using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Events;

namespace OLS.Features.IdleBlock.Game.Mid
{
    public class BlockIncomeReceiveSystem : EventListenerSystem<BlockReceivedIncomeEvent>
    {
        private readonly CurrencyManagerSystem _currencyManagerSystem;
        private EcsPool<Data.Components.IdleBlock> _idleBlockPool;

        public BlockIncomeReceiveSystem(CurrencyManagerSystem currencyManagerSystem)
        {
            _currencyManagerSystem = currencyManagerSystem;
        }
        
        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);
            
            var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
            _idleBlockPool = idleBlockWorld.GetPool<Data.Components.IdleBlock>();
        }

        protected override void OnEvent(int eventEntityId, BlockReceivedIncomeEvent component)
        {
            var blockEntityId = component.SenderEntityId;
            var income = _idleBlockPool.Get(blockEntityId).Income;
            
            _currencyManagerSystem.AddSoft(income);
        }
    }
}