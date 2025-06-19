using Leopotam.EcsLite;
using OLS.Features.Currency.Data;
using OLS.Features.Currency.Render;
using UnityEngine;

namespace OLS.Features.Currency.Game.Base
{
    public class CurrencyInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var currencyWorld = systems.GetWorld(CurrencyConst.Currency);
            
            var currencyView = Object.FindFirstObjectByType<CurrencyView>();
            
            var currencyEntityId = currencyWorld.NewEntity();
            ref var currency = ref currencyWorld.GetPool<Data.Currency>().Add(currencyEntityId);
            currency.EntityId = currencyEntityId;

            ref var currencyViewPointer = ref currencyWorld.GetPool<Data.CurrencyViewPointer>().Add(currencyEntityId);
            currencyViewPointer.CurrencyView = currencyView;
        }
    }
}