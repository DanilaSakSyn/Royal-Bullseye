using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class StarPanel : MonoBehaviour
    {
        [Header("Slider & Stars")]
        public Slider scoreSlider;
        public GameObject[] stars; // 3 звезды
        public Color[] sliderColors; // 3 цвета для слайдера
        public int[] starThresholds; // 3 значения для получения звезды

        public int currentScore = 0;
        private int maxStars = 3;
        private int currentStars = 0;

        private void Start()
        {
            scoreSlider.value = 0;
            scoreSlider.maxValue = starThresholds[maxStars - 1];
            for (int i = 0; i < stars.Length; i++)
                stars[i].SetActive(false);
            scoreSlider.fillRect.GetComponent<Image>().color = sliderColors[0];
        }

        public void AddScore(int score)
        {
            currentScore += score;
            scoreSlider.value = Mathf.Clamp(currentScore, 0, scoreSlider.maxValue);
            UpdateStarsAndColor();
        }

        private void UpdateStarsAndColor()
        {
            for (int i = 0; i < maxStars; i++)
            {
                if (currentScore >= starThresholds[i])
                {
                    if (!stars[i].activeSelf)
                        stars[i].SetActive(true);
                    scoreSlider.fillRect.GetComponent<Image>().color = sliderColors[i];
                    currentStars = i + 1;
                }
            }
        }

        public int GetCurrentStars()
        {
            return currentStars;
        }
    }
}

