using System;
using System.Collections.Generic;
using Core.Feature;
using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.Currency.Game;
using OLS.Features.IdleBlock.Game;
using UnityEngine;

namespace Core.Game
{
    public class CoreEntryPoint : MonoBehaviour
    {
        private EcsSystems _systems;

        private bool isInited = false;

        public void Start()
        {
            InitSystems();
        }

        private void OnDestroy()
        {
            if (_systems == null)
            {
                return;
            }

            var worlds = _systems.GetAllNamedWorlds().Values;

            _systems.Destroy();
            _systems = null;

            foreach (var world in worlds)
            {
                world.Destroy();
            }
        }

        private void InitSystems()
        {
            _systems = new EcsSystems(new EcsWorld());

            //Systems
            var coreBuilder = new CoreServicesBuilder();
            var currencyBuilder = new CurrencyBuilder(coreBuilder.Get<EventsManagerSystem>());
            var idleBlockBuilder = new IdleBlockBuilder(
                coreBuilder.Get<ContentManagerSystem>(),
                coreBuilder.Get<EventsManagerSystem>(),
                currencyBuilder.Get<CurrencyManagerSystem>());

            BuildFeatureBuilders(new List<FeatureBuilder>
            {
                coreBuilder,
                currencyBuilder,
                idleBlockBuilder
            });

            _systems.Init();
            isInited = true;
        }

        private void BuildFeatureBuilders(List<FeatureBuilder> featureBuilders)
        {
            foreach (var featureBuilder in featureBuilders)
            {
                featureBuilder.PrecacheSystems();
            }
            
            foreach (var featureBuilder in featureBuilders)
            {
                featureBuilder.BuildFeatureWorlds(_systems);
            }
            
            foreach (var featureBuilder in featureBuilders)
            {
                featureBuilder.BuildBaseSystems(_systems);
            }
            
            foreach (var featureBuilder in featureBuilders)
            {
                featureBuilder.BuildMiddleSystems(_systems);
            }

            featureBuilders.Reverse();
            foreach (var featureBuilder in featureBuilders)
            {
                featureBuilder.BuildPostSystems(_systems);
            }
        }

        private void Update()
        {
            if (isInited)
            {
                _systems?.Run();
            }
        }
    }
}