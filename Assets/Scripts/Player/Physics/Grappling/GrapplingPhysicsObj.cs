using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Grappling Physics Object", menuName = "ScriptableObjs/Physics/Grappling Hook", order = 1)]

public class GrapplingPhysicsObj : ScriptableObject
{
    [Header("Prefabs")]

    public GameObject hookPrefab;

    [Header("Properties")]

    public float minLength;
    public float maxLength;

    [Range(0, 1)] public float maxDistanceMultiple, minDistanceMultiple;

    public float springForce, damperForce;
    public float massScale;
    public float lungeForce, lungeGravityComp;
    public float scaleSpeed;

    [Header("Animation")]

    public float drawSpeed;
    [Range(1, 32)] public int sampleRate;
    public float curveSize;
    public AnimationCurve curveShape;

    [Header("Audio")]

    [Space] public AudioClip[] hookSounds;
    public float hookVolume;
    public float2 hookPitchRange;
    
    [Space] public AudioClip[] zipSounds;
    public float zipVolume;
    public float2 zipPitchRange;

/*    [Space] public AudioClip[] whipSounds;
    public float whipVolume;
    public float2 whipPitchRange;*/

    [Space] public AudioClip[] whooshSounds;
    public float whooshVolume;
    public float2 whooshPitchRange;
}
