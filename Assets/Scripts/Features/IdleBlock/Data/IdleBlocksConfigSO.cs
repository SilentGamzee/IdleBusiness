using System;
using UnityEngine;

namespace OLS.Features.IdleBlock.Data
{
    [CreateAssetMenu(fileName = "IdleBlocksConfigSO", menuName = "ScriptableObjects/IdleBlocksConfigSO", order = 1)]
    public class IdleBlocksConfigSO : ScriptableObject
    {
        public BlockData[] Blocks;
    }

    [Serializable]
    public class BlockData
    {
        public string Name;
        public float IncomeTime;
        public int BaseCost;
        public int BaseIncome;
        public UpgradeData[] UpgradeDatas;
    }

    [Serializable]
    public class UpgradeData
    {
        public int Price;
        public float IncomeMultiplier;
    }
}