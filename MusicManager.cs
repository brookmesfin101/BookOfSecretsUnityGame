using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    [SerializeField]
    private AudioClip[] trackToPlay;

    private AudioSource musicPlayer;

    static private MusicManager instance;

    enum FadeOutState
    {
        FadeOutNotStarted,
        FadeOutComplete
    }

    FadeOutState currentFadeState;

    [SerializeField]
    private float fadeSpeed;

    public static MusicManager Instance
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
        musicPlayer = GetComponent<AudioSource>();
        Instance = this;
        ChangeTrack();
        GameObject.DontDestroyOnLoad(this.gameObject);        
    }

    // Use this for initialization
    void Start () {
        currentFadeState = FadeOutState.FadeOutNotStarted;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeTrack()
    {        
        switch (SceneManager.GetActiveScene().name)
        {            
            case "_OpeningMenu": musicPlayer.clip = trackToPlay[0]; break;
            case "_Level-1": musicPlayer.clip = trackToPlay[1]; break;
            case "_Level-2": musicPlayer.clip = trackToPlay[2]; break;
            case "_Level-3": musicPlayer.clip = trackToPlay[3]; break;
            default: Debug.Log("No Music Track For This Scene Available"); break;
        }
        musicPlayer.Play();
    }

    public void ChangeTrackTo(string scene)
    {
        Debug.Log("ChangeTrack");

        switch (scene)
        {
            case "_OpeningMenu": musicPlayer.clip = trackToPlay[0]; break;
            case "_Level-1": musicPlayer.clip = trackToPlay[1]; break;
            case "_Level-2": musicPlayer.clip = trackToPlay[2]; break;
            case "_Level-3": musicPlayer.clip = trackToPlay[3]; break;
            default: Debug.Log("No Music Track For This Scene Available"); break;
        }
        musicPlayer.Play();
    }

    public void StartChangeTrackAndFadeOutTo(string scene)
    {
        StartCoroutine(ChangeTrackAndFadeOutTo(scene));
    }

    private IEnumerator ChangeTrackAndFadeOutTo(string scene)
    {
        while (musicPlayer.volume > 0)
        {
            musicPlayer.volume = musicPlayer.volume - .1f;
            yield return new WaitForSeconds(fadeSpeed);
        }
        ChangeTrackTo(scene);
        while (musicPlayer.volume <= 1)
        {
            musicPlayer.volume = musicPlayer.volume + .1f;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }

    private IEnumerator FadeOutTrack()
    {
        while(musicPlayer.volume > 0)
        {
            musicPlayer.volume = musicPlayer.volume - .1f;
            yield return new WaitForSeconds(fadeSpeed);
        }
        StartCoroutine(FadeInTrack());
    }

    private IEnumerator FadeInTrack()
    {
        while(musicPlayer.volume <= 1)
        {
            musicPlayer.volume = musicPlayer.volume + .1f;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
