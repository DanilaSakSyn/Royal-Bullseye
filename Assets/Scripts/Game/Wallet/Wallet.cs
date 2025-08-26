using UnityEngine;

namespace Game.Wallet
{
    public class Wallet : MonoBehaviour
    {
        public static Wallet Instance { get; private set; }
        private const string CoinsKey = "WalletCoins";
        private int coins;

        public int Coins => coins;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            coins = PlayerPrefs.GetInt(CoinsKey, 0);
        }

        public void AddCoins(int amount)
        {
            coins += amount;
            PlayerPrefs.SetInt(CoinsKey, coins);
            PlayerPrefs.Save();
        }
    }
}

