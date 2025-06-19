using Leopotam.EcsLite;
using OLS.Features.CoreServices.Game.Base;
using OLS.Features.Currency.Data;
using OLS.Features.Currency.Data.Events;
using UnityEngine;

public class CurrencyManagerSystem : IEcsInitSystem
{
    private EcsPool<Currency> _currencyPool;
    private readonly EventsManagerSystem _eventsManagerSystem;

    public CurrencyManagerSystem(EventsManagerSystem eventsManagerSystem)
    {
        _eventsManagerSystem = eventsManagerSystem;
    }
    
    public void Init(IEcsSystems systems)
    {
        var currencyWorld = systems.GetWorld(CurrencyConst.Currency);

        _currencyPool = currencyWorld.GetPool<Currency>();

        //Initial render update
        ref var currencyChangeEvent = ref _eventsManagerSystem.SendEvent<CurrencyChangeEvent, CurrencyManagerSystem>();
        currencyChangeEvent.Count = GetSoftCount();
    }

    public void AddSoft(int count)
    {
        //TODO: In case of scaling, we can add logic for find by currency type
        //But in our case we have world with always 1 entity
        ref var currency = ref _currencyPool.Get(0);
        currency.Count = Mathf.Min(0, currency.Count + count);

        ref var currencyChangeEvent = ref _eventsManagerSystem.SendEvent<CurrencyChangeEvent, CurrencyManagerSystem>();
        currencyChangeEvent.Count = currency.Count;
    }

    public int GetSoftCount()
    {
        ref var currency = ref _currencyPool.Get(0);
        return currency.Count;
    }
}