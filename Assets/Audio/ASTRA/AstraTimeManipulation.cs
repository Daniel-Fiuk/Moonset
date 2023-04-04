using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstraTimeManipulation : MonoBehaviour
{
    [HideInInspector] AstraTrack astra;
    [HideInInspector] List<AstraClip> clips;
    
    public float TimeScale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        astra = GetComponent<AstraTrack>();
        clips = astra.clips;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AstraClip clip in clips)
        {
            clip.clip.GetComponent<AudioSource>().pitch = TimeScale;
        }
    }
}
