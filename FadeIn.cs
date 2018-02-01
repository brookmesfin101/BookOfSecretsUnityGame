using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum FadeOutColor
{
    Black,
    White
};

public class FadeIn : MonoBehaviour {

    private Image panel;
    
    public FadeOutColor fadeColor;  // this public var should appear as a drop down

    [SerializeField]
    private bool autoFadeOutEnabled;

    float timer;

    [SerializeField]
    private float fadeInTime;

    [SerializeField]
    private float fadeOutTime;

    private float fadeOutDuration;

    float currentFadeInTime;

    float currentFadeOutTime;

    LevelManager levelManager;

    Color fadeInStartColor;

    Color fadeOutStartColor = new Color(0, 0, 0, 0);

    // Use this for initialization
    void Start () {
        fadeInStartColor = GetComponent<Image>().color;

        levelManager = GameObject.FindObjectOfType<LevelManager>();

        if(levelManager)
        {
            fadeOutDuration = levelManager.AutoLoadNextLevelAfter - fadeOutTime;
        }
    }
	
	// Update is called once per frame
	void Update () {

        timer = Time.timeSinceLevelLoad;

        //Fade In
        if (timer < fadeInTime)
        {
            FadeInTo(new Color(fadeInStartColor.r, fadeInStartColor.g, fadeInStartColor.b, 0));
        }            

        //Fade Out
        if(timer > fadeOutTime && autoFadeOutEnabled)
        {
            if (fadeColor == FadeOutColor.Black)
                AutoFadeOut(Color.black);
            else
                AutoFadeOut(Color.white);
        }

        //Debug.Log(timer);
	}

    private void FadeInTo(Color fadeToColor)
    {
        currentFadeInTime += Time.deltaTime;

        if(currentFadeInTime > fadeInTime)
        {
            currentFadeInTime = fadeInTime;
        }

        float percent = currentFadeInTime / fadeInTime;

        //easing in
        percent = 1f - Mathf.Cos(percent * Mathf.PI * .5f);

        //Debug.Log(currentLerpTime);

        Color inbetweenColor = Color.Lerp(fadeInStartColor, fadeToColor, percent);

        GetComponent<Image>().color = inbetweenColor;
        
        if(GetComponent<Image>().color.a <= .01)
        {
            GetComponent<RectTransform>().position = new Vector3(0, 0, -10);
        }        
    }

    private void AutoFadeOut(Color fadeToColor)
    {
        if(GetComponent<RectTransform>().position.z < 0)
            GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

        currentFadeOutTime += Time.deltaTime;

        if(currentFadeOutTime > fadeOutDuration)
        {
            currentFadeOutTime = fadeOutDuration;
        }

        float percent = currentFadeOutTime / fadeOutDuration;

        Color inbetweenColor = Color.Lerp(fadeOutStartColor, fadeToColor, percent);

        GetComponent<Image>().color = inbetweenColor;
    }
}
