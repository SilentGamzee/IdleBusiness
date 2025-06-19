using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.Currency.Data;
using OLS.Features.Currency.Data.Events;
using UnityEngine;

namespace OLS.Features.Currency.Game.Mid
{
    public class CurrencySaveSystem : EventListenerSystem<CurrencyChangeEvent>
    {
        private EcsPool<Data.Currency> _currencyPool;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);

            var currencyWorld = systems.GetWorld(CurrencyConst.Currency);
            _currencyPool = currencyWorld.GetPool<Data.Currency>();
        }

        protected override void OnEvent(int eventEntityId, CurrencyChangeEvent component)
        {
            var currencyCount = _currencyPool.Get(0).Count;
            PlayerPrefs.SetInt(CurrencyConst.SoftCurrency, currencyCount);
            PlayerPrefs.Save();
        }
    }
}