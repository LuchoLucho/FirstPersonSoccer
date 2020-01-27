using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Utils;
using System;
using System.Collections.Generic;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IResourceProducer<T> : IConstruction<T>
    {
        Rectangle GetBroadCastingProductionArea();
        void SetCentralMarket(ICentralMarket<T> newCentralCommunicator);
        void Sell(List<IResource> list);
        List<IResource> getAllProducedResources();
        List<IResource> AddInitialProducedResources();
    }
}
