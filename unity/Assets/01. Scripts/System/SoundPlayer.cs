using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    // Singleton

    static SoundPlayer _instance;
    public static SoundPlayer Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = FindObjectOfType<SoundPlayer>();
                if(null == _instance)
                {
                    GameObject go = new GameObject();
                    go.name = "SoundPlayer";
                    _instance = go.AddComponent<SoundPlayer>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Player

    AudioSource _audioSource;

    public void PlayEffect(string soundName)
    {
        string filePath = "Sounds/Effects/" + soundName;
        AudioClip clip = Resources.Load<AudioClip>(filePath);
        if(null != clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
