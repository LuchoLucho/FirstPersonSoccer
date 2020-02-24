using System;
using System.Collections.Generic;
using System.Text;
using SaavedraCraft.Model.Constructions.Interfaces;
using SaavedraCraft.Model.Interfaces;

namespace SaavedraCraft.Model.Constructions
{
    abstract public class BasicConstrucHybridConsumerProducer<T> : BasicConstrucProducer<T> //Producer will be the father, consumer will be a property
, IHybridConsumerProducer<T>
    {
        protected IResourceConsumer<T> meAsConsumer;

        public BasicConstrucHybridConsumerProducer(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
        {
            meAsConsumer = getNewInstanceMeAsConsumer(aName+"-Consumer",aComponent,newI,newj, newCentralMarket);
        }

        public abstract BasicContrucConsumer<T>  getNewInstanceMeAsConsumer(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralCommunicator);

        public void AddToExternalResource(IResource newExternalResournce)
        {
            meAsConsumer.AddToExternalResource(newExternalResournce);
        }

        public void Buy(List<IResource> list)
        {
            meAsConsumer.Buy(list);
            newResoucesArrivedToBeTransformed(meAsConsumer);
        }        

        public List<IResource> getAllExternalResources()
        {
            return meAsConsumer.getAllExternalResources();
        }

        public List<IResource> GetNeeds(List<IResource> resources)
        {
            return meAsConsumer.GetNeeds(resources);
        }
        
        public List<IResource> GetResourceIntersectionWithProducer<T1>(IResourceProducer<T1> producer)
        {
            return meAsConsumer.GetResourceIntersectionWithProducer(producer);
        }

        public abstract void newResoucesArrivedToBeTransformed(IResourceConsumer<T> consumer);        
    }
}
