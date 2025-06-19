using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.IdleBlock.Data;
using OLS.Features.IdleBlock.Data.Components;
using OLS.Features.IdleBlock.Data.Events;

public class BlockIncomeRendererSystem : EventListenerSystem<BlockChangedEvent>
{
    private EcsPool<IdleBlock> _idleBlockPool;
    private EcsPool<IdleBlockViewPointer> _idleBlockViewPointerPool;

    public override void Init(IEcsSystems systems)
    {
        base.Init(systems);
        var idleBlockWorld = systems.GetWorld(IdleBlockConst.IdleBlock);
        _idleBlockPool = idleBlockWorld.GetPool<IdleBlock>();
        _idleBlockViewPointerPool = idleBlockWorld.GetPool<IdleBlockViewPointer>();
    }

    protected override void OnEvent(int eventEntityId, BlockChangedEvent component)
    {
        var blockEntityId = component.SenderEntityId;
        var income = _idleBlockPool.Get(blockEntityId).Income;
        
        _idleBlockViewPointerPool.Get(blockEntityId).View.SetIncome(income);
    }
}