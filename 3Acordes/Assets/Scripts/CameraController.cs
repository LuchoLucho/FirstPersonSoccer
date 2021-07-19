using Assets.Scripts;
using SaavedraCraft.Model.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraAnimationTotalLength = 10.0f;
    public Camera mainCamera;
    private MapManangerBehaviour mapManangerBehaviour;

    private Vector3 desiredPosition = new Vector3();
    private Vector3 positionDifferential = new Vector3();
    private float currentAnimation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        desiredPosition = mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAnimation > 0.0f)
        {
            //Debug.Log("currentAnimation mainCamera " + mainCamera.transform.position);
            mainCamera.transform.position += Time.deltaTime*positionDifferential;
            currentAnimation -= Time.deltaTime * 1.00f;
            if (currentAnimation<=0.0f)
            {
                mainCamera.transform.position = desiredPosition;
                currentAnimation = 0.0f;
            }
        }
    }

    public void MoveCameraTo(Vector3 destination)
    {
        Debug.Log("Camera should move!");
        if (mapManangerBehaviour == null)
        {
            mapManangerBehaviour = this.GetComponent<MapManangerBehaviour>();            
        }
        if (mapManangerBehaviour != null)
        {
            positionDifferential = destination - mainCamera.transform.position;
            //Debug.Log("mainCamera = " + mainCamera.transform.position);
            //Debug.Log("positionDifferential = " + positionDifferential);
            float dx = destination.x - mainCamera.transform.position.x;
            //Debug.Log("cameraAnimationTotalLength = " + cameraAnimationTotalLength);
            dx = dx / cameraAnimationTotalLength;
            positionDifferential = new Vector3(dx, 0.0f, 0.0f);//positionDifferential.y / cameraAnimationTotalLength, positionDifferential.z / cameraAnimationTotalLength);
            //Debug.Log("positionDifferential scaled = " + positionDifferential);
            currentAnimation = cameraAnimationTotalLength;
            desiredPosition = new Vector3(destination.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
    }
}
