using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OLS.Features.IdleBlock.Render
{
    public class IdleBlockView : MonoBehaviour
    {
        //TODO: We can split by universal view components for scaling
        [SerializeField] private TMP_Text _blockName;
        [SerializeField] private Image _incomeProgressBar;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private TMP_Text _levelUpPriceText;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private List<BlockUpgradeButtonView> _blockUpgradeButtons;

        public void Init(string blockName, Action onLevelUpCallback)
        {
            _blockName.text = blockName;
            _levelUpButton.onClick.AddListener(() => onLevelUpCallback?.Invoke());
        }

        public BlockUpgradeButtonView GetBlockUpgradeByIndex(int index)
        {
            foreach (var blockUpgradeButtonView in _blockUpgradeButtons)
            {
                if (blockUpgradeButtonView.GetBlockIndex() == index)
                {
                    return blockUpgradeButtonView;
                }
            }

            return null;
        }

        public void SetLevelButtonReady(bool isReady)
        {
            _levelUpButton.interactable = isReady;
        }

        public void SetProgress(float progress, float maxProgress)
        {
            _incomeProgressBar.fillAmount = progress / maxProgress;
        }

        public void SetLevel(int level)
        {
            _levelText.text = $"LVL\n{level}";
        }

        public void SetIncome(int income)
        {
            _incomeText.text = $"INCOME\n{income}";
        }

        public void SetLevelUpPrice(int price)
        {
            _levelUpPriceText.text = $"LEVEL UP:\n{price}$";
        }
    }
}