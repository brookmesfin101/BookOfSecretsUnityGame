using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    [SerializeField]
    private bool scrolling, parallax;

    [SerializeField]
    private float backgroundSize;

    [SerializeField]
    private float parallaxSpeed;

    private float lastCameraX;
    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;

        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = layers.Length - 1;

    }

    private void Update()
    {
        if(parallax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
        }        

        lastCameraX = cameraTransform.position.x;

        if(scrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }
    }

    private void ScrollLeft()
    { 
        layers[rightIndex].localPosition = Vector3.right * (layers[leftIndex].localPosition.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
        //Debug.Log("Scrolling Left");
    }

    private void ScrollRight()
    {
        layers[leftIndex].localPosition = Vector3.right * (layers[rightIndex].localPosition.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
        //Debug.Log("Scrolling Right");
    }

}
