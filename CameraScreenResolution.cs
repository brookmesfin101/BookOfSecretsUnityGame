using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenResolution : MonoBehaviour {

    private void Update()
    {
        Debug.Log("Camera pixel height " + GetComponent<Camera>().pixelHeight);
        Debug.Log("Camera pixel width " + GetComponent<Camera>().pixelWidth);
    }
}
