using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockUpgradeButtonView : MonoBehaviour
{
    [SerializeField] private int _blockIndex;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _incomeText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Button _upgradeButton;

    public void Init(string blockName, float income, int price, Action onButtonClick)
    {
        _nameText.text = blockName;
        _incomeText.text = $"Income: {(income * 100):F0}%";
        _priceText.text = $"Price: {price}$";
        _upgradeButton.onClick.AddListener(() => onButtonClick?.Invoke());
    }
    
    public int GetBlockIndex() => _blockIndex;

    public void SetButtonEnabled(bool isEnabled)
    {
        _upgradeButton.interactable = isEnabled;
    }

    public void SetUpgraded()
    {
        SetButtonEnabled(false);
        _priceText.text = $"BOUGHT";
    }
}