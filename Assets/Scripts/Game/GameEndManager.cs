using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.UI;

namespace Game
{
    public class GameEndManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject winScreen;
        [SerializeField] private List<GameObject> starsObjects;
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private UnityEngine.UI.Button restartButton;
        [SerializeField] private UnityEngine.UI.Button menuButton;
        [SerializeField] private StarPanel starPanel;
        [SerializeField] private TMPro.TextMeshProUGUI coinsText;
        
        [Space(20)]
        [SerializeField] private RandomSpawner spawner;

        private void Awake()
        {
            winScreen.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);
            menuButton.onClick.AddListener(GoToMenu);
        }

        public void ShowWinScreen()
        {
            spawner.Stop();
            winScreen.SetActive(true);
            int stars = starPanel != null ? starPanel.GetCurrentStars() : 0;
            int score = GameScoreManager.Instance != null ? GameScoreManager.Instance.GetScore() : 0;
            int coinsReward = stars * Random.Range(5, 16); // случайное количество монет за звезду
            if (Game.Wallet.Wallet.Instance != null)
            {
                Game.Wallet.Wallet.Instance.AddCoins(coinsReward);
            }
            for (int i = 0; i < starsObjects.Count; i++)
            {
                if (i < stars)
                    starsObjects[i].SetActive(true);
                else
                {
                    starsObjects[i].SetActive(false);
                }
            }
            scoreText.text = $"{score}";
            if (coinsText != null)
                coinsText.text = coinsReward.ToString();
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void GoToMenu()
        {
            SceneManager.LoadScene("Menu"); // Название сцены меню
        }
    }
}
