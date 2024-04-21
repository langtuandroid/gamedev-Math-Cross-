using UnityEngine;
using UnityEngine.Audio;

namespace stickin
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundItem : MonoBehaviour
    {
        private AudioSource _audioSource;

        public bool IsPlaying => _audioSource?.isPlaying ?? false;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play(SoundConfig data, AudioMixerGroup group)
        {
            _audioSource.clip = data.Clip;
            _audioSource.loop = data.Loop;
            _audioSource.outputAudioMixerGroup = group;
            _audioSource.volume = data.Volume;
            _audioSource.Play();
        }
    }
}