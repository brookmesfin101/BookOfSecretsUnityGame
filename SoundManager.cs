using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundsToPlay
{
    teleport
}

public class SoundManager : MonoBehaviour {

    private static SoundManager instance;

    AudioSource audioSource;

    [SerializeField]
    AudioClip teleportSound;

    [SerializeField]
    AudioClip jumpSound;

    public static SoundManager Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(SoundsToPlay sound)
    {
        switch(sound)
        {
            case SoundsToPlay.teleport: audioSource.PlayOneShot(teleportSound); break;
        }
    }


}
