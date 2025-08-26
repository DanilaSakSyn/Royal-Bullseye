using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class ClickableTimerCircle : MonoBehaviour, IPointerClickHandler
    {
        [Header("Timer Settings")] [SerializeField]
        private float lifetime = 3f; // Time in seconds before auto-destruction

        [SerializeField] private Color startColor = Color.white;
        [SerializeField] private Color endColor = Color.red;
        [SerializeField] private Transform scaleEffect;

        [Header("Appear Animation")] [SerializeField]
        private float appearDuration = 0.5f;

        [SerializeField]
        private Transform animatedChild; // если scaleEffect — это нужный объект, можно использовать его

        [SerializeField] private SpriteRenderer spriteRenderer;
        private float currentTime;
        private bool isAlive = true;
        private MaterialPropertyBlock propertyBlock;
        private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
        private static readonly int FillColor = Shader.PropertyToID("_FillColor");


        private bool isClicked = false;
        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
            currentTime = lifetime;

            // Ensure the material supports the fill shader properties
            if (spriteRenderer != null)
            {
                // Get the current material to avoid modifying the shared material
                spriteRenderer.material = new Material(spriteRenderer.material);
                UpdateMaterialProperties(1f);
            }
        }

        private void OnEnable()
        {
            if (animatedChild == null) animatedChild = scaleEffect;
        }

        private System.Collections.IEnumerator AppearAnimation()
        {
            if (spriteRenderer == null || animatedChild == null) yield break;
            float t = 0f;
            Vector3 startPos = animatedChild.localPosition;
            Vector3 targetPos = Vector3.zero;
            Color color = spriteRenderer.color;
            float startAlpha = color.a;
            float targetAlpha = 1f;
            while (t < appearDuration)
            {
                t += Time.deltaTime;
                float progress = Mathf.Clamp01(t / appearDuration);
                animatedChild.localPosition = Vector3.Lerp(startPos, targetPos, progress);
                color.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
                spriteRenderer.color = color;
                yield return null;
            }

            animatedChild.localPosition = targetPos;
            color.a = targetAlpha;
            spriteRenderer.color = color;
            DestroySelf();
        }

        private void Update()
        {
            if (!isAlive) return;

            // Update the timer
            currentTime -= Time.deltaTime;
            float fillAmount = currentTime / lifetime;

            // Update the material properties
            UpdateMaterialProperties(fillAmount);

            // Check if time's up
            if (currentTime <= 0f)
            {
                DestroySelf();
            }
        }

        private void UpdateMaterialProperties(float fillAmount)
        {
            scaleEffect.localScale = Vector3.one * (1 + fillAmount);
            if (spriteRenderer == null) return;

            // Get the current property block
            spriteRenderer.GetPropertyBlock(propertyBlock);

            // Update fill amount (0-1)
            propertyBlock.SetFloat(FillAmount, fillAmount);

            // Update color based on fill amount
            Color currentColor = Color.Lerp(endColor, startColor, fillAmount);
            propertyBlock.SetColor(FillColor, currentColor);

            // Apply the property block
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }


        private void DestroySelf()
        {
          
            // Optional: Add destruction effect here
            // For example: Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Destroy the object
            Destroy(gameObject);
        }

        // Optional: Call this to manually set the lifetime
        public void SetLifetime(float newLifetime)
        {
            lifetime = newLifetime;
            currentTime = lifetime;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isAlive) return;
            if (isClicked) return;
            isClicked = true;
            isAlive = false;
            SoundManager.PlaySound(0,true);
            // Вычисляем очки: максимум 100, минимум 10
            int minScore = 10;
            int maxScore = 100;
            float fillAmount = currentTime / lifetime;
            int score = Mathf.RoundToInt(Mathf.Lerp(minScore, maxScore, fillAmount));
            GameScoreManager.Instance?.AddScore(score);
            // Спавним текст с количеством очков

            switch (score)
            {
                case > 83:
                    WorldTextSpawner.SpawnText($"Perfect!", transform.position + 1f * Vector3.up);
                    break;
                case > 70:
                    WorldTextSpawner.SpawnText($"Good!", transform.position + 1f * Vector3.up);
                    break;
            }

            SmoothMover.MoveTo(transform.position);
            WorldTextSpawner.SpawnText($"+{score}", transform.position);

            StartCoroutine(AppearAnimation());
        }
    }
}