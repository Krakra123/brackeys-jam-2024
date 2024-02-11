using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(
    fileName = "SoundEntity", menuName = "Scriptable Objects/SoundEntity", order = 1
)]
public class SoundEntity : ScriptableObject
{
    [HideInInspector]
    public string Name;
    public void SetName()
        => Name = soundType.ToString();

    public GameSound soundType;
    public AudioClip audioClip;
    public AudioMixerGroup audioMixer;

    public bool loop;

    [Range(0, 1)]
    public float volumeLow = 1f;
    [Range(0, 1)]
    public float volumeHigh = 1f;
    [Range(0, 2)]
    public float pitchLow = 1f;
    [Range(0, 2)]
    public float pitchHigh = 1f;
}