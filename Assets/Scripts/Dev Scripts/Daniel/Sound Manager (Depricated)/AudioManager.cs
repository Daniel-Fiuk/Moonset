using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    /*----------Play Sounds----------*/
    #region Play Sounds

    /*--Play--*/
    #region Play

    public static void Play(AudioSource audioSource, AudioClipObj audioClipObj) //Plays the audio
    {
        //Setup the audio source
        //If the audio source is invalid, quit Operation
        if (!SetUpAudioSource(audioSource, audioClipObj)) return;

        //Play the audio
        audioSource.Play();
    }
    
    public static void Play(AudioSource audioSource, AudioClipObj audioClipObj, PlaybackType playbackType, float playbackTime) //Plays the audio at a spesific time
    {
        //Setup the audio source
        //If the audio source is invalid or the playback input is invalid, quit Operation
        if (!SetUpAudioSource(audioSource, audioClipObj) || !SetPlayback(audioSource, playbackType, playbackTime)) return;
        
        //Play the audio
        audioSource.Play();
    }

    #endregion

    /*--Play Additive--*/
    #region Play Additive

    public static void PlayAdditive(GameObject audioSourceParent, AudioClipObj audioClipObj) //Adds a new Audio Source to a parent and plays the audio
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSourceParent.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, audioClipObj))
        {
            //Destroy the new audio source
            Object.Destroy(newAudioSource);
            return;
        };

        //Play the audio
        newAudioSource.Play();
    }

    public static void PlayAdditive(GameObject audioSourceParent, AudioClipObj audioClipObj, PlaybackType playbackType, float playbackTime) //Adds a new Audio Source to a parent and plays the audio at a spesific time
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSourceParent.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid or the playback input is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, audioClipObj) || !SetPlayback(newAudioSource, playbackType, playbackTime))
        {
            //Destroy the new audio source
            Object.Destroy(newAudioSource);
            return;
        };

        //Play the audio
        newAudioSource.Play();
    }

    #endregion

    /*--Fade In--*/
    #region Fade In

    public static IEnumerator FadeIn(AudioSource audioSource, AudioClipObj audioClipObj, float duration) //Fades in the audio
    {
        //Setup the audio source
        //If the audio source is invalid, quit Operation
        if (!SetUpAudioSource(audioSource, audioClipObj)) yield break;

        float endVolume = audioSource.volume;
        audioSource.volume = 0f;

        //Play the audio
        audioSource.Play();

        float t = 0f;
        while (t < 1f)
        {
            //Incrimentaly increase the volume over time until it is equal to the audio clip object's set volume
            audioSource.volume = Mathf.Lerp(0f, endVolume, t);

            t += Time.unscaledDeltaTime / duration;

            yield return new WaitForEndOfFrame();
        }

        //Set the final volume of the source
        audioSource.volume = audioClipObj.volume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, AudioClipObj audioClipObj, float duration, PlaybackType playbackType, float playbackTime) //Fades in the audio
    {
        //Setup the audio source
        //If the audio source is invalid or the playback input is invalid, quit Operation
        if (!SetUpAudioSource(audioSource, audioClipObj) || !SetPlayback(audioSource, playbackType, playbackTime)) yield break;

        float endVolume = audioSource.volume;
        audioSource.volume = 0f;

        //Play the audio
        audioSource.Play();

        float t = 0f;
        while (t < 1f)
        {
            //Incrimentaly increase the volume over time until it is equal to the audio clip object's set volume
            audioSource.volume = Mathf.Lerp(0f, endVolume, t);

            t += Time.unscaledDeltaTime / duration;

            yield return new WaitForEndOfFrame();
        }

        //Set the final volume of the source
        audioSource.volume = audioClipObj.volume;
    }

    #endregion

    /*--Additive Fade In--*/
    #region Fade In Additive

    public static IEnumerator FadeInAdditive(GameObject audioSourceParent, AudioClipObj audioClipObj, float duration) //Adds a new Audio Source to the parent and fades in the audio
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSourceParent.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, audioClipObj)) yield break;

        float endVolume = newAudioSource.volume;
        newAudioSource.volume = 0f;

        //Play the audio
        newAudioSource.Play();

        float t = 0f;
        while (t < 1f)
        {
            //Incrimentaly increase the volume over time until it is equal to the audio clip object's set volume
            newAudioSource.volume = Mathf.Lerp(0f, endVolume, t);

            t += Time.unscaledDeltaTime / duration;

            yield return new WaitForEndOfFrame();
        }

        //Set the final volume of the source
        newAudioSource.volume = audioClipObj.volume;
    }

    public static IEnumerator FadeInAdditive(GameObject audioSourceParent, AudioClipObj audioClipObj, float duration, PlaybackType playbackType, float playbackTime) //Adds a new Audio Source to the parent and fades in the audio at a spesific time
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSourceParent.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid or the playback input is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, audioClipObj) || !SetPlayback(newAudioSource, playbackType, playbackTime)) yield break;

        float endVolume = newAudioSource.volume;
        newAudioSource.volume = 0f;

        //Play the audio
        newAudioSource.Play();

        float t = 0f;
        while (t < 1f)
        {
            //Incrimentaly increase the volume over time until it is equal to the audio clip object's set volume
            newAudioSource.volume = Mathf.Lerp(0f, endVolume, t);

            t += Time.unscaledDeltaTime / duration;

            yield return new WaitForEndOfFrame();
        }

        //Set the final volume of the source
        newAudioSource.volume = audioClipObj.volume;
    }

    #endregion

    #endregion

    /*----------Stop Sounds---------*/ //TODO: Add playback type and time to stop sounds
    #region Stop Sounds

    /*--Stop--*/
    #region Stop

    public static void Stop(AudioSource audioSource) //Stops the audio
    {
        //Stop the audio
        audioSource.Stop();
    }

    public static IEnumerator Stop(AudioSource audioSource, PlaybackType playbackType, float playbackTime) //Stops the audio at a spesific time
    {
        if (playbackType == PlaybackType.Time) yield return new WaitUntil(() => audioSource.time >= playbackTime);
        
        else if (playbackType == PlaybackType.TimeSamples)
        {
            //Try conversion to Int and set time samples
            if (int.TryParse(playbackTime.ToString(), out int playbackTimeSamples)) audioSource.timeSamples = playbackTimeSamples;
            else { Debug.LogWarning($"Playback time {playbackTime} is an invalid input for the playback type :{playbackType}:!"); yield return false; } //Failed Operation
        }

        else { Debug.LogWarning($"{playbackType} is an invalid playback type!"); yield return false; } //Failed Operation

        //Stop the audio
        audioSource.Stop();
    }

    #endregion

    /*--Stop Subtractive--*/
    #region Stop Subtractive

    public static void StopSubtractive(AudioSource audioSource) //Stops the audio and destroys the audio source component
    {
        //Stop the audio and destroy the audio source
        audioSource.Stop();
        Object.Destroy(audioSource);
    }

    #endregion

    /*--Fade Out--*/
    #region Fade Out
    
    public static IEnumerator FadeOut(AudioSource audioSource, float duration) //Fades the audio out
    {
        //Save the original Volume
        float startVolume = audioSource.volume;

        float t = 0f;
        while (t < 1f)
        {
            //Incrimentaly decrease the volume over time until it is silent
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t);
            
            t += Time.unscaledDeltaTime / duration;
            
            yield return new WaitForEndOfFrame();
        }

        //Stop the audio and reset the volume
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    #endregion

    /*--Fade Out Subtractive--*/
    #region Fade Out Subtractive

    public static IEnumerator FadeOutSubtractive(AudioSource audioSource, float duration) //Fades the audio out and destroys the audio source component
    {
        //Save the original Volume
        float startVolume = audioSource.volume;

        float t = 0f;
        while (t < 1f)
        {
            //Incrimentaly decrease the volume over time until it is silent
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t);

            t += Time.unscaledDeltaTime / duration;

            yield return new WaitForEndOfFrame();
        }

        //Stop and destroy the audio
        audioSource.Stop();
        GameObject.Destroy(audioSource);
    }

    #endregion

    #endregion

    /*----------Transition Sounds----------*/
    #region Transition Sounds

    /*--Transition Imidiate--*/
    #region Transition Imidiate

    public static void TransitionImidiate(AudioSource audioSource, AudioClipObj newAudioClipObj) //Swaps the audio clip of the audio source
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSource.gameObject.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, newAudioClipObj))
        {
            //Destroy the new audio source
            Object.Destroy(newAudioSource);
            return;
        }

        //Play the new audio source
        newAudioSource.Play();

        //Stop and destroy the old audio source
        audioSource.Stop();
        Object.Destroy(audioSource);
    }

    public static void TransitionImidiate(AudioSource audioSource, AudioClipObj newAudioClipObj, PlaybackType playbackType, float playbackTime) //Swaps the audio clip of the audio source at a spesific time
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSource.gameObject.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid or the playback input is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, newAudioClipObj) || !SetPlayback(audioSource, playbackType, playbackTime))
        {
            //Destroy the new audio source
            Object.Destroy(newAudioSource);
            return;
        }

        //Play the new audio source
        newAudioSource.Play();

        //Stop and destroy the old audio source
        audioSource.Stop();
        Object.Destroy(audioSource);
    }

    #endregion

    /*--Transition--*/
    #region Transition

    public static IEnumerator Transition(AudioSource audioSource, AudioClipObj newAudioClipObj, float duration) //Swaps the audio clip of the audio source with a fade
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSource.gameObject.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, newAudioClipObj)) 
        {
            //Destroy the new audio source
            Object.Destroy(newAudioSource);
            yield break;
        }

        //Save the volume of both audio sources
        float startVolumeAS = audioSource.volume;
        float endVolumeNAS = newAudioSource.volume;

        //Play the audio
        newAudioSource.Play();

        float t = 0f;
        while (t < 0)
        {
            //Incrimentaly transition the volume over time
            audioSource.volume = Mathf.Lerp(startVolumeAS, 0f, t);
            newAudioSource.volume = Mathf.Lerp(0f, endVolumeNAS, t);

            t += Time.unscaledDeltaTime / duration;

            yield return new WaitForEndOfFrame();
        }

        //Stop and destroy the old audio source
        audioSource.Stop();
        Object.Destroy(audioSource);
    }

    public static IEnumerator Transition(AudioSource audioSource, AudioClipObj newAudioClipObj, float duration, PlaybackType playbackType, float playbackTime) //Swaps the audio clip of the audio source with a fade at a spesific time
    {
        //Add new Audio Source
        AudioSource newAudioSource = audioSource.gameObject.AddComponent<AudioSource>();

        //Setup the audio source
        //If the audio source is invalid or the playback input is invalid, quit Operation
        if (!SetUpAudioSource(newAudioSource, newAudioClipObj) || !SetPlayback(audioSource, playbackType, playbackTime))
        {
            //Destroy the new audio source
            Object.Destroy(newAudioSource);
            yield break;
        }

        //Save the volume of both audio sources
        float startVolumeAS = audioSource.volume;
        float endVolumeNAS = newAudioSource.volume;

        //Play the audio
        newAudioSource.Play();

        float t = 0f;
        while (t < 0)
        {
            //Incrimentaly transition the volume over time
            audioSource.volume = Mathf.Lerp(startVolumeAS, 0f, t);
            newAudioSource.volume = Mathf.Lerp(0f, endVolumeNAS, t);

            t += Time.unscaledDeltaTime / duration;

            yield return new WaitForEndOfFrame();
        }

        //Stop and destroy the old audio source
        audioSource.Stop();
        Object.Destroy(audioSource);
    }

    #endregion

    #endregion

    /*----------Setup Audio Source----------*/
    #region Setup Audio Source

    private static bool SetUpAudioSource(AudioSource audioSource, AudioClipObj audioClipObj)
    {
        //Check if the scriptable object is valid
        if (audioClipObj == null) { Debug.LogWarning($"{audioClipObj.name} is null!"); return false; } //Failed Operation
        if (audioClipObj.audioClip == null) { Debug.LogWarning($"{audioClipObj.name}'s audio clip is null!"); return false; } //Failed Operation

        //Set the audio source settings to that of the scriptable object
        audioSource.clip = audioClipObj.audioClip;
        audioSource.loop = audioClipObj.loop;
        audioSource.volume = audioClipObj.volume;
        audioSource.pitch = audioClipObj.pitch;

        //If we want to use random volume or pitch, we set the volume or pitch to a random value between the min and max values set in the scriptable object
        if (audioClipObj.useRandomVolume) audioSource.volume = Random.Range(audioClipObj.randomVolumeRange.x, audioClipObj.randomVolumeRange.y);
        if (audioClipObj.useRandomPitch) audioSource.pitch = Random.Range(audioClipObj.randomPitchRange.x, audioClipObj.randomPitchRange.y);

        return true; //Successful Operation
    } //Sets up audio sources, returns false if the audio clip object is invalid

    private static bool SetPlayback(AudioSource audioSource, PlaybackType playbackType, float playbackTime)
    {
        //Seek to specified time
        if (playbackType == PlaybackType.Time) audioSource.time = playbackTime;

        else if (playbackType == PlaybackType.TimeSamples) 
        {
            //Try conversion to Int and set time samples
            if (int.TryParse(playbackTime.ToString(), out int playbackTimeSamples)) audioSource.timeSamples = playbackTimeSamples;
            else { Debug.LogWarning($"Playback time {playbackTime} is an invalid input for the playback type :{playbackType}:!"); return false; } //Failed Operation
        }

        else { Debug.LogWarning($"{playbackType} is an invalid playback type!"); return false; } //Failed Operation

        return true; //Successful Operation
    }

    #endregion
}

public enum PlaybackType
{
    Time,
    TimeSamples
}

public enum StopFade //TODO: Find Better Name
{
    Start,
    End
}