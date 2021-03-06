﻿using System.Collections.Generic;
using SaavedraCraft.Model.Interfaces;

namespace SaavedraCraft.Model.Constructions.Interfaces
{
    public interface IHybridConsumerProducer<T> : IResourceConsumer<T>, IResourceProducer<T>
    {
        void AddToExternalResource(IResource newExternalResournce);
        void Buy(List<IResource> list);
        List<IResource> getAllExternalResources();
        List<IResource> GetNeeds(List<IResource> resources);
        BasicContrucConsumer<T> getNewInstanceMeAsConsumer(string aName, T aComponent, float newI, float newj);
        List<IResource> GetResourceIntersectionWithProducer(IResourceProducer<T> producer, IResourceConsumer<T> consumerWithTheNeedMethod);
    }
}