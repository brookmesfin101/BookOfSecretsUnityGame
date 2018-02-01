using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WealthBar : MonoBehaviour {

    private Text wealthBartext;

    public Text WealthBarText
    {
        get
        {
            return wealthBartext;
        }

        set
        {
            wealthBartext = value;
        }
    }

    // Use this for initialization
    void Start () {
        WealthBarText = GetComponent<Text>();
        WealthBarText.text = Player.Instance.PlayerWealth.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        WealthBarText.text = Player.Instance.PlayerWealth.ToString();
	}
}
