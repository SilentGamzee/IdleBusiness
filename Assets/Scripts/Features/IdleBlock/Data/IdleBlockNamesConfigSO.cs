using System;
using UnityEngine;

namespace OLS.Features.IdleBlock.Data
{
    [CreateAssetMenu(fileName = "IdleBlockNamesConfigSO", menuName = "ScriptableObjects/IdleBlockNamesConfigSO", order = 1)]
    public class IdleBlockNamesConfigSO : ScriptableObject
    {
        public IdleBlockName[] IdleBlocksNames;
    }

    [Serializable]
    public class IdleBlockName
    {
        public string Name;
        public string[] UpgradeBlocksNames;
    }
}