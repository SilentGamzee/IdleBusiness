using System;

namespace OLS.Features.IdleBlock.Data
{
    [Serializable]
    public class BlockSaveData
    {
        public int Level;
        public float Progress;
        public int[] UpgradeIndexes;
    }
}