using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    public enum AudioName : int {
        EnemyHitByRock,
        GameOver,
        Empower1,
        Explosion1,
        Explosion2,
        Explosion3
    }

    //instance for conveniently get gameObject in this script
    private static AudioManager instance;

    public static AudioMixerGroup audioGroup;
    public AudioClip[] audioClips;
    public static List<AudioSource> pooledSources;
    public static int pooledAmount = 10;
    public static float lowPitchRange = 0.95f, highPitchRange = 1.05f;
    public static bool canGrow = true;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

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

    public static void PlaySound(params AudioName[] name) {
        foreach (AudioName clipName in name) {
            if (instance.audioClips[(int)clipName]) {
                instance.GetAudioSoucre().PlayOneShot(instance.audioClips[(int)clipName]);
            } else {
                print("AudioManager : AudioClip[" + name.ToString() + "] has not been set");
            }
        }
    }

    public static void PlaySoundRandomPitch(params AudioName[] name) {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        foreach (AudioName clipName in name) {
            if (instance.audioClips[(int)clipName]) {
                AudioSource source = instance.GetAudioSoucre();

                source.pitch = randomPitch;
                source.PlayOneShot(instance.audioClips[(int)clipName]);
            } else {
                print("AudioManager : AudioClip[" + name.ToString() + "] has not been set");
            }
        }
    }

    public static bool HaveAudio(string s) {
        if (System.Enum.IsDefined(typeof(AudioName), s))
            return true;
        else
            return false;
    }

    private AudioSource GetAudioSoucre()
    {
        //pick the first AudioSource which is not playing
        for (int i = 0; i < pooledSources.Count; i++)
        {
            if (!pooledSources[i].isPlaying)
            {
                return pooledSources[i];
            }
        }

        //add a new AudioSource
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

}
