using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Constructions
{
    public abstract class BasicContrucConsumer<T> : BasicContruction<T>, IResourceConsumer<T>
    {
        private List<IResource> externalResources = new List<IResource>();
        protected ICentralMarket<T> centralMarket;

        public BasicContrucConsumer(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralCommunicator) : base(aName, aComponent, newI, newj)
        {
            centralMarket = newCentralCommunicator;
        }        

        public void Buy(List<IResource> list)
        {
            foreach (IResource toAdd in list)
            {
                if (getAllExternalResources().Contains(toAdd))
                {
                    getAllExternalResources().Find(x => x.Equals(toAdd)).Add(toAdd.GetResourceAmount());
                }
                else
                {
                    AddToExternalResource(toAdd);
                }
            }
        }

        public void AddToExternalResource(IResource resources)
        {
            externalResources.Add(resources);
        }

        public List<IResource> getAllExternalResources()
        {
            return externalResources;
        }

        public abstract List<IResource> GetNeeds(List<IResource> resources);

        public List<IResource> GetResourceIntersectionWithProducer<T1>(IResourceProducer<T1> producer)
        {
            List<IResource> intersectionOfNeedsAndProvisionsFromProducer = producer.getAllProducedResources().FindAll(x => this.GetNeeds(new List<IResource> { x }).Contains(x));
            if ((intersectionOfNeedsAndProvisionsFromProducer.Count == 0) || (intersectionOfNeedsAndProvisionsFromProducer[0].GetResourceAmount() == 0))
            {
                return new List<IResource>();
            }
            List<IResource> resourcesIntersectionWithActualNeededAmount = cloneResourcesAndInactive(this.GetNeeds(intersectionOfNeedsAndProvisionsFromProducer));
            return resourcesIntersectionWithActualNeededAmount;
        }

        private List<IResource> cloneResourcesAndInactive(List<IResource> list)
        {
            List<IResource> toRet = new List<IResource>();
            foreach (IResource resource in list)
            {
                IResource newResource = resource.Clone();
                toRet.Add(newResource);
            }
            return toRet;
        }

        public override void SetActive(bool newValue)
        {
            base.SetActive(newValue);
            externalResources.ForEach(x => x.setActive(newValue));
        }

        public void SetCentralMarket(ICentralMarket<T> newCentralCommunicator)
        {
            centralMarket = newCentralCommunicator;
        }

        public override void TimeTick(float timedelta)
        {            
            if (GetNeeds(null).Count > 0 && needsHaveResourceAvailables())
            {
                centralMarket.AddConsumer(this);
            }
        }

        private bool needsHaveResourceAvailables()
        {
            foreach (IResource currentResource in GetNeeds(null))
            {
                if (currentResource.GetResourceAmount() > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
