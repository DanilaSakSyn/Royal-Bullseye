using System.Collections;
// using DG.Tweening;
using UnityEngine;

public class LoaderView : MonoBehaviour
{
    private Coroutine _loadingCoroutine;

    private bool _canRotate;

    private void OnEnable()
    {
        _loadingCoroutine = StartCoroutine(DelayLoading());
    }

    private void OnDisable()
    {
        StopCoroutine(_loadingCoroutine);
    }

    private IEnumerator DelayLoading()
    {
        int sec = 20;
        _canRotate = true;
        while (sec-- > 0 && _canRotate)
        {
            float elapsed = 0f;
            float duration = 1.1f;
            float startZ = gameObject.transform.localRotation.eulerAngles.z;
            float endZ = startZ + 350f;
            while (elapsed < duration)
            {
                float z = Mathf.Lerp(startZ, endZ, elapsed / duration);
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, endZ);
            yield return null;
        }
    }
}
