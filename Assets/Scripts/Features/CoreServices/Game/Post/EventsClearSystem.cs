using System;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Data;

namespace OLS.Features.CoreServices.Game.Post
{
    public class EventsClearSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld eventsWorld;
        private int[] entities = new int[10];

        public void Init(IEcsSystems systems)
        {
            eventsWorld = systems.GetWorld(CoreServicesConst.EventsWorld);
        }

        public void Run(IEcsSystems systems)
        {
            int curCount = eventsWorld.GetEntitiesCount();
            if (curCount == 0)
            {
                return;
            }

            if (entities.Length < curCount)
            {
                Array.Resize(ref entities, entities.Length << 1);
            }

            int count = eventsWorld.GetAllEntities(ref entities);
            for (var index = 0; index < count; index++)
            {
                var entityId = entities[index];
                eventsWorld.DelEntity(entityId);
            }
        }
    }
}