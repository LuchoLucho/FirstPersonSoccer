using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    public interface ICameraController
    {
        void AddCameraObserver(ICameraObserver newCameraObserver);
        void RemoveCameraObserver(ICameraObserver cameraObserverToBeRemoved);

        void UpdateCameraNewPos();
    }

    public interface ICameraObserver
    {
        void NotifyCameraNewPos();
    }
}
