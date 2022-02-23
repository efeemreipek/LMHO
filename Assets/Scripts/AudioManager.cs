using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public List<AudioClip> femaleLaughs;
    public List<AudioClip> maleLaughs;
    public AudioClip booEffect;
    public AudioClip deathEffect;
    public AudioClip scoreEffect;
}
