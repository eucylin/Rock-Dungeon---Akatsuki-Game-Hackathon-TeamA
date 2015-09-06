using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public enum AudioName
    {
        EnemyHitByRock,
        GameOver,
    }

    public static AudioManager instance;

    public AudioMixerGroup audioGroup;
    public AudioClip[] audioClips;
    public List<AudioSource> pooledSources;
    public int pooledAmount = 10;
    public float lowPitchRange = 0.95f, highPitchRange = 1.05f;
    public bool canGrow = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
			Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        pooledSources = new List<AudioSource>();

        for (int i = 0; i < pooledAmount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.outputAudioMixerGroup = audioGroup;
            source.playOnAwake = false;
            pooledSources.Add(source);
        }
    }

    public AudioSource GetSoucre()
    {
        for (int i = 0; i < pooledSources.Count; i++)
        {
            if (!pooledSources[i].isPlaying)
            {
                return pooledSources[i];
            }
        }

        if (canGrow)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.outputAudioMixerGroup = audioGroup;
            source.playOnAwake = false;
            pooledSources.Add(source);
            return source;
        }

        return null;
    }

    public void PlaySound(params AudioName[] name)
    {
        foreach(AudioName clipName in name){
            if (audioClips[(int)clipName])
            {
                GetSoucre().PlayOneShot(audioClips[(int)clipName]);
            }
            else
            {
                print("AudioManager : AudioClip[" + name.ToString() + "] is not setted");
            }
        }
    }

    public void PlaySoundRandomPitch(params AudioName[] name)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        foreach (AudioName clipName in name)
        {
            if (audioClips[(int)clipName])
            {
                AudioSource source = GetSoucre();

                source.pitch = randomPitch;
                source.PlayOneShot(audioClips[(int)clipName]);
            }
            else
            {
                print("AudioManager : AudioClip[" + name.ToString() + "] is not setted");
            }
        }
    }


}
