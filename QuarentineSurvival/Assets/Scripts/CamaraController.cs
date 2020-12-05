using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CamaraController : MonoBehaviour
    {
        public Camera mainCamera;
        public Camera chestCamera;
        private Camera currentCamera;
        public bool shouldCamaraFollowPlayer = true;
        private MapManangerBehaviour mapManangerBehaviour;

        public void Start()
        {
            SwitchToMainCamera();
        }

        public void SwitchToMainCamera()
        {
            mainCamera.enabled = true;
            chestCamera.enabled = false;
            currentCamera = mainCamera;
        }

        public void SwitchToChestCamera()
        {
            mainCamera.enabled = false;
            chestCamera.enabled = true;
            currentCamera = chestCamera;
        }

        public void Update()
        {
            if (mapManangerBehaviour == null)
            {
                mapManangerBehaviour = this.GetComponent<MapManangerBehaviour>();
                if (mapManangerBehaviour != null)
                {
                    if (shouldCamaraFollowPlayer)
                    {
                        mainCamera.transform.parent = mapManangerBehaviour.GetRealInstancePlayer().transform; // Camera is child of PLAYER<<<<<<<<<<<<<<<<<<<<<<<<<<
                    }
                }
            }
        }

        public Camera GetCurrentCamera()
        {
            return currentCamera;
        }
    }
}
