using UnityEngine;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable] // Allows me to edit my custom class. Unity doesn't natively allow editing custom classes
    public class NamedAudioClip // Custom class that takes in a string name and an audio clip which can be populated in the inspector because of System.Serializable
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudioClip[] audioClips; // An array of audio clips
    private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>(); // A dictionary to store entries of names and clips associated with it
    private AudioSource audioSource;

    private void Awake()
    {
        // Make sure only 1 instance of the AudioManager exists to prevent sounds from being duplicated when called
        if (Instance == null) // So if there's no AudioManager, make this script the AM
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // If another one exists, delete it
            return;
        }

        DontDestroyOnLoad(gameObject); // Allow this GameObject to exist throughout different scenes without being destroyed

        audioSource = gameObject.AddComponent<AudioSource>(); // Get the audio source component

        foreach (var namedClip in audioClips) // Loop through the audioClips array 
        {
            if (!audioClipDictionary.ContainsKey(namedClip.name)) // and if the dictionary doesn't already have an entry with that name and clip, add it
            {
                audioClipDictionary.Add(namedClip.name, namedClip.clip);
            }
            else // Else display an error saying it already exists
            {
                Debug.LogWarning($"Duplicate audio clip name found: {namedClip.name}. Only the first instance will be used.");
            }
        }
    }

    public void PlaySound(string name) // A function that can be called from other scripts with a String (name)
    {
        if (audioClipDictionary.TryGetValue(name, out AudioClip clip)) // Search the dictionary for the String given and the audio associated with it
        {
            audioSource.PlayOneShot(clip); // Play that "got" audio once
        }
        else // Else display a message to say it wasn't found
        {
            Debug.LogWarning($"Audio clip with name {name} not found.");
        }
    }

    public float GetSoundLength(string name) // A function that returns the length of a given sound
    {
        if (audioClipDictionary.TryGetValue(name, out AudioClip clip)) // Search the dictionary for the given audio name
        {
            return clip.length; // Return the length of the audio clip
        }

        Debug.LogWarning($"Audio clip with name {name} not found."); // Display a warning if the sound isn't found
        return 0f; // Return 0 if the sound wasn't found
    }
}
