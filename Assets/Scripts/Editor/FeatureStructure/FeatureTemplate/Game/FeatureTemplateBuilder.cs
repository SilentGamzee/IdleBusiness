using System.Collections.Generic;
using Core.Feature;
using Leopotam.EcsLite;
using OLS.Features.FeatureTemplate.Data;

namespace OLS.Features.FeatureTemplate.Game
{
    public class FeatureTemplateBuilder : FeatureBuilder
    {
        protected override Dictionary<EcsWorld, string> GetFeatureWorlds()
        {
            return new Dictionary<EcsWorld, string>()
            {
                [new EcsWorld(FeatureTemplateConst.FeatureTemplateConfig)] = FeatureTemplateConst.FeatureTemplate,
            };
        }
        
        protected override IEcsSystem[] GetBaseSystems()
        {
            return new IEcsSystem[0];
        }

        protected override IEcsSystem[] GetMidSystems()
        {
            return new IEcsSystem[0];
        }

        protected override IEcsSystem[] GetPostSystems()
        {
            return new IEcsSystem[0];
        }
    }
}