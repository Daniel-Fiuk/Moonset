using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstraConditionalClip : MonoBehaviour
{
    public AudioSource audioSource;
    public bool randomizePitch = false;
    public float randoimPitchVariation = 0.1f;
    private float basePitch;

    public bool useClipRandomization = false;
    public List<AudioClip> RandomizationClips;
    
    public void PlayOnce()
    {
        audioSource.loop = false;
        RandomizePitch();
        audioSource.Play();
    }
    
    public void PlayOnLoop()
    {
        audioSource.loop = true;
        RandomizePitch();
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
    
    public void RandomizeClip()
    {
        if (useClipRandomization)
        {
            audioSource.clip = RandomizationClips[Random.Range(0,RandomizationClips.Count)];
        }
    }

    public void RandomizePitch()
    {
        if (randomizePitch)
        {
            audioSource.pitch = basePitch + Random.Range(-randoimPitchVariation,randoimPitchVariation);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        basePitch = audioSource.pitch;
    }
}
