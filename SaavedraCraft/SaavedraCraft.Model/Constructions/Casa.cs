using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Utils;
using System;
using System.Collections.Generic;

namespace SaavedraCraft.Model.Constructions
{
    public class Casa<T> : IResourceProducer<T>, IResourceConsumer<T>
    {
        protected string name;
        protected T componentMolde;
        private int i, j;
        private bool active;
        protected ICentralMarket<T> centralMarket;

        protected T componentInstanciaReal;

        private List<IResource> producedResources = new List<IResource>();
        private List<IResource> externalResources = new List<IResource>();

        public Casa(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralCommunicator)
        {
            name = aName;
            componentMolde = aComponent;
            i = newI;
            j = newj;
            producedResources.AddRange(AddInitialProducedResources());
            active = false;
            this.SetCentralCommunicator(newCentralCommunicator);
        }

        public virtual List<IResource> AddInitialProducedResources()
        {
            return new List<IResource>() { new SimpleResource(4, "Persona/s") };
        }

        public virtual IConstruction<T> CloneMe()
        {
            return new Casa<T>(this.name, this.componentMolde, this.i, this.j, centralMarket);
        }

        public T GetComponentMolde()
        {
            return componentMolde;
        }

        public string GetConstructionInfo()
        {
            int totalNumberOfResources = 0;
            string toRet = this.GetName() + "\r\nProd:\r\n";
            producedResources.ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet += "Externo:\r\n";
            externalResources.ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet.TrimEnd(new char[] { '\r', '\n' });
            return toRet;
        }

        public int GetCoordI()
        {
            return i;
        }

        public int GetCoordJ()
        {
            return j;
        }

        public int GetHeigh()
        {
            return 1;
        }

        public string GetName()
        {
            return name;
        }

        public int GetWidh()
        {
            return 1;
        }

        public bool isActive()
        {
            return active;
        }

        public virtual void SetActive(bool newValue)
        {
            active = newValue;
            this.externalResources.ForEach(x => x.setActive(newValue));
            this.producedResources.ForEach(x => x.setActive(newValue));
        }

        public virtual void SetComponentInstanciaReal(T componentReal)
        {
            /*componentInstanciaReal = componentReal;
            ConstructionClickable construcClickable = componentReal.gameObject.GetComponent<ConstructionClickable>();
            if (construcClickable != null)
            {
                construcClickable.SetConstruction(this);
            } */           
        }

        public IConstruction<T> SetNewIJ(int v1, int v2)
        {
            this.i = v1;
            this.j = v2;
            return this;
        }

        public void TimeTick(float timedelta)
        {
            foreach (IResource currentResource in producedResources)
            {
                int resourceCntPreviousTick = currentResource.GetResourceAmount();
                currentResource.TimeTick(timedelta);
                if (currentResource.GetResourceAmount() > resourceCntPreviousTick)
                {
                    centralMarket.AddProducer(this);
                }
            }
            if (GetNeeds(null).Count > 0 && needsHaveResourceAvailables())
            {                
                centralMarket.AddConsumer(this);
            }
        }

        public bool needsHaveResourceAvailables()
        {
            foreach (IResource currentResource in GetNeeds(null))
            {
                if (currentResource.GetResourceAmount()>0)
                {
                    return true;
                }
            }
            return false;
        }


        public void SetCentralCommunicator(ICentralMarket<T> newCentralCommunicator)
        {
            centralMarket = newCentralCommunicator;
        }

        public List<IResource> getAllProducedResources()
        {
            return producedResources;
        }

        public List<IResource> getAllExternalResources()
        {
            return externalResources;
        }

        Rectangle IResourceProducer<T>.GetBroadCastingProductionArea()
        {
            return new Rectangle(this.GetCoordI(), this.GetCoordJ(), this.GetWidh(), this.GetHeigh());
        }

        public virtual void Sell(List<IResource> list)
        {
            throw new NotImplementedException();
        }

        public virtual List<IResource> GetNeeds(List<IResource> resources)
        {
            int neededAmount = 1;
            IResource singleNeed = new SimpleResource(neededAmount, "Tomates/s");
            IResource alreadyGotResource = externalResources.Find(x => x.Equals(singleNeed));
            if (alreadyGotResource?.GetResourceAmount()>= neededAmount)
            {
                return new List<IResource>();//My need was fulfil!!!
            }
            return new List<IResource> { singleNeed };
        }

        public List<IResource> GetResourceIntersectionWithProducer<T>(IResourceProducer<T> producer)
        {
            List<IResource> intersectionOfNeedsAndProvisionsFromProducer = producer.getAllProducedResources().FindAll(x => this.GetNeeds(new List<IResource> { x }).Contains(x));
            if ((intersectionOfNeedsAndProvisionsFromProducer.Count == 0) || (intersectionOfNeedsAndProvisionsFromProducer[0].GetResourceAmount()==0))
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

        public void Buy(List<IResource> list)
        {
            foreach (IResource toAdd in list)
            {
                if (externalResources.Contains(toAdd))
                {
                    externalResources.Find(x => x.Equals(toAdd)).Add(toAdd.GetResourceAmount());
                }
                else
                {
                    externalResources.Add(toAdd);
                }
            }
        }

        public override bool Equals(object obj)
        {
            IConstruction<T> other = obj as IConstruction<T>;
            if (other == null)
            {
                return false;
            }
            return this.GetCoordI() == other.GetCoordI() && this.GetCoordJ() == other.GetCoordJ();
        }        
    }
}
