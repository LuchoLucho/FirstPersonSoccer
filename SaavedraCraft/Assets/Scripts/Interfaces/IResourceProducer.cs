using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IResourceProducer : IConstruction
    {
        Rect GetBroadCastingProductionArea();
        void SetCentralCommunicator(ICentralResourcesCommunicator newCentralCommunicator);
        List<IResource> getAllResources();
    }
}
