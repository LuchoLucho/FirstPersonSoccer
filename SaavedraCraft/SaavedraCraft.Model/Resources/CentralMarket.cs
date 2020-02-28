using SaavedraCraft.Model.Constructions.Interfaces;
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
        public List<IHybridConsumerProducer<T>> hybrids = new List<IHybridConsumerProducer<T>>();

        private List<Transaction<T>> allTransactions = new List<Transaction<T>>();

        private Action<string> logAction = null;

        public void SetLogAction(Action<string> newAction)
        {
            logAction = newAction;
        }

        private void logMessage(string message)
        {
            if (logAction == null)
            {
                return;
            }
            logAction(message);
        }

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

        public void RemoveProducer(IResourceProducer<T> basicConstrucProducer)
        {
            producersWithProductionAvailable.Remove(basicConstrucProducer);
        }

        public void RemoveConsumer(IResourceConsumer<T> basicContrucConsumer)
        {
            consumerWithNeeds.Remove(basicContrucConsumer);
        }

        public List<Transaction<T>> GetTransactions()
        {
            List<Transaction<T>> ret = new List<Transaction<T>>();
            if ((producersWithProductionAvailable.Count == 0) || (consumerWithNeeds.Count == 0))
            {
                logMessage("There are no producers("+ producersWithProductionAvailable.Count+") or consumers("+ consumerWithNeeds.Count+")");
                return ret;
            }
            bool isThereProducerConsumerCloseEachOther = false;
            //I'm not using area instead distance:
            foreach (IResourceProducer<T> producer in producersWithProductionAvailable)
            {                
                foreach (IResourceConsumer<T> consumer in consumerWithNeeds)
                {
                    if (distanceBetweenConstructions(producer,consumer) < MAX_DISTANCE_PRODUCER_CONSUMER)
                    {
                        isThereProducerConsumerCloseEachOther = true;
                        List<IResource> resourceIntersection = consumer.GetResourceIntersectionWithProducer(producer,consumer);
                        if (resourceIntersection.Count > 0)
                        {
                            ret.Add(generateTransaction(consumer, producer, resourceIntersection));
                        }
                    }
                }
            }
            ret = removeTransactionInvolvingTheSameProducer(ret);
            ret.ForEach(x =>
            {
                if (!allTransactions.Contains(x))
                {
                    allTransactions.Add(x);
                }                
            }
            );
            if (!isThereProducerConsumerCloseEachOther)
            {
                logMessage("There are no producer consumer close enought!");
            }
            return ret;
        }

        private List<Transaction<T>> removeTransactionInvolvingTheSameProducer(List<Transaction<T>> ret)
        {
            List<IResourceProducer<T>> producersWithMultipleTransactions = new List<IResourceProducer<T>>();
            ret.ForEach(x =>
            {
                if ((ret.FindAll(y => y.getProducer().Equals(x.getProducer())).Count > 1 ) && (!producersWithMultipleTransactions.Contains(x.getProducer())))
                {
                    producersWithMultipleTransactions.Add(x.getProducer());
                }
            });
            foreach (IResourceProducer<T> currentProducer in producersWithMultipleTransactions)
            {
                List<Transaction<T>> allRepeatedTransactions = ret.FindAll(x=>x.getProducer() == currentProducer);
                allRepeatedTransactions.Remove(allRepeatedTransactions[0]);//Remove the first one so this get ignored and it is no removed later
                allRepeatedTransactions.ForEach(x => ret.Remove(x));
            }
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

        public void AddHybrid(IHybridConsumerProducer<T> hybrid)
        {
            hybrids.Add(hybrid);
            /*AddConsumer(hybrid.GetAsConsumer());
            AddProducer(hybrid.GetAsProducer());*/
            AddConsumer(hybrid);
            AddProducer(hybrid);
        }

        public void RemoveHybrid(IHybridConsumerProducer<T> basicConstrucHybridConsumerProducer)
        {
            hybrids.Remove(basicConstrucHybridConsumerProducer);
            RemoveConsumer(basicConstrucHybridConsumerProducer);
            RemoveProducer(basicConstrucHybridConsumerProducer);
        }
    }
}
