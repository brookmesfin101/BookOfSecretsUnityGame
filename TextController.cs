using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    string[] missionText = new string[] { "You are a ninja.", "With dreadlocks.", "Your mission,", "is to break into a secure facility and", "obtain the Book of Secret Knowledge.", "Do not fail"};
    private int currentText = 0;

    [SerializeField]
    private float typeWriterSpeed;

    [SerializeField]
    private Image fadePanel;

    [SerializeField]
    private Text nextButtonText;

    [SerializeField]
    private Collider2D bottomCollider;

    private bool displayAllText = false;

    private bool animateTextComplete;

    private bool fadeInStarted = false;

    private bool enablePlayer = false;

    private bool fadeInComplete = false;

    [SerializeField]
    private GameObject player;

    // Use this for initialization
    void Start () {
        StartCoroutine(AnimateText());
	}
	
	// Update is called once per frame
	void Update () {

        if (enablePlayer)
            player.GetComponent<Player>().enabled = true;

        if(fadeInComplete)
            nextButtonText.text = "Infiltrate";
    }

    public void NextText()
    {
        if(nextButtonText.text != "Infiltrate")
        {
            if (displayAllText == false && animateTextComplete == false)
            {
                StopAllCoroutines();
                GetComponent<Text>().text = missionText[currentText];
                displayAllText = true;
            }
            else
            {
                if (currentText >= missionText.Length - 1)
                {
                    //Don't go over length mission Text Array
                    if (!fadeInStarted)
                    {
                        StartCoroutine(FadeInPanel());
                        fadeInStarted = true;
                    }
                }
                else
                {
                    currentText++;
                    StartCoroutine(AnimateText());
                }
                displayAllText = false;
            }
        }
        else
        {
            Debug.Log("Bottom Collider disabled");
            bottomCollider.enabled = false;
        }        
    }

    private IEnumerator AnimateText()
    {
        animateTextComplete = false;
        for (int i = 0; i < (missionText[currentText].Length+1); i++)
        {
            GetComponent<Text>().text = missionText[currentText].Substring(0, i);
            yield return new WaitForSeconds(typeWriterSpeed);
        }
        animateTextComplete = true;
    }

    private IEnumerator FadeInPanel()
    {
        if(fadePanel)
        {
            float lerpDuration = 15;
            float current = 0;

            while (current < lerpDuration)
            {
                current += .1f;

                enablePlayer = true;
                
                if(current > 3)
                    fadeInComplete = true;

                Color tempColor = Color.Lerp(fadePanel.color, new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0), current / lerpDuration);

                fadePanel.color = tempColor;
                yield return new WaitForSeconds(.1f);
            }         
        }
    }
}
