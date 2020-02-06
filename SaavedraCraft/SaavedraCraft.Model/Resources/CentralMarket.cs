using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Resources
{
    public class CentralMarket<T> : ICentralMarket<T>
    {
        public const int MAX_DISTANCE_PRODUCER_CONSUMER = 3;

        public List<IResourceProducer<T>> producersWithProductionAvailable = new List<IResourceProducer<T>>();
        public List<IResourceConsumer<T>> consumerWithNeeds = new List<IResourceConsumer<T>>();

        private List<Transaction<T>> allTransactions = new List<Transaction<T>>();

        public void AddProducer(IResourceProducer<T> resourceProducer)
        {
            //throw new NotImplementedException();
            //List<IResource> listOfAllResources = resourceProducer.getAllResources().FindAll(x => x.GetResourceAmount() > 0);
            //Debug.Log("New Resources Availables from " + resourceProducer + " Total: " + listOfAllResources[0].GetResourceAmount());
            if (!producersWithProductionAvailable.Contains(resourceProducer))
            {
                producersWithProductionAvailable.Add(resourceProducer);
            }
        }

        public void AddConsumer(IResourceConsumer<T> resourceConsumer)
        {
            if (!consumerWithNeeds.Contains(resourceConsumer))
            {
                consumerWithNeeds.Add(resourceConsumer);
            }
        }

        public List<Transaction<T>> GetTransactions()
        {
            List<Transaction<T>> ret = new List<Transaction<T>>();
            if ((producersWithProductionAvailable.Count == 0) || (consumerWithNeeds.Count == 0))
            {
                return ret;
            }
            //I'm not using area instead distance:
            foreach (IResourceProducer<T> producer in producersWithProductionAvailable)
            {                
                foreach (IResourceConsumer<T> consumer in consumerWithNeeds)
                {
                    if (distanceBetweenConstructions(producer,consumer) < MAX_DISTANCE_PRODUCER_CONSUMER)
                    {
                        List<IResource> resourceIntersection = consumer.GetResourceIntersectionWithProducer(producer);
                        if (resourceIntersection.Count > 0)
                        {
                            ret.Add(generateTransaction(consumer, producer, resourceIntersection));
                        }
                    }
                }
            }
            ret.ForEach(x =>
            {
                if (!allTransactions.Contains(x))
                {
                    allTransactions.Add(x);
                }                
            }
            );
            return ret;
        }

        private Transaction<T> generateTransaction(IResourceConsumer<T> consumer, IResourceProducer<T> producer, List<IResource> resourceIntersection)
        {
            return new Transaction<T>(consumer,producer, resourceIntersection);
        }

        private int distanceBetweenConstructions(IConstruction<T> c1, IConstruction<T> c2)
        {
            int squareDistance =  (c1.GetCoordI() - c2.GetCoordI()) * (c1.GetCoordI() - c2.GetCoordI()) +
                (c1.GetCoordJ()-c2.GetCoordJ())* (c1.GetCoordJ() - c2.GetCoordJ());
            return (int) Math.Sqrt(squareDistance);
        }

        private List<IResource> clonResourcesAndInactive(List<IResource> list)
        {
            List<IResource> toRet = new List<IResource>();
            foreach (IResource resource in list)
            {
                IResource newResource = resource.Clone();
                toRet.Add(newResource);
            }
            return toRet;
        }

        public List<Transaction<T>> GetAllTransactions()
        {
            return allTransactions;
        }

    }
}
