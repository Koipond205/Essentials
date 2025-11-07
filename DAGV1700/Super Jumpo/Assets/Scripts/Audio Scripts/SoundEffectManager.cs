using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [Header("Assign Audio Sources in Inspector")]
    public List<AudioSource> audioSources = new List<AudioSource>();

    public static SoundEffectManager Instance;

    public void PlaySound(string clipName)
    {
        var source = audioSources.Find(a => a.name == clipName);
        if (source != null)
            source.Play();
        else
            Debug.LogWarning($"No AudioSource found with name {clipName}");
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
