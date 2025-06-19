using Leopotam.EcsLite;
using OLS.Features.CoreServices.Data;

namespace OLS.Features.CoreServices.Game.Mid
{
    public abstract class EventListenerSystem<T> : IEcsInitSystem, IEcsRunSystem
        where T : struct, IEventComponent
    {
        protected EcsWorld EventsWorld;

        private EcsPool<T> eventPool;
        private EcsFilter eventFilter;

        public virtual void Init(IEcsSystems systems)
        {
            EventsWorld = systems.GetWorld(CoreServicesConst.EventsWorld);

            eventPool = EventsWorld.GetPool<T>();
            eventFilter = EventsWorld.Filter<T>().End();
        }

        public virtual void Run(IEcsSystems systems)
        {
            foreach (var entityId in eventFilter)
            {
                var component = eventPool.Get(entityId);
                OnEvent(entityId, component);
            }
        }

        protected abstract void OnEvent(int eventEntityId, T component);
    }
}
