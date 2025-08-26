using UnityEngine;
using System.Collections;

namespace Game
{
    public class SmoothMover : MonoBehaviour
    {
        private static SmoothMover instance;
        private Coroutine moveCoroutine;

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Плавно перемещает указанный объект в целевую точку за 0.2 секунды.
        /// </summary>
        public static void MoveTo( Vector3 targetPosition)
        {
            if (instance == null) return;
            SmoothMover mover = instance;
            if (mover.moveCoroutine != null)
                mover.StopCoroutine(mover.moveCoroutine);
            mover.moveCoroutine = mover.StartCoroutine(mover.MoveRoutine(instance.transform, targetPosition));
        }

        private IEnumerator MoveRoutine(Transform objTransform, Vector3 targetPosition)
        {
            float duration = 0.2f;
            float elapsed = 0f;
            Vector3 startPos = objTransform.position;
            while (elapsed < duration)
            {
                objTransform.position = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            objTransform.position = targetPosition;
            moveCoroutine = null;
        }
    }
}

