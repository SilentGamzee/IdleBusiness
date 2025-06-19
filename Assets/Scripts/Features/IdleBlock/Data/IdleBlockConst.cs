using Leopotam.EcsLite;

namespace OLS.Features.IdleBlock.Data
{
    public static class IdleBlockConst
    {
        public const string IdleBlock = nameof(IdleBlock);
        public static readonly EcsWorld.Config IdleBlockConfig = new();

        public const string BlockSavePrefix = "Block";
        public const string BlockResourceName = "BusinessBlock";
        public const string ConfigResourceName = "IdleBlocksConfigSO";
        public const string ConfigNamesResourceName = "IdleBlockNamesConfigSO";
    }
}