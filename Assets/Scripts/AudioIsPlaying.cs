using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DmitryAdventure
{
    public enum SoundType
    {
        /// <summary>
        /// Sound is played on failure.
        /// </summary>
        Negative,  
        /// <summary>
        /// Sound is played on success or a neutral situation.
        /// </summary>
        Positive
    }
    
    
    
    /// <summary>
    /// Represents an entity that plays audio
    /// </summary>
    public class AudioIsPlaying : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [field: SerializeField] private AudioClip NegativeClipToPlay { get; set; }
        [field: SerializeField] private AudioClip PositiveClipToPlay { get; set; }
        private AudioSource NegativeSourceToPlay { get; set; }
        private AudioSource PositiveSourceToPlay { get; set; }

        public delegate void AudioTriggerHandler(bool isSoundPlayed);  
        public event AudioTriggerHandler? AudioTriggerNotify;

        private Coroutine playCoroutine;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            if (NegativeClipToPlay != null)
            {
                var sourceToPlay = gameObject.AddComponent<AudioSource>();
                MakeAudio(NegativeClipToPlay, ref sourceToPlay);
                NegativeSourceToPlay = sourceToPlay;
            }
            
            if (PositiveClipToPlay != null)
            {
                var sourceToPlay = gameObject.AddComponent<AudioSource>();
                MakeAudio(PositiveClipToPlay, ref sourceToPlay);
                PositiveSourceToPlay = sourceToPlay;
            }
  
        }
        #endregion
        
        #region Coroutines

        private IEnumerator PlayCoroutine(SoundType type,  float volume)
        {
            switch (type)
            {
                case SoundType.Negative:
                    NegativeSourceToPlay.volume = volume;
                    NegativeSourceToPlay.Play();
                    yield return new WaitWhile (()=> NegativeSourceToPlay.isPlaying);
                    break;
                case SoundType.Positive:
                    PositiveSourceToPlay.volume = volume;
                    PositiveSourceToPlay.Play();
                    yield return new WaitWhile (()=> PositiveSourceToPlay.isPlaying);
                    break;
            }
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
        public void PlaySound(SoundType type, float volume = 1.0f)
        {
            playCoroutine = StartCoroutine(PlayCoroutine(type, volume));
        }

        #endregion
    }
}