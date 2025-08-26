using TMPro;
using UnityEngine;

namespace Game
{
    public class WorldTextSpawner : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPrefab;
        [SerializeField] private Canvas worldCanvas;
        [SerializeField] private float defaultLifetime = 1.5f;
        [SerializeField] private float disappearDuration = 0.5f;
        [SerializeField] private Camera cam;
        private static WorldTextSpawner _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static void SpawnText(string text, Vector3 worldPosition, float lifetime = -1f)
        {
            if (_instance == null || _instance.textPrefab == null || _instance.worldCanvas == null)
                return;

            var spawnedText = Instantiate(_instance.textPrefab, _instance.worldCanvas.transform);
            spawnedText.text = text;

            // Переводим мировую позицию в экранную и затем в позицию канваса
            Vector2 screenPos = _instance.cam.WorldToScreenPoint(worldPosition);
            spawnedText.rectTransform.position = screenPos;

            float destroyTime = lifetime > 0 ? lifetime : _instance.defaultLifetime;
            _instance.StartCoroutine(_instance.DisappearAndDestroy(spawnedText, destroyTime));
        }

        private System.Collections.IEnumerator DisappearAndDestroy(TextMeshProUGUI text, float visibleTime)
        {
            float timer = 0f;
            float startAlpha = text.color.a;
            Vector3 startScale = text.rectTransform.localScale;
            yield return new WaitForSeconds(visibleTime - disappearDuration);
            float t = 0f;
            while (t < disappearDuration)
            {
                t += Time.deltaTime;
                float progress = Mathf.Clamp01(t / disappearDuration);
                // Scale down
                text.rectTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
                // Fade out
                Color c = text.color;
                c.a = Mathf.Lerp(startAlpha, 0f, progress);
                text.color = c;
                yield return null;
            }

            Destroy(text.gameObject);
        }
    }
}