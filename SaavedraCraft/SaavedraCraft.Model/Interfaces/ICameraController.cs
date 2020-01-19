using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
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
