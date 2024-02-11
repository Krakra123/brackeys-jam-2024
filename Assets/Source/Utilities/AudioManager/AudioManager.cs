using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioManager : GenericSingleton<AudioManager>
{
    [SerializeField]
    private SoundEntity[] soundList = default;

    private Dictionary<GameSound, List<SoundEntity>> soundMap = new Dictionary<GameSound, List<SoundEntity>>();

    public AudioSource PlaySound(GameSound soundType, float volumeMultiplier = 1f, float pitchMultiplier = 1f)
        => PlaySoundInternal(soundType, volumeMultiplier, pitchMultiplier);
    private AudioSource PlaySoundInternal(GameSound _soundType, float _volumeMultiplier, float _pitchMultiplier)
    {
        List<SoundEntity> _list = soundMap[_soundType];
        SoundEntity _soundEntity = _list[Random.Range(0, _list.Count)];

        AudioSource _audioSource = new GameObject().AddComponent<AudioSource>();

        _audioSource.name = _soundEntity.Name;
        _audioSource.transform.parent = transform;
        _audioSource.clip = _soundEntity.audioClip;
        _audioSource.outputAudioMixerGroup = _soundEntity.audioMixer;
        _audioSource.volume = Random.Range(_soundEntity.volumeLow, _soundEntity.volumeHigh) * _volumeMultiplier;
        _audioSource.pitch = Random.Range(_soundEntity.pitchLow, _soundEntity.pitchHigh) * _pitchMultiplier;
        _audioSource.loop = _soundEntity.loop;

        _audioSource.Play();
        if (!_soundEntity.loop) StartCoroutine(DestroyFinishedSound(_audioSource, _soundEntity.audioClip.length));

        return _audioSource;
    }

    private void PopulateSoundMap()
    {
        if (soundList == null || soundList.Length == 0) return;

        soundMap.Clear();
        foreach (var _sound in soundList)
        {
            if (_sound.soundType == GameSound.None) 
            {
                continue;
            }

            if (_sound.audioClip == null)
            {
                Debug.LogWarning(_sound.Name + " has no audio clip.");
                continue;
            }

            if (soundMap.TryGetValue(_sound.soundType, out var list))
            {
                list.Add(_sound);
            }
            else
            {
                soundMap.Add(_sound.soundType, new List<SoundEntity>() { _sound });
            }

            _sound.SetName();
        }
    }

    private IEnumerator DestroyFinishedSound(AudioSource _audioSource, float _length)
    {
        yield return new WaitForSeconds(_length);

        Destroy(_audioSource.gameObject);
    }

    protected override void Awake()
    {
        base.Awake();

        PopulateSoundMap();
    }

    // #if UNITY_EDITOR    
    // private void OnValidate()
    // {
    //     foreach (SoundEntity _sound in soundList)
    //     {
    //         if (_sound != null)
    //             _sound.SetName();
    //     }

    //     if (EditorApplication.isPlaying)
    //     {
    //         PopulateSoundMap();
    //     }
    // }
    // #endif
}
