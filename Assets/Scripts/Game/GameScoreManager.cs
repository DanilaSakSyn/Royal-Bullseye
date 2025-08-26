using Game.UI;
using UnityEngine;

namespace Game
{
    public class GameScoreManager : MonoBehaviour
    {
        public static GameScoreManager Instance { get; private set; }
        private int score = 0;
        private bool gameEnded = false;
        
        [SerializeField] private StarPanel starPanel;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
           
        }

        public void AddScore(int amount)
        {
            if (gameEnded) return;
            score += amount;
            starPanel.AddScore(amount);
        }

        public int GetScore()
        {
            return score;
        }

        public void EndGame()
        {
            gameEnded = true;
            Debug.Log($"Game Over! Your score: {score}");
        }
    }
}

