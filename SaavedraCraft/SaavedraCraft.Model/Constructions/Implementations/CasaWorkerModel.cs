using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Constructions.Implementations
{
    public class CasaWorkerModel<T> : BasicConstrucHybridConsumerProducer<T>
    {
        public CasaWorkerModel(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
        {
        }

        public override List<IResource> AddInitialProducedResources()
        {
            int neededAmount = 0;
            IResource initialResource = new SimpleResource(neededAmount, "Worker/s");
            return new List<IResource> { initialResource };
        }

        public override IConstruction<T> CloneMe()
        {
            return new CasaWorkerModel<T>(this.name, this.componentMolde, i, j, centralMarket);
        }

        public override string GetConstructionInfo()
        {
            string toRet = this.GetName() + "\r\n";
            toRet += "Externo:\r\n";
            getAllExternalResources().ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet.TrimEnd(new char[] { '\r', '\n' });
            toRet += "Producido:\r\n";
            getAllProducedResources().ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet.TrimEnd(new char[] { '\r', '\n' });
            return toRet;
        }

        public override BasicContrucConsumer<T> getNewInstanceMeAsConsumer(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralCommunicator)
        {
            return new Casa<T>(aName, aComponent, newI, newj, newCentralCommunicator);
        }

        public override void newResoucesArrivedToBeTransformed(IResourceConsumer<T> meAsConsumer)
        {
            List<IResource> allResources = meAsConsumer.getAllExternalResources();
            if (allResources.Count > 0)
            {
                IResource singleTomatoe = allResources.Find(x => x.GetResourceName().Contains("Toma"));
                if (singleTomatoe.GetResourceAmount() > 0)
                {
                    //We transform the resources:
                    singleTomatoe.Subtract(1);
                    if (getAllProducedResources().FindAll(x => x.GetResourceName().Contains("Worker")).Count == 0)
                    {
                        IResource newResource = new SimpleResource(1, "Worker/s");
                        getAllProducedResources().Add(newResource);
                    }
                    else
                    {
                        IResource alreadyFoundResource = this.getAllProducedResources().Find(x => x.GetResourceName().Contains("Worker"));
                        alreadyFoundResource.Add(1);
                    }
                }
            }
        }
    }
}
