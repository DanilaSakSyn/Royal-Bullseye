using UnityEngine;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {
        public AudioClip[] soundEffects;
        private static SoundManager instance;
        private AudioSource audioSource;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                audioSource = gameObject.AddComponent<AudioSource>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Воспроизводит звуковой эффект по индексу из массива soundEffects.
        /// </summary>
        public static void PlaySound(int index, bool randomPitch = false)
        {
            if (instance == null || instance.soundEffects == null || index < 0 || index >= instance.soundEffects.Length)
                return;
            if (randomPitch)
                instance.audioSource.pitch = Random.Range(0.9f, 1.1f);
            else
                instance.audioSource.pitch = 1f;
            instance.audioSource.PlayOneShot(instance.soundEffects[index]);
        }
    }
}
