using System;
using UnityEngine;
using TMPro;

namespace Game.Wallet
{
    public class WalletDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;

        private Wallet wallet;

        private void Start()
        {
            UpdateDisplay();
        }

        private int t;

        private void Update()
        {
            t++;
            if (t <= 10) return;
            UpdateDisplay();
            t = 0;
        }

        public void UpdateDisplay()
        {
            if (Wallet.Instance != null && coinsText != null)
                coinsText.text = Wallet.Instance.Coins.ToString();
        }
    }
}