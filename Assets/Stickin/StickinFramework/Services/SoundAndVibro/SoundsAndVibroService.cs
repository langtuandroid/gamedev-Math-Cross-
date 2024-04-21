using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace stickin
{
    public enum VibrationType
    {
        VeryLight,
        Light,
        Selection
    }
    
    public enum AudioGroup
    {
        Music,
        UI,
        Game
    }

    public class SoundsAndVibroService : BaseService
    {
        #region Constants
        private const float VOLUME_MIN = -80;
        private const float VOLUME_MAX = 0;
        private const string MUSIC_VOLUME_KEY = "musicVolume";
        private const string GAME_VOLUME_KEY = "gameVolume";
        private const string UI_VOLUME_KEY = "uiVolume";
        private const float VOLUME_CHANGE_DURATION = 1f;
        
        private const string SOUND_KEY = "SettingsIsSound";
        private const string MUSIC_KEY = "SettingsIsMusic";
        private const string VIBRATION_KEY = "SettingsIsVibration";
        #endregion

        #region Serialized Fields
        [SerializeField] private SoundsConfig _soundsConfig;
        [SerializeField] private SoundItem _soundItemPrefab;
        [SerializeField] private SoundItem _musicItem;
        
        [Header("Mixer")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioMixerGroup _musicAudioMixerGroup;
        [SerializeField] private AudioMixerGroup _uiAudioMixerGroup;
        [SerializeField] private AudioMixerGroup _gameAudioMixerGroup;
        #endregion

        #region Public Properties
        public bool IsSoundEnabled { get; private set; }
        public bool IsMusicEnabled { get; private set; }
        public bool IsVibrationEnabled { get; private set; }
        #endregion
        
        #region Private Properties
        private Dictionary<string, SoundConfig> _soundsMap = new Dictionary<string, SoundConfig>();
        private List<SoundItem> _itemsList = new List<SoundItem>();
        #endregion

        public override void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            base.Init(appData, callbackComplete);
            
            InjectService.Bind<SoundsAndVibroService>(this);
            
            IsSoundEnabled = PlayerPrefsExtensions.GetBool(SOUND_KEY);
            IsMusicEnabled = PlayerPrefsExtensions.GetBool(MUSIC_KEY);
            IsVibrationEnabled = PlayerPrefsExtensions.GetBool(VIBRATION_KEY);

            RefreshMixerGroupsVolume();
            InitSoundsMap();
            
            InitComplete(true);
        }
        
        #region Enable/Disable Methods
        public void SetSoundEnabled(bool value)
        {
            IsSoundEnabled = value;
            PlayerPrefsExtensions.SetBool(SOUND_KEY, value);
            RefreshMixerGroupsVolume();
        }

        public void SetMusicEnabled(bool value)
        {
            IsMusicEnabled = value;
            PlayerPrefsExtensions.SetBool(MUSIC_KEY, value);
            RefreshMixerGroupsVolume();
        }

        public void SetVibrationEnabled(bool value)
        {
            IsVibrationEnabled = value;
            PlayerPrefsExtensions.SetBool(VIBRATION_KEY, value);
        }
        #endregion
        
        #region Sounds
        public void PlaySound(string id, AudioGroup group = AudioGroup.Game)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (_soundsMap.ContainsKey(id))
                {
                    var item = GetFreeAudioSource(_itemsList, _soundItemPrefab);
                    item.Play(_soundsMap[id], GetGroup(group));
                }
                else
                    Debug.LogError($"Fail play sound 2d for id '{id}'");
            }
            else
                Debug.LogError("Fail play sound 2d -> id is null or empty");
        }
        
        public void PlayMusic(string id)
        {
            if (_soundsMap.ContainsKey(id))
                _musicItem.Play(_soundsMap[id], GetGroup(AudioGroup.Music));
            else
                Debug.LogError($"Fail play music with id -> {id}");
        }

        private void RefreshMixerGroupsVolume()
        {
            _audioMixer.SetFloat(MUSIC_VOLUME_KEY, GetVolume(MUSIC_VOLUME_KEY));
            _audioMixer.SetFloat(UI_VOLUME_KEY, GetVolume(UI_VOLUME_KEY));
            _audioMixer.SetFloat(GAME_VOLUME_KEY, GetVolume(GAME_VOLUME_KEY));
        }
        
        private void InitSoundsMap()
        {
            if (_soundsConfig != null)
            {
                foreach (var soundData in _soundsConfig.Sounds)
                {
                    if (_soundsMap.ContainsKey(soundData.Id))
                        Debug.LogError($"Duplicate sound config with id -> {soundData.Id}");
                    else
                        _soundsMap[soundData.Id] = soundData;
                }
            }
            else
                Debug.LogError("Fail init SoundsAndVibroService -> config is null");
        }
        private float GetVolume(string type)
        {
            var isEnabled = type == MUSIC_VOLUME_KEY ? IsMusicEnabled : IsSoundEnabled;
            return isEnabled ? VOLUME_MAX : VOLUME_MIN;
        }
        
        private void SetVolume(AudioMixer mixer, string name, float value)
        {
            mixer.SetFloat(name, value);
        }
        
        private AudioMixerGroup GetGroup(AudioGroup group)
        {
            if (group == AudioGroup.Music)
                return _musicAudioMixerGroup;

            if (group == AudioGroup.UI)
                return _uiAudioMixerGroup;

            return _gameAudioMixerGroup;
        }

        private SoundItem GetFreeAudioSource(List<SoundItem> list, SoundItem prefab)
        {
            foreach (var item in list)
            {
                if (!item.IsPlaying)
                    return item;
            }

            var newAudioSource = Instantiate(prefab, transform);
            list.Add(newAudioSource);
            return newAudioSource;
        }
        #endregion
        
        #region Vibration
        public void Vibration(VibrationType type)
        {
#if !UNITY_EDITOR
            if (IsVibrationEnabled)
            {
                if (type == VibrationType.VeryLight)
                    Taptic.VeryLight();
                else if (type == VibrationType.Light)
                    Taptic.Light();
                else if (type == VibrationType.Selection)
                    Taptic.Selection();
            }
#endif
        }
        #endregion
    }
}
