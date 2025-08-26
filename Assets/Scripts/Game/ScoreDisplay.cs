using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Update()
        {
            if (GameScoreManager.Instance != null)
            {
                scoreText.text = $"Score: {GameScoreManager.Instance.GetScore()}";
            }
        }
    }
}

