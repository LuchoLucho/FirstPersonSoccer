using Assets.Scripts.Camera;
using SaavedraCraft.Model.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour,ICameraController {

    public float dragSpeed = 0.5f;
    private Vector3 dragOrigin;
    //private float timeOfCameraMove;

    private bool isButtonHolding = false;

    public Component MarkerExampleComponent;

    private ICameraObserver singleCameraObserver;

    public void AddCameraObserver(ICameraObserver newCameraObserver)
    {
        singleCameraObserver = newCameraObserver;
    }

    public void RemoveCameraObserver(ICameraObserver cameraObserverToBeRemoved)
    {
        singleCameraObserver = null;
    }

    public void UpdateCameraNewPos()
    {
        if (singleCameraObserver == null)
        {
            return;
        }
        singleCameraObserver.NotifyCameraNewPos();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //Esto solo se dispara cuando apretas el mouse, cuando haces hold ya no entra de vuelta.
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("dragOrigin: " + dragOrigin);
            //float[] ijFromCamera = MapRender.turnRealVector3CoordIntoIJ(transform.position.x, transform.position.y);
            //MapRender.GetInstance().AddNewMarkerToIJCoord(MarkerExampleComponent, ijFromCamera[0], ijFromCamera[1]);
            isButtonHolding = true;
        }
        else
        {
            if (Input.GetMouseButtonUp(1))
            {
                isButtonHolding = false;
            }
        }


        if (isButtonHolding)
        {            
            Vector3 deltaVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - dragOrigin;

            Vector3 move = -deltaVec;

            transform.Translate(move, Space.World);
            this.UpdateCameraNewPos();

            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
