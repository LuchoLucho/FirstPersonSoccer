﻿using System;
using System.Collections.Generic;
using System.Text;
using SaavedraCraft.Model.Interfaces;

namespace SaavedraCraft.Model.Resources
{
    public class Transaction<T>
    {
        private IResourceConsumer<T> consumer;
        private IResourceProducer<T> producer;
        private List<IResource> intersectionOfNeedsAndProvisionsFromProducer;
        private bool wasApplied = false;

        public Transaction(IResourceConsumer<T> consumer, IResourceProducer<T> producer, List<IResource> intersectionOfNeedsAndProvisionsFromProducer)
        {
            this.consumer = consumer;
            this.producer = producer;
            this.intersectionOfNeedsAndProvisionsFromProducer = intersectionOfNeedsAndProvisionsFromProducer;
        }

        public IResourceProducer<T> getProducer()
        {
            return producer;
        }

        public IResourceConsumer<T> getConsumer()
        {
            return consumer;
        }

        public List<IResource> getResources()
        {
            return intersectionOfNeedsAndProvisionsFromProducer;
        }

        public void DebitarAcreditar()
        {            
            if (wasApplied)
            {
                return;
            }
            this.producer.Sell(getResources());
            this.consumer.Buy(getResources());
            wasApplied = true;
        }

        public override string ToString()
        {
            return "Transac between: " + getProducer().ToString() + " and " + getConsumer() + " Resources: " + getResources()[0].ToString();
        }

    }
}
