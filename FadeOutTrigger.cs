using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutTrigger : MonoBehaviour {

    [SerializeField]
    private GameObject fadeOutPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        fadeOutPanel.GetComponent<Animator>().SetTrigger("fadeOut");
    }
}
