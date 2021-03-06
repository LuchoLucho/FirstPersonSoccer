﻿using System;
using System.Collections.Generic;
using System.Text;
using SaavedraCraft.Model.Resources;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IResourceConsumer<T> : IConstruction<T>
    {
        List<IResource> GetNeeds(List<IResource> resources);
        List<IResource> GetResourceIntersectionWithProducer(IResourceProducer<T> producer, IResourceConsumer<T> consumerWithTheNeedMethod);
        void Buy(List<IResource> list);
        void AddToExternalResource(IResource newExternalResournce);
        List<IResource> getAllExternalResources();
        void SetCentralMarket(ICentralMarket<T> newCentralCommunicator);
    }
}
