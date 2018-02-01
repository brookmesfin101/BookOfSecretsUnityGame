using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenseBar : MonoBehaviour {

    private static DefenseBar instance;

    [SerializeField]
    private Image defenseBar;

    [SerializeField]
    private Text ratioText;

    private float defensePoints;
    private float maxDefensePoints;

    public static DefenseBar Instance
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

    public float DefensePoints
    {
        get
        {
            return defensePoints;
        }

        set
        {
            defensePoints = value;
        }
    }

    public float MaxDefensePoints
    {
        get
        {
            return maxDefensePoints;
        }

        set
        {
            maxDefensePoints = value;
        }
    }

    // Use this for initialization
    void Start () {
        DefensePoints = Player.Instance.DefensePoints;
        MaxDefensePoints = Player.Instance.MaxDefensePoints;
        Instance = this;
        UpdateDefenseBar();
    }

    private void UpdateDefenseBar()
    {
        float ratio = DefensePoints / MaxDefensePoints;
        defenseBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        ratioText.text = Mathf.Round((ratio * 100)).ToString() + '%';
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ReduceDefense(float damage)
    {
        DefensePoints -= damage;
        if (DefensePoints < 0)
        {
            DefensePoints = 0;
        }

        UpdateDefenseBar();
    }

    public void RepairDefense(float heal)
    {
        DefensePoints += heal;

        if (DefensePoints >= MaxDefensePoints)
        {
            DefensePoints = MaxDefensePoints;
        }

        UpdateDefenseBar();
    }

    public void ResetDefenseBar()
    {
        DefensePoints = Player.Instance.DefensePoints;
        MaxDefensePoints = Player.Instance.MaxDefensePoints;
        UpdateDefenseBar();
    }
}
