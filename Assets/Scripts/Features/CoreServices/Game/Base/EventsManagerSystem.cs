using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Data;

namespace OLS.Features.CoreServices.Game.Base
{
    public class EventsManagerSystem : IEcsInitSystem
    {
        private EcsWorld eventsWorld;

        public void Init(IEcsSystems systems)
        {
            eventsWorld = systems.GetWorld(CoreServicesConst.EventsWorld);
        }

        public ref T SendEvent<T, TK>(int entityId = -1) 
            where T : struct, IEventComponent
            where TK: IEcsSystem
        {
            ref var component = ref NewComponentWithId<T>(eventsWorld);
            component.SenderSystem = nameof(TK); //DEBUG INFO
            component.SenderEntityId = entityId;
            
            return ref component;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ref T NewWithId<T>(EcsPool<T> pool)
            where T : struct, IEventComponent
        {
            int entityId = pool.GetWorld().NewEntity();
            ref var res = ref pool.Add(entityId);
            res.EntityId = entityId;
            return ref res;
        }
        
        private static ref T NewComponentWithId<T>(EcsWorld world)
            where T : struct, IEventComponent
        {
            return ref NewWithId(world.GetPool<T>());
        }
    }

}