using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Feature
{
    public abstract class FeatureBuilder
    {
        protected abstract Dictionary<EcsWorld, string> GetFeatureWorlds();
        protected abstract IEcsSystem[] GetBaseSystems();
        protected abstract IEcsSystem[] GetMidSystems();
        protected abstract IEcsSystem[] GetPostSystems();
        
        public void BuildFeatureWorlds(EcsSystems ecsSystems)
        {
            foreach (var kv in GetFeatureWorlds())
            {
                ecsSystems.AddWorld(kv.Key, kv.Value);
            }
        }
        
        public void BuildBaseSystems(EcsSystems ecsSystems)
        {
            foreach (var baseSystem in GetBaseSystems())
            {
                ecsSystems.Add(baseSystem);
            }
        }

        public void BuildMiddleSystems(EcsSystems ecsSystems)
        {
            foreach (var middleSystem in GetMidSystems())
            {
                ecsSystems.Add(middleSystem);
            }
        }

        public virtual void BuildPostSystems(EcsSystems ecsSystems)
        {
            foreach (var postSystem in GetPostSystems())
            {
                ecsSystems.Add(postSystem);
            }
        }
    }
}