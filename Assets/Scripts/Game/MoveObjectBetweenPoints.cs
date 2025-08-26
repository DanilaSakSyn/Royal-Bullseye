using UnityEngine;

namespace Game
{
    public class MoveObjectBetweenPoints : MonoBehaviour
    {
        [Header("Move Settings")]
        public Transform startPoint;
        public Transform endPoint;
        public GameObject objectPrefab;
        public float moveDuration = 2f;

        public GameEndManager gameEndManager;
        private GameObject movingObject;
        private float timer = 0f;
        private bool isMoving = false;

        public void SpawnAndMove()
        {
            if (objectPrefab != null && startPoint != null && endPoint != null)
            {
                movingObject = Instantiate(objectPrefab, startPoint.position, Quaternion.identity);
                timer = 0f;
                isMoving = true;
            }
        }

        private void Update()
        {
            if (isMoving && movingObject != null)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / moveDuration);
                movingObject.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
                if (t >= 1f)
                {
                    isMoving = false;
                    gameEndManager.ShowWinScreen();
                }
            }
        }
    }
}

