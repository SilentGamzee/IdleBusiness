using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Mid;
using OLS.Features.Currency.Data;
using OLS.Features.Currency.Data.Events;

namespace OLS.Features.Currency.Game.Mid
{
    public class CurrencyRenderSystem : EventListenerSystem<CurrencyChangeEvent>
    {
        private EcsPool<CurrencyViewPointer> _currencyViewPointerPool;

        public override void Init(IEcsSystems systems)
        {
            base.Init(systems);

            var currencyWorld = systems.GetWorld(CurrencyConst.Currency);

            _currencyViewPointerPool = currencyWorld.GetPool<CurrencyViewPointer>();
        }

        protected override void OnEvent(int eventEntityId, CurrencyChangeEvent component)
        {
            var currencyView = _currencyViewPointerPool.Get(0).CurrencyView;
            currencyView.SetCount(component.Count);
        }
    }
}