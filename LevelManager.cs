using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private static LevelManager instance;

    [SerializeField]
    private float autoLoadNextLevelAfter;

    public float AutoLoadNextLevelAfter
    {
        get
        {
            return autoLoadNextLevelAfter;
        }

        set
        {
            autoLoadNextLevelAfter = value;
        }
    }

    private bool fadeOutSceneTrigger;

    public bool FadeOutSceneTrigger
    {
        get
        {
            return fadeOutSceneTrigger;
        }

        set
        {
            fadeOutSceneTrigger = value;
        }
    }

    public static LevelManager Instance
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

    [SerializeField]
    private GameObject fadePanel;

    // Use this for initialization
    void Start () {
        Instance = this;
        if (AutoLoadNextLevelAfter <= 0)
        {
            Debug.Log("Level auto load disabled, use a positive number in seconds");
        }
        else
        {
            Invoke("LoadNextLevel", AutoLoadNextLevelAfter);
        }
    }

    public void FadePanelToBlack()
    {
        fadePanel.GetComponent<Animator>().SetTrigger("FadeToBlack");
    }

    public IEnumerator LoadLevelIn3(string name)
    {
        FadePanelToBlack();
        yield return new WaitForSeconds(3);
        //MusicManager.Instance.ChangeTrackTo(name);
        LoadLevel(name);
    }

    public void LoadLevelin3Seconds(string name)
    {
        StartCoroutine(LoadLevelIn3(name));
    }

    public void LoadLevel(string name)
    {
        Debug.Log("Level load requested for: " + name);
        SceneManager.LoadScene(name);
    }

    public void QuitRequest()
    {
        Debug.Log("Quit Request Hit");
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
