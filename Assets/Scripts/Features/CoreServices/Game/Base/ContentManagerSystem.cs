using Leopotam.EcsLite;
using UnityEngine;

namespace OLS.Features.CoreServices.Game.Base
{
    public class ContentManagerSystem : IEcsSystem
    {
        //TODO: Can be reworked to addressables 
        public T Load<T>(string resourcePath) where T : UnityEngine.Object
        {
            return Resources.Load<T>(resourcePath);
        }
    }
}