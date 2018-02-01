using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrollBackground : MonoBehaviour {

    [SerializeField]
    private float scrollSpeed;

    [SerializeField]
    private float scrollBuffer;

    private Transform[] layers;

    private int leftIndex;

    private int rightIndex;      

    Camera cam;
    float camHeight;
    float camWidth;

	// Use this for initialization
	void Start () {
        layers = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;

        rightIndex = layers.Length - 1;

        cam = FindObjectOfType<Camera>();

        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
	}
	
	// Update is called once per frame
	void Update () {
        
        //Move the right most slide behind all the slides when it passes the right edge of the camera + scrollBuffer
        if(layers[rightIndex].position.x - 5 > (camWidth/2 + scrollBuffer))
        {
            ScrollRight();
        }

        GetComponent<Transform>().Translate(new Vector3(scrollSpeed * Time.deltaTime, 0, 0));
	}

    private void ScrollRight()
    {
        layers[rightIndex].position = new Vector3(layers[leftIndex].position.x - 10, layers[leftIndex].position.y, layers[leftIndex].position.z);

        leftIndex = rightIndex;
        if (rightIndex == 0)
        {
            rightIndex = 4;
        }
        else
        {
            rightIndex--;
        }                        
    }

}
