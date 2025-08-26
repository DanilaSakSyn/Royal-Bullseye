using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RandomSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")] [SerializeField]
        private GameObject objectToSpawn; // Prefab to spawn

        [SerializeField] private float spawnRadius = 5f; // Radius of the spawn area
        [SerializeField] private int numberOfObjects = 10; // Number of objects to spawn
        [SerializeField] private float spawnInterval = 1f; // Time between spawns
        [SerializeField] private bool spawnOnStart = true; // Whether to start spawning automatically
        [SerializeField] private Vector3 spawnCenter = Vector3.zero; // Center of the spawn area

        [Header("Dynamic Interval Settings")] [SerializeField]
        private float minSpawnInterval = 0.2f;

        [SerializeField] private float maxSpawnInterval = 2f;
        [SerializeField] private int maxSpawnCount = 4;
        [SerializeField] private List<GameObject> spawnedObjects = new List<GameObject>();
        [SerializeField] private AnimationCurve intervalCurve;

        private Coroutine spawnCoroutine;

        private void Awake()
        {
            // Генерируем случайную синусоидальную кривую с шумом
            Keyframe[] keys = new Keyframe[5];
            for (int i = 0; i < keys.Length; i++)
            {
                float t = i / (float)(keys.Length - 1);
                float value = Mathf.Sin(t * Mathf.PI) * 0.5f + 0.5f + Random.Range(-0.15f, 0.15f);
                value = Mathf.Clamp01(value);
                keys[i] = new Keyframe(t, value);
            }

            intervalCurve = new AnimationCurve(keys);
        }

        private void Start()
        {
            if (spawnOnStart)
            {
                StartCoroutine(SpawnObjects());
            }
        }

        // Call this method to start spawning objects
        public void StartSpawning()
        {
            spawnCoroutine = StartCoroutine(SpawnObjects());
        }

        // Метод для удаления всех заспавненных объектов и остановки спавна
        public void ClearSpawnedObjects()
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
            foreach (var obj in spawnedObjects)
            {
                if (obj != null)
                    Destroy(obj);
            }
            spawnedObjects.Clear();
        }

        private IEnumerator SpawnObjects()
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                // Generate a random point within the circle
                Vector2 randomPoint = GetRandomPointInCircle(transform.position, spawnRadius);

                // Spawn the object at the random point
                var spawnedObject = Instantiate(objectToSpawn, randomPoint, Quaternion.identity);
                spawnedObjects.Add(spawnedObject);
                // Wait for the specified interval before spawning the next object
                float progress = numberOfObjects > 1 ? i / (float)(numberOfObjects - 1) : 0f;
                float curveValue = intervalCurve.Evaluate(progress);
                float dynamicInterval = Mathf.Lerp(minSpawnInterval, maxSpawnInterval, curveValue);

                yield return new WaitForSeconds(dynamicInterval);

                while (spawnedObjects.Count > maxSpawnCount)
                {
                    for (int j = 0; j < spawnedObjects.Count; j++)
                    {
                        if (spawnedObjects[j] == null)
                        {
                            spawnedObjects.RemoveAt(j);
                            break;
                        }
                    }

                    yield return null;
                }
            }
        }

        // Helper method to get a random point within a circle
        private Vector2 GetRandomPointInCircle(Vector2 center, float radius)
        {
            // Generate a random angle in radians
            float angle = Random.Range(0f, 2f * Mathf.PI);

            // Generate a random distance from the center (square root for uniform distribution)
            float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

            // Convert polar coordinates to Cartesian coordinates
            float x = center.x + distance * Mathf.Cos(angle);
            float y = center.y + distance * Mathf.Sin(angle);

            return new Vector2(x, y);
        }

        // Draw the spawn area in the editor for easier visualization
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(spawnCenter, spawnRadius);
        }

        public void Stop()
        {
            StopAllCoroutines();
            foreach (var obj in spawnedObjects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            spawnedObjects.Clear();
        }
    }
}