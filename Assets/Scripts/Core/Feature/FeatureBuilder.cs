using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Core.Feature
{
    public abstract class FeatureBuilder
    {
        private IEcsSystem[] _baseSystems;
        private IEcsSystem[] _midSystems;
        private IEcsSystem[] _postSystems;
        
        protected abstract Dictionary<EcsWorld, string> GetFeatureWorlds();
        protected abstract IEcsSystem[] GetBaseSystems();
        protected abstract IEcsSystem[] GetMidSystems();
        protected abstract IEcsSystem[] GetPostSystems();

        public T GetSystem<T>() where T : IEcsSystem
        {
            PrecacheSystems();
            
            foreach (var system in _baseSystems)
            {
                if (system is T tSystem)
                {
                    return tSystem;
                }
            }
            
            foreach (var system in _midSystems)
            {
                if (system is T tSystem)
                {
                    return tSystem;
                }
            }
            
            foreach (var system in _postSystems)
            {
                if (system is T tSystem)
                {
                    return tSystem;
                }
            }
            
            Debug.LogError($"Cannot find {nameof(T)} in {this.GetType()}");
            return default;
        }

        public void PrecacheSystems()
        {
            if (_baseSystems != null)
            {
                return;
            }
            
            _baseSystems = GetBaseSystems();
            _midSystems = GetMidSystems();
            _postSystems = GetPostSystems();
        }
        
        public void BuildFeatureWorlds(EcsSystems ecsSystems)
        {
            foreach (var kv in GetFeatureWorlds())
            {
                ecsSystems.AddWorld(kv.Key, kv.Value);
            }
        }
        
        public void BuildBaseSystems(EcsSystems ecsSystems)
        {
            foreach (var baseSystem in _baseSystems)
            {
                ecsSystems.Add(baseSystem);
            }
        }

        public void BuildMiddleSystems(EcsSystems ecsSystems)
        {
            foreach (var middleSystem in _midSystems)
            {
                ecsSystems.Add(middleSystem);
            }
        }

        public virtual void BuildPostSystems(EcsSystems ecsSystems)
        {
            foreach (var postSystem in _postSystems)
            {
                ecsSystems.Add(postSystem);
            }
        }
    }
}