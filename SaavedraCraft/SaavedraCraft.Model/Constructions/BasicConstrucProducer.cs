using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Constructions
{
    public abstract class BasicConstrucProducer<T> : BasicContruction<T>, IResourceProducer<T>
    {
        private List<IResource> producedResources = new List<IResource>();
        protected ICentralMarket<T> centralMarket;

        public BasicConstrucProducer(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralMarket) : base(aName, aComponent, newI, newj)
        {
            centralMarket = newCentralMarket;
            producedResources.AddRange(AddInitialProducedResources());            
        }

        public abstract List<IResource> AddInitialProducedResources();        

        public List<IResource> getAllProducedResources()
        {
            return producedResources;
        }

        public Rectangle GetBroadCastingProductionArea()
        {
            throw new NotImplementedException();
        }        

        public void Sell(List<IResource> list)
        {            
            this.getAllProducedResources().FindAll(x => list.Contains(x)).ForEach(y => y.Subtract(list.Find(z => z.Equals(y)).GetResourceAmount()));
        }

        public override void SetActive(bool newValue)
        {
            base.SetActive(newValue);
            producedResources.ForEach(x => x.setActive(newValue));
            if (newValue)
            {
                centralMarket.AddProducer(this);
            }
            else
            {
                centralMarket.RemoveProducer(this);
            }
        }

        public void SetCentralMarket(ICentralMarket<T> newCentralCommunicator)
        {
            centralMarket = newCentralCommunicator;
        }
        
        public override void TimeTick(float timedelta)
        {
            foreach (IResource currentResource in producedResources)
            {
                int resourceCntPreviousTick = currentResource.GetResourceAmount();
                currentResource.TimeTick(timedelta);                
            }            
        }
    }
}
