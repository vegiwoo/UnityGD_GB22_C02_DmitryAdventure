using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents an entity that plays audio
    /// </summary>
    public class AudioIsPlaying : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [field: SerializeField] private AudioClip AudioClipToPlay { get; set; }
        private AudioSource AudioSourceToPlay { get; set; }

        public delegate void AudioTriggerHandler(bool isSoundPlayed);  
        public event AudioTriggerHandler? AudioTriggerNotify;

        private Coroutine playCoroutine;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            var sourceToPlay = gameObject.AddComponent<AudioSource>();
            MakeAudio(AudioClipToPlay, ref sourceToPlay);
            AudioSourceToPlay = sourceToPlay;
        }
        #endregion
        
        #region Coroutines

        private IEnumerator PlayCoroutine()
        {
            AudioSourceToPlay.Play();
            yield return new WaitWhile (()=> AudioSourceToPlay.isPlaying);
            AudioTriggerNotify?.Invoke(true);
        }
        #endregion
        
        #region Other methods

        /// <summary>
        /// Creates an AudioSource from provided AudioClip.
        /// </summary>
        /// <param name="clip">AudioClip to create an AudioSource.</param>
        /// <param name="audioSource">AudioSource to play audio</param>
        private static void MakeAudio(in AudioClip clip, ref AudioSource audioSource)
        {
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
            audioSource.pitch = Random.Range(0.85f, 1.0f);
            audioSource.panStereo = audioSource.spatialBlend = audioSource.spread = 0;
            audioSource.volume = audioSource.reverbZoneMix = audioSource.dopplerLevel = 1;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.minDistance = 1;
            audioSource.maxDistance = 500;
        }

        /// <summary>
        /// Starts audio playback.
        /// </summary>
        public void PlaySound()
        {
            playCoroutine = StartCoroutine(PlayCoroutine());
        }

        #endregion
    }
}