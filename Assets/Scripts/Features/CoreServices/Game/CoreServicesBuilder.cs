using System;
using System.Collections.Generic;
using Core.Feature;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Data;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.CoreServices.Game.Post;
using UnityEngine;

namespace OLS.Features.CoreServices.Game
{
    public class CoreServicesBuilder : FeatureBuilder
    {
        protected override Dictionary<EcsWorld, string> GetFeatureWorlds()
        {
            return new Dictionary<EcsWorld, string>()
            {
                [new EcsWorld(CoreServicesConst.CoreServicesConfig)] = CoreServicesConst.CoreServices,
                [new EcsWorld(CoreServicesConst.EventsWorldConfig)] = CoreServicesConst.EventsWorld,
            };
        }
        
        protected override IEcsSystem[] GetBaseSystems()
        {
            return new IEcsSystem[]
            {
                new ContentManagerSystem(),
                new EventsManagerSystem()
            };
        }

        protected override IEcsSystem[] GetMidSystems()
        {
            return Array.Empty<IEcsSystem>();
        }

        protected override IEcsSystem[] GetPostSystems()
        {
            return new IEcsSystem[]
            {
                new EventsClearSystem()
            };
        }

        public override void BuildPostSystems(EcsSystems ecsSystems)
        {
            base.BuildPostSystems(ecsSystems);
            
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                // Register debug systems for control state of every world
                ecsSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
                foreach (var namedWorld in ecsSystems.GetAllNamedWorlds())
                {
                    ecsSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(namedWorld.Key));
                }
            }
#endif
        }
    }
}