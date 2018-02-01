using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private static HealthBar instance;

    [SerializeField]
    private Image healthbar;

    [SerializeField]
    private Text ratioText;

    private float hitpoints;
    private float maxhitPoints;

    public static HealthBar Instance
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

    private void Start()
    {
        hitpoints = Player.Instance.GetComponent<Health>().Life;
        maxhitPoints = Player.Instance.GetComponent<Health>().MaxLife;
        instance = this;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float ratio = hitpoints / maxhitPoints;
        healthbar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        ratioText.text = Mathf.Round((ratio * 100)).ToString() + '%';
    }

    public void TakeDamage(float damage)
    {
        hitpoints -= damage;
        if(hitpoints < 0)
        {
            hitpoints = 0;
        }

        UpdateHealthBar();
    }

    public void HealDamage(float heal)
    {
        hitpoints += heal;

        if (hitpoints >= maxhitPoints)
        {
            hitpoints = maxhitPoints;
        }

        UpdateHealthBar();
    }

    public void ResetHealthBar()
    {
        hitpoints = Player.Instance.GetComponent<Health>().Life;
        maxhitPoints = Player.Instance.GetComponent<Health>().MaxLife;
        UpdateHealthBar();
    }
}
