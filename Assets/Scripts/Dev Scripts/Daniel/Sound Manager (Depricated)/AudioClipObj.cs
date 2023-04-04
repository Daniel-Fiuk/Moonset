using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Clip Object", menuName = "ScriptableObjs/Audio/Audio Clip Object", order = 0)]

public class AudioClipObj : ScriptableObject
{
    #region inspector

    [Header("Main")] public AudioClip audioClip;
    public bool loop;
    public float volume = 1f;
    public float pitch = 1f;

    [Header("Random Volume")] public bool useRandomVolume;
    public float2 randomVolumeRange = new float2(0.9f, 1.1f);
    
    [Header("Random Pitch")] public bool useRandomPitch;
    public float2 randomPitchRange = new float2(0.9f, 1.1f);

    #endregion
}
