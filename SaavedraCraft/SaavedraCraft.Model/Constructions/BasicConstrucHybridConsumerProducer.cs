﻿using System;
using System.Collections.Generic;
using System.Text;
using SaavedraCraft.Model.Constructions.Interfaces;
using SaavedraCraft.Model.Interfaces;

namespace SaavedraCraft.Model.Constructions
{
    abstract public class BasicConstrucHybridConsumerProducer<T> : BasicConstrucProducer<T> //Producer will be the father, consumer will be a property
, IHybridConsumerProducer<T>
    {
        private IResourceConsumer<T> meAsConsumer;

        public BasicConstrucHybridConsumerProducer(string aName, T aComponent, float newI, float newj, ICentralMarket<T> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
        {
            meAsConsumer = getNewInstanceMeAsConsumer(aName+"-Consumer",aComponent,newI,newj);            
        }

        public abstract BasicContrucConsumer<T> getNewInstanceMeAsConsumer(string aName, T aComponent, float newI, float newj);
        /*, ICentralMarket<T> newCentralCommunicator);*/
        //Consumer new instance should not have direct control of the market! 

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

        public virtual List<IResource> GetNeeds(List<IResource> resources)
        {
            return meAsConsumer.GetNeeds(resources);
        }
        
        public List<IResource> GetResourceIntersectionWithProducer(IResourceProducer<T> producer, IResourceConsumer<T> consumerWithTheNeedMethod)
        {
            return meAsConsumer.GetResourceIntersectionWithProducer(producer,this);
        }

        public abstract void newResoucesArrivedToBeTransformed(IResourceConsumer<T> consumer);

        public override void SetActive(bool newValue)
        {
            base.SetActive(newValue);
            if (newValue)
            {
                centralMarket.AddHybrid(this);
            }
            else
            {
                centralMarket.RemoveHybrid(this);
            }
        }        
    }
}
