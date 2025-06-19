using Leopotam.EcsLite;

namespace OLS.Features.Currency.Data
{
    public static class CurrencyConst
    {
        public const string Currency = nameof(Currency);
        public static readonly EcsWorld.Config CurrencyConfig = new();
        
        public const string SoftCurrency = nameof(SoftCurrency);
    }
}