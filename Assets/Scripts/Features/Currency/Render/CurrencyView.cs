using TMPro;
using UnityEngine;

namespace OLS.Features.Currency.Render
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void SetCount(int count)
        {
            _text.text = $"Balance: {count}$"; 
        }
    }
}