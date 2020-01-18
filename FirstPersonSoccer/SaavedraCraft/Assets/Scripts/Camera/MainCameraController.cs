using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class MainCameraController : MonoBehaviour, ICameraObserver
    {
        private List<ICameraController> allCameraControllers = new List<ICameraController>();
        private Vector2 previousCameraPosition = new Vector2();

        public const float CAMERA_RADIOUS_MAX = 9;
        
        void Start()
        {
            allCameraControllers.AddRange(GetComponents<ICameraController>());
            foreach (ICameraController currentCameraController in allCameraControllers)
            {
                currentCameraController.AddCameraObserver(this);
            }
        }

        public void NotifyCameraNewPos()
        {
            Vector2 camera2d = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            //Debug.Log("MainCameraController.NotifyCameraNewPos: " + camera2d + " Radious: " + camera2d.magnitude);
            //throw new NotImplementedException();
            checkLimits(camera2d);
            refreshSpritesBaseOnPosition(this.transform.position);
        }

        private void refreshSpritesBaseOnPosition(Vector3 cameraPosition)
        {
            //throw new NotImplementedException();
            Vector2 sprinteCoordIJ = ConstructionFactory.GetInstance().GetAproximateIJFromRealCoord(cameraPosition);
            //Debug.Log("getAproximateIJFromRealCoord = " + sprinteCoordIJ);
            Component toDestroy = ConstructionFactory.GetInstance().GetComponentFromIJCoord(sprinteCoordIJ.x, sprinteCoordIJ.y);
            //GameObject.Destroy(toDestroy.gameObject);
        }

        private void checkLimits(Vector2 camera2d)
        {
            if (camera2d.magnitude> CAMERA_RADIOUS_MAX)
            {
                Vector2 vectorInDirectionOfMoveOutOfRadious = camera2d; //- previousCameraPosition;
                vectorInDirectionOfMoveOutOfRadious.Normalize();
                vectorInDirectionOfMoveOutOfRadious *= CAMERA_RADIOUS_MAX * 0.99f;
                Vector3 translation = new Vector3(vectorInDirectionOfMoveOutOfRadious.x, vectorInDirectionOfMoveOutOfRadious.y,0) - new Vector3(camera2d.x,camera2d.y,0);
                this.gameObject.transform.Translate(translation);
            }
            previousCameraPosition = camera2d;
        }
    }
}
